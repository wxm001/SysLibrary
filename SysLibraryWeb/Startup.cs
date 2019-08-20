using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SysLibraryWeb.Data;

namespace SysLibraryWeb
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.UI.Services;
    using SysLibraryWeb.Infrastructure;
    using SysLibraryWeb.Models;

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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            //----------------------
            services.AddDbContext<LendingInfoDbContext>(options =>
                {
                    options.UseSqlServer(Configuration.GetConnectionString("LendingInfoDbContext"));
                });
            services.AddDbContext<StudentIdentityDbContext>(options =>
                {
                    options.UseSqlServer(Configuration.GetConnectionString("StudentIdentityDbContext"));
                });
            //自定义账号密码设置
            services.AddIdentity<Student,IdentityRole>(
                opts =>
                    {
                        opts.User.RequireUniqueEmail = true;
                        opts.User.AllowedUserNameCharacters =
                            "qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM0123456789";
                        opts.Password.RequiredLength = 6;
                        opts.Password.RequireNonAlphanumeric = false;
                        opts.Password.RequireLowercase = false;
                        opts.Password.RequireUppercase = false;
                        opts.Password.RequireDigit = false;
                    }).AddEntityFrameworkStores<StudentIdentityDbContext>().AddDefaultTokenProviders();


            services.ConfigureApplicationCookie(opts =>
                {
                    opts.Cookie.HttpOnly = true;
                    opts.LoginPath = "/StudentAccount/Login";
                    opts.AccessDeniedPath = "/StudentAccount/Login";
                    opts.ExpireTimeSpan = TimeSpan.FromMinutes(5);
                });

            services.AddSingleton<EmailSender>();
            //----------------------

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddDistributedMemoryCache();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
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
            //中间件顺序很重要
            app.UseAuthentication(); //若没有这个，则访问[Authorize] 的方法会再度要求进行验证
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            //app.UseSession();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
            using (var serviceScope=app.ApplicationServices.CreateScope())
            {
                var services = serviceScope.ServiceProvider;
                StudentInitiator.Initial(services).Wait();
                BookInitiator.BookInitial(services).Wait();
            }

        }
    }
}
