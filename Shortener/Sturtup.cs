using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Shortener.Models;
using Shortener.Services;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Shortener
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = Configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddSingleton<ShortenLinkGenerator>();


            services.AddControllersWithViews();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = "oidc";
            })
   .AddCookie()
   .AddOpenIdConnect("oidc", options =>
   {
       options.Authority = "https://your-identity-server";
       options.ClientId = "your-client-id";
       options.ClientSecret = "your-client-secret";
       options.ResponseType = "code";

        // Додаткові налаштування...

        options.Scope.Add("openid");
       options.Scope.Add("profile");
       options.SaveTokens = true;
   });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
            });

            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin", builder =>
                {
                    builder.WithOrigins("https://localhost:44467") // Вкажіть ваш домен
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
                options.AddPolicy("AllowAngularDevClient", builder =>
                {

                    // Дозволяємо запити з Angular додатку, наприклад, коли ви розробляєте локально
                    builder.WithOrigins("http://localhost:44467")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseWebSockets();
            app.Use(async (context, next) =>
            {
                if (context.WebSockets.IsWebSocketRequest)
                {
                    var webSocket = await context.WebSockets.AcceptWebSocketAsync();
                    // Обробляйте WebSocket-запити тут
                }
                else
                {
                    await next();
                }
            });
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseCors("AllowAngularDevClient");
            app.UseCors("AllowSpecificOrigin");

            app.UseAuthorization();
            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Link}/{action=Shorten}/{id?}");
            });
        }
    }
}