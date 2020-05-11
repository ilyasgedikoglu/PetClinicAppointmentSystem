using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PetClinicAppointmentSystem.Filters;
using PetClinicAppointmentSystem.Model.Infrastructure;

namespace PetClinicAppointmentSystem
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
                    Title = "Turna Medar API",
                    Description = "ASP.NET Core Web API",
                    TermsOfService = new Uri("https://turnateknoloji.com"),
                    Contact = new OpenApiContact
                    {
                        Name = "Turna Teknoloji",
                        Email = string.Empty,
                        Url = new Uri("https://turnateknoloji.com")
                    }
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Description = "Authorization header using the Bearer scheme",
                    Name = "Authorization",
                    In = ParameterLocation.Header
                });

                c.DocumentFilter<SwaggerSecurityDocumentFilter>();

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
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void InitializeContainer(IServiceCollection services)
        {
            services.AddSingleton<Microsoft.AspNetCore.Http.IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<YetkiKontrol>();

            //services.AddTransient<IRepository<Kullanici>, Repository<Kullanici>>();
            //services.AddTransient<IKullaniciRepository, KullaniciRepository>();
            //services.AddTransient<IKullaniciService, KullaniciService>();
            //services.AddTransient<IRepository<Dokuman>, Repository<Dokuman>>();
            //services.AddTransient<IDokumanRepository, DokumanRepository>();
            //services.AddTransient<IDokumanService, DokumanService>();
            //services.AddTransient<IRepository<DokumanEtiket>, Repository<DokumanEtiket>>();
            //services.AddTransient<IDokumanEtiketRepository, DokumanEtiketRepository>();
            //services.AddTransient<IDokumanEtiketService, DokumanEtiketService>();

            //services.AddTransient<IRepository<Etiket>, Repository<Etiket>>();
            //services.AddTransient<IEtiketRepository, EtiketRepository>();
            //services.AddTransient<IEtiketService, EtiketService>();


            //services.AddTransient<IRepository<IletisimBilgisi>, Repository<IletisimBilgisi>>();
            //services.AddTransient<IIletisimBilgisiRepository, IletisimBilgisiRepository>();
            //services.AddTransient<IIletisimBilgisiService, IletisimBilgisiService>();


            //services.AddTransient<IRepository<Yetki>, Repository<Yetki>>();
            //services.AddTransient<IYetkiRepository, YetkiRepository>();
            //services.AddTransient<IYetkiService, YetkiService>();

            //services.AddTransient<IRepository<Grup>, Repository<Grup>>();
            //services.AddTransient<IGrupRepository, GrupRepository>();
            //services.AddTransient<IGrupService, GrupService>();

            //services.AddTransient<IRepository<KullaniciGrup>, Repository<KullaniciGrup>>();
            //services.AddTransient<IKullaniciGrupRepository, KullaniciGrupRepository>();
            //services.AddTransient<IKullaniciGrupService, KullaniciGrupService>();

            //services.AddTransient<IRepository<Kod>, Repository<Kod>>();
            //services.AddTransient<IKodRepository, KodRepository>();
            //services.AddTransient<IKodService, KodService>();

            //services.AddTransient<ILisansKontrolService, LisansKontrolService>();

            //services.AddTransient<IHesapService, HesapService>();

            //services.AddTransient<IRepository<MailAyar>, Repository<MailAyar>>();
            //services.AddTransient<IMailAyarRepository, MailAyarRepository>();
            //services.AddTransient<IMailAyarService, MailAyarService>();

            //services.AddTransient<IRepository<Klasor>, Repository<Klasor>>();

            //services.AddTransient<IRepository<Boyut>, Repository<Boyut>>();
            //services.AddTransient<IBoyutRepository, BoyutRepository>();
            //services.AddTransient<IBoyutService, BoyutService>();

            //services.AddTransient<IRepository<PaylasilabilirLink>, Repository<PaylasilabilirLink>>();
            //services.AddTransient<IPaylasilabilirLinkRepository, PaylasilabilirLinkRepository>();
            //services.AddTransient<IPaylasilabilirLinkService, PaylasilabilirLinkService>();

            //services.AddTransient<IRepository<KullaniciHareket>, Repository<KullaniciHareket>>();
            //services.AddTransient<IKullaniciHareketRepository, KullaniciHareketRepository>();
            //services.AddTransient<IKullaniciHareketService, KullaniciHareketService>();

            //services.AddTransient<IRepository<GenelAyar>, Repository<GenelAyar>>();
            //services.AddTransient<IGenelAyarRepository, GenelAyarRepository>();
            //services.AddTransient<IGenelAyarService, GenelAyarService>();

            //services.AddTransient<ILdapService, LdapService>();


            //services.AddTransient<IRepository<DokumanGrup>, Repository<DokumanGrup>>();
            //services.AddTransient<IDokumanGrupRepository, DokumanGrupRepository>();
            //services.AddTransient<IDokumanGrupService, DokumanGrupService>();

            //services.AddTransient<IRepository<Job>, Repository<Job>>();
            //services.AddTransient<IJobRepository, JobRepository>();
            //services.AddTransient<IJobService, JobService>();

            //services.AddTransient<ILogRepository, LogRepository>();
            //services.AddTransient<ILogService, LogService>();

            //services.AddScoped<IRepository<Giris>, Repository<Giris>>();
            //services.AddScoped<IGirisRepository, GirisRepository>();
            //services.AddScoped<IGirisService, GirisService>();

            //services.AddScoped<IRepository<EtiketResim>, Repository<EtiketResim>>();
            //services.AddScoped<IEtiketResimRepository, EtiketResimRepository>();
            //services.AddScoped<IEtiketResimService, EtiketResimService>();
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
