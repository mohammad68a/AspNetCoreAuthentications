using Auth03_PolicyAndClaims.AuthRequirements;
using Auth03_PolicyAndClaims.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Security.Claims;

namespace Auth03_PolicyAndClaims
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(SchemaNames.CookieAuth)
                .AddCookie(SchemaNames.CookieAuth, config =>
                {
                    config.Cookie.Name =  CookieNames.AuthCookie;
                    config.LoginPath = "/home/authenticate";
                });

            services.AddAuthorization(config =>
            {
                // first way (inline)
                //var defaultAuthBuilder = new AuthorizationPolicyBuilder();
                //var defaultAuthPolicy = defaultAuthBuilder
                //    .RequireAuthenticatedUser()
                //    .RequireClaim(ClaimTypes.MobilePhone)
                //    .Build();
                //config.DefaultPolicy = defaultAuthPolicy;

                // second way (using external handler)
                config.AddPolicy(PolicyNames.MobileRequired, builder =>
                {
                    builder.RequireAuthenticatedUser();
                    builder.RequireCustomClaim(ClaimTypes.MobilePhone);
                });
            });

            // Adding CustomRequireClaimHandler to the DI container 
            // If you injected some database services in CustomRequireClaimHandler you must AddScoped instead of singletone
            services.AddSingleton<IAuthorizationHandler, CustomRequireClaimHandler>();

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
