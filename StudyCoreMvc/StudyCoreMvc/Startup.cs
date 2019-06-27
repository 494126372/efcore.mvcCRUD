using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using StudyCoreMvc.Models;
using Microsoft.Extensions.Logging;

namespace StudyCoreMvc
{

    public class Startup
    {
        /// <summary>
        /// Startup�ļ���ִ�з����Ĳ���ʹ��A��B��C��D��ʾ
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        /// <summary>
        /// ����B  ���ַ�������ע��ĵط���netcore������������ע�뷽ʽΪ��
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            ///  dotnet ef migrations add Initial    dotnet ef database update 

            //���mvc����û�����mvc���в���
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddDbContext<StudyCoreMvcContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("StudyCoreMvcContext")));

                //���ﻹ������ע�뷽ʽ
               //services.AddSingleton<>();//����ע��
               //services.AddScoped<>();//������ע��
               //services.AddMemoryCache(); //MemoryCache����ע��
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            //���Console���
      
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            //app.UseMvc(); // ����Ĭ��Vs����MVC ������VSCode�������Դ�·�����ü��ļ���Controller
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}"); //�����{id?} ���ű�ʾ�����п���
            });
        }
    }
}
