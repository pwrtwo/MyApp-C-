using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyApp.Data;
using MyApp.Data.Repositories;
using MyApp.Models;

namespace MyApp
{
    public class Startup
    {
        private readonly IConfiguration _config;

        public Startup(IConfiguration config)
        {
            _config = config;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<MyAppContext>(options =>
            {
                options.UseSqlServer(_config.GetConnectionString("MyAppConnection"));

            });

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 6;
            })
            .AddEntityFrameworkStores<MyAppContext>(); 
            // AddTransient는 새로운 http 요청이 있을때마다 
            // 새로운 instance를 매번 생성하는 생명주기를 가지고 있다. 
            // 보존되지 않는 형태.
            services.AddTransient<DbSeeder>();

            // AddScoped는 닷넷 서비스 생명주기 중 하나 
            // ITeacherRepository를 필요로 하는 곳에서 http 요청이 있을때마다 
            // instance를 생성하고 그 instance를 http request안에서 
            // 계속 재사용하는 생명주기 http 요청이 끝나면 다른 instance 생성하게 된다.
            services.AddScoped<ITeacherRepository, TeacherRepository>();
            services.AddScoped<IStudentRepository, StudentRepository>();

            // 어플리케이션 주기동안 단 한번만 생성한다.
            // http 요청이 있을때마다 똑같은 instance를 계속 쓴다.
            // 주로 속성이 변하지 않는 정적인 데이터를 메모리 내에 저장해서
            // 의존성 주입이 필요할때마다 쓰이게 된다.
            // services.AddSingleton
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, DbSeeder seeder)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Account}/{action=Login}/{id?}");
            });

            seeder.SeedDatabase().Wait();
        }
    }
}
