using Auth02_AspNetIdentity.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NETCore.MailKit.Extensions;
using NETCore.MailKit.Infrastructure.Internal;

namespace Auth02_AspNetIdentity
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(config =>
            {
                config.UseInMemoryDatabase("RAM");
            });

            // Add identity registers the services
            services
                .AddIdentity<IdentityUser, IdentityRole>(config =>
                {
                    config.Password.RequiredLength = 4;
                    config.Password.RequireDigit = false;
                    config.Password.RequireLowercase = false;
                    config.Password.RequireUppercase = false;
                    config.Password.RequireNonAlphanumeric = false;

                    config.SignIn.RequireConfirmedEmail = true;
                    config.SignIn.RequireConfirmedAccount = false;
                    config.SignIn.RequireConfirmedPhoneNumber = false;
                })
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders(); // Set the token provider for send email confirmation token.

            services
                .ConfigureApplicationCookie(config =>
                {
                    config.LoginPath = "/home/login";
                    config.Cookie.Name = "Identity.Cookie";
                });

            services.AddMailKit(config =>
            {
                config.UseMailKit(_configuration.GetSection(nameof(MailKitOptions)).Get<MailKitOptions>());
            });

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
