using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using DotNetCoreMVC.Models;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using DotNetCoreMVC.Services;
using Microsoft.AspNetCore.Http;

namespace DotNetCoreMVC
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
            services.Configure<NumberCounterConfig>(Configuration.GetSection("Counting"));

            services.AddSingleton<NumberCounterSingleton>();
            services.AddScoped<NumberCounterScoped>();
            services.AddTransient<NumberCounterTransient>();

            services.AddTransient<NumberCounterDependent>();

            services.AddControllersWithViews();

            //Authentication
            //TODO: Authority from appsettings and secrets store
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = "oidc";
            })
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
                //options =>
                //{
                //    options.LoginPath = new PathString("/auth/login");
                //    options.AccessDeniedPath = new PathString("/auth/denied");
                //})
            .AddOpenIdConnect("oidc", options =>
            {
                options.Authority = "https://localhost:5004";
                options.ClientId = "mvc.client";
                options.ClientSecret = "SuperSecretPassword";
                options.CallbackPath = "/signin";

                options.Scope.Add("weatherapi.read");
                //options.Scope.Add("openid");
                //options.Scope.Add("profile");

                options.SaveTokens = true;
                options.GetClaimsFromUserInfoEndpoint = true;

                //options.ClaimActions.MapUniqueJsonKey("CareerStarted","CareerStarted");
                //options.ClaimActions.MapUniqueJsonKey("Role", "Role");
                //options.ClaimActions.MapUniqueJsonKey("Permission", "permission");

                options.ResponseType = "code";
                options.ResponseMode = "query";
                options.UsePkce = true;
            });

            services.Configure<IdentityServerSettings>(Configuration.GetSection("IdentityServerSettings"));
            services.AddSingleton<ITokenService, TokenService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
