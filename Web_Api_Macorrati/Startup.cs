using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.CodeAnalysis.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Web_Api_Macorrati.Context;
using Web_Api_Macorrati.DTOs.Mappings;
using Web_Api_Macorrati.Extensions;
using Web_Api_Macorrati.Filters;
using Web_Api_Macorrati.Logging;
using Web_Api_Macorrati.Repository;
using Web_Api_Macorrati.Services;

namespace Web_Api_Macorrati
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("PermitirApiRequest",
                    builder =>
                    builder.WithOrigins("http://apirequest.io/")
                    .WithMethods("GET")
                    );
            });

            //Config Auto mapper
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddScoped<ApiLoggingFilter>();
            //Registrando padrão Unit Of Work como um serviço 
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddDbContext<AppDbContext>(options =>
            options.UseMySql(Configuration.GetConnectionString("DefaultConnection")));

            //Registrando Identity
            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication(
                JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidAudience = Configuration["TokenConfiguration:Audience"],
                    ValidIssuer = Configuration["TokenConfiguration:Issuer"],
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(Configuration["Jwt:key"]))
                });

            //Swagger 
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "CatalogoAPI",
                    Description = "Catálogo de Produtos e Categorias",
                    TermsOfService = new Uri("https://douglas.net/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "macoratti",
                        Email = "macoratti@yahoo.com",
                        Url = new Uri("https://www.douglas.net"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Usar sobre LICX",
                        Url = new Uri("https://douglas.net/license"),
                    }
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);

                var security = new Dictionary<string, IEnumerable<string>>
                {
                    {"Bearer", new string[] { } },
                };
            });

            //services.AddApiVersioning(options =>
            //{
            //    options.AssumeDefaultVersionWhenUnspecified = true;
            //    options.DefaultApiVersion = new ApiVersion(1, 0);
            //    options.ReportApiVersions = true;
            //    options.ApiVersionReader = new HeaderApiVersionReader("x-api-version");
            //});

            //services.AddTransient<IMeuServico, MeuServico>();

            services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling
                    = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //loggerFactory.AddProvider(new CustomLoggerProvider(new CustomLoggerProviderConfiguration
            //{
            //    LogLevel = LogLevel.Information
            //})); 

            //Adicionando o middleware de tratamento de erros 
            app.ConfigureExceptionHandler();

            app.UseHttpsRedirection();

            app.UseRouting();
            //middleware de autenticação 
            app.UseAuthentication();
            //middleware de autorização
            app.UseAuthorization();

            //Swagger
            app.UseSwagger();

            //SwaggerUI
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json",
                    "Cátalogo de Produtos e Categorias");
            });

            ////CORS
            //app.UseCors();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
