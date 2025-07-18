using AutoMapper;
using HelpDeskSystem.ClaimsManagement;
using HelpDeskSystem.Data;
using HelpDeskSystem.Models;
using HelpDeskSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HelpDeskSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders()
                .AddDefaultUI();
            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();

            .AddCookie(options =>
             {
                 options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                 options.SlidingExpiration = true;
                 options.Cookie.Name = ".HelpDesk.Session";
                 options.Cookie.HttpOnly = true;
                 options.Cookie.IsEssential = true;
             });

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                options.LoginPath = "/Identity/Account/Login";
            });

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("Permission", policyBuilder =>
                {
                    policyBuilder.Requirements.Add(new PermissionAuthorizationRequirement());
                });
            });

            builder.Services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();

            var config = new MapperConfiguration(
                cfg =>
                {
                    if (cfg != null)
                    {
                        cfg.SourceMemberNamingConvention = new PascalCaseNamingConvention();
                        cfg.DestinationMemberNamingConvention = new PascalCaseNamingConvention();
                        cfg.AllowNullDestinationValues = true;
                        cfg.AddProfile(new AutomapperProfileService());
                    }
                });

            var mapper = config.CreateMapper();
            builder.Services.AddSingleton(mapper);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {

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

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            app.Run();
        }
    }
}



