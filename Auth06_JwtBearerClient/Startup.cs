using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Auth06_JwtBearerClient
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(config =>
            {
                // We check the cookie to confirm that we are authenticated.
                config.DefaultAuthenticateScheme = Constants.ClientCookie;

                // When we sign in we will deal out a cookie.
                config.DefaultSignInScheme = Constants.ClientCookie;

                // Use this to check if we are allowed to do something.
                config.DefaultChallengeScheme = Constants.JwtServer;
            })
            .AddCookie(Constants.ClientCookie)
            .AddOAuth(Constants.JwtServer, config =>
            {
                config.ClientId = "client_id";
                config.ClientSecret = "client_secret";
                config.CallbackPath = "/oauth/callback";
                config.AuthorizationEndpoint = "http://localhost:10957/oauth/authorize"; //-server endpoint
                config.TokenEndpoint = "http://localhost:10957/oauth/token";
                config.SaveTokens = true;
                config.Events = new OAuthEvents()
                {
                    OnCreatingTicket = context =>
                    {
                        var token = new JwtSecurityTokenHandler().ReadToken(context.AccessToken) as JwtSecurityToken;
                        foreach (var claim in token.Claims)
                        {
                            context.Identity.AddClaim(new Claim(claim.Type, claim.Value));
                        }
                        return Task.CompletedTask;
                    }
                };
            });

            services.AddHttpClient();

            services.AddControllersWithViews()
                .AddRazorRuntimeCompilation(); // Added via package `Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation`
        }

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
