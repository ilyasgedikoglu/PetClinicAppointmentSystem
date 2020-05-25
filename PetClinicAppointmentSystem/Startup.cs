using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PetClinicAppointmentSystem.Extensions;
using PetClinicAppointmentSystem.Filters;
using PetClinicAppointmentSystem.Model.Entity;
using PetClinicAppointmentSystem.Model.Infrastructure;
using PetClinicAppointmentSystem.Repository;
using PetClinicAppointmentSystem.Repository.Interfaces;
using PetClinicAppointmentSystem.Service;
using PetClinicAppointmentSystem.Service.Interfaces;

namespace PetClinicAppointmentSystem
{
    public class Startup
    {
        private readonly IWebHostEnvironment _hostingEnv;
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            _hostingEnv = env;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddScoped<YetkiKontrol>();

            //services.AddTransient<IKullaniciRolService, KullaniciRolService>();
            services.AddDbContext<DatabaseContext>(x => x.UseNpgsql(Configuration.GetConnectionString("DivaConnection")));

            services.AddCors(options =>
            {
                options.AddPolicy("EnableCORS", builder =>
                {
                    builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod().Build();
                });
            });

            InitializeContainer(services);

            services.AddMvc(options =>
            {
            }).AddJsonOptions(options => { });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "SENG 448 Project API",
                    Description = "ASP.NET Core Web API Pet Appointment System Project",
                    TermsOfService = new Uri("https://www.cankaya.edu.tr/"),
                    Contact = new OpenApiContact
                    {
                        Name = "Hande Aydın",
                        Email = "c1511410@student.cankaya.edu.tr",
                        Url = new Uri("https://www.cankaya.edu.tr/")
                    }
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Description = "Authorization header using the Bearer scheme",
                    Name = "Authorization",
                    In = ParameterLocation.Header
                });

                c.DocumentFilter<SwaggerSecurityDocumentFilter>();
                c.OperationFilter<DosyaYuklemeOperationFilter>();
            });


            // services.AddAutoMapper();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(jwtBearerOptions =>
                {
                    jwtBearerOptions.RequireHttpsMetadata = false;
                    jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateActor = true,
                        ValidateAudience = false,
                        ValidateIssuer = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:SigningKey"]))
                    };
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();

            app.UseDefaultFiles();
            
            app.UseStaticFiles();

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseCors("EnableCORS");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();
            app.UseAuthorization();

            // kullanıcının kaç kere giriş yaptığını kontrol eden middware
            app.UseRequestToken();

            // genel hata yakalama middware
            app.UseGenelExceptionMiddlewareExtensions();


            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            //// Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            //// specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Seng 448 Project API V1");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void InitializeContainer(IServiceCollection services)
        {
            services.AddSingleton<Microsoft.AspNetCore.Http.IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<YetkiKontrol>();

            services.AddTransient<IRepository<User>, Repository<User>>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IUserService, UserService>();

            services.AddTransient<IRepository<Yetki>, Repository<Yetki>>();
            services.AddTransient<IYetkiRepository, YetkiRepository>();
            services.AddTransient<IYetkiService, YetkiService>();

            services.AddTransient<IRepository<Pet>, Repository<Pet>>();
            services.AddTransient<IPetRepository, PetRepository>();
            services.AddTransient<IPetService, PetService>();

            services.AddTransient<IRepository<Appointment>, Repository<Appointment>>();
            services.AddTransient<IAppointmentRepository, AppointmentRepository>();
            services.AddTransient<IAppointmentService, AppointmentService>();

            services.AddTransient<IRepository<AvailableAppointmentTime>, Repository<AvailableAppointmentTime>>();
            services.AddTransient<IAvailableAppointmentRepository, AvailableAppointmentRepository>();
            services.AddTransient<IAvailableAppointmentService, AvailableAppointmentService>();

            services.AddScoped<IRepository<Giris>, Repository<Giris>>();
            services.AddScoped<IGirisRepository, GirisRepository>();
            services.AddScoped<IGirisService, GirisService>();
        }

        private static void SeedDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<DatabaseContext>();
                // Seed the database.
            }
        }
    }
}
