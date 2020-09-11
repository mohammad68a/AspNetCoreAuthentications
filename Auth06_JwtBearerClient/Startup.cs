using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
                        var accessToken = context.AccessToken;
                        var payloadBase64 = accessToken.Split('.')[1];
                        var bytes = Convert.FromBase64String(payloadBase64);
                        var jsonPayload = Encoding.UTF8.GetString(bytes);
                        var claims = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonPayload);
                        foreach (var claim in claims)
                        {
                            context.Identity.AddClaim(new Claim(claim.Key, claim.Value));
                        }
                        return Task.CompletedTask;
                    }
                };
            });

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
