using Board;
using Board.Controllers;
using Board.Data;
using Board.Infrastructure;
using Board.Interfaces;
using Board.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Board
{

    public class Startup
    {
        private IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        // ConfigureServices 메서드: 의존성 주입을 구성하는 메서드
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<BoardContext>(options =>
            options.UseSqlServer("Data Source=.\\SQLEXPRESS;Initial Catalog=board;Persist Security Info=True;User ID=sa;Password=123qwe!@#QWE;Trusted_Connection=True;MultipleActiveResultSets=true;Trust Server Certificate=true"));

            services.AddMvc();
            //services.AddControllersWithViews();

            services.AddScoped<CreateSessionRepository,
                EFSboardSessionRepository>();
        }


        // Configure 메서드: 애플리케이션의 요청 처리를 구성하는 메서드
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            if (!env.IsDevelopment())
            {
                app.UseExceptionHandler("/Notices/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            //else
            //{
            //    app.UseExceptionHandler("/Home/Error");
            //}
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Notices}/{action=Index}/{id?}");
            });
        }
        
    }
}