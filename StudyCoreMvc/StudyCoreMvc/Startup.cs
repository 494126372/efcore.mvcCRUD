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
        /// 
        /*
         ������ע��ͨ�����°취�����Щ����:
        ʹ�ýӿ�������������ʵ�֡�
        �ڷ���������ע�������ASP.NET�����ṩ���÷�������IServiceProvider��������Ӧ�õķ�����ע�ᡣStartup.ConfigureServices
        ������ע��ʹ��������Ĺ��캯������ܸ��𴴽��������ʵ��,���ڲ�����Ҫʱ���䴦��
             */
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
            // ע��ʵ���������ĵط� 
            services.AddDbContext<StudyCoreMvcContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("StudyCoreMvcContext")));
            //services.AddSingleton(typeof(ILogger<T>), typeof(Logger<T>));
            //Startup ��� ConfigureServices ���������÷���Ҫָ������ IDateTime ʱʹ�� SystemDateTime ��ʵ����������
            //���������嵥�и������е���� ConfigureServices �����У�
            //services.AddTransient<IDateTime,SystemDateTime>();
            //Ҳ������ôд
            services.AddTransient(typeof(IDateTime), typeof(SystemDateTime));

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
    //public class ILogger<T> : B<T> where T : IEnumerable {
        
        
    //   }
    public class MyDependency : IMyDependency
    {
        private readonly ILogger<MyDependency> _logger;

        public MyDependency(ILogger<MyDependency> logger)
        {
            _logger = logger;
            /*
             MyDependency���乹�캯��������һ��ILogger_lt;TcategoryName>��
             ����ʽ��ʽʹ��������ע�벢��������
             ÿ������������������������Լ��������
             ��������ͼ���е������������ȫ�����ķ��񡣱��������һ��������ͨ����Ϊ����������������ϵͼ�����ͼ��
             IMyDependency�����ڷ���������ע�ᡣ �� ��ע�ᡣ ����־��¼��������ṹע��,������ǿ��Ĭ��ע��Ŀ���ṩ�ķ���
             */
        }

        public Task WriteMessage(string message)
        {
            _logger.LogInformation(
                "MyDependency.WriteMessage called. Message: {MESSAGE}",
                message);

            return Task.FromResult(0);
        }
    }
    public interface IMyDependency
    {
        Task WriteMessage(string message);
    }
    public class Logger : ILogger
    {
        public IDisposable BeginScope<TState>(TState state)
        {
            throw new NotImplementedException();
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            throw new NotImplementedException();
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            throw new NotImplementedException();
        }
    }

    // ���Ӷ�
    public interface IDateTime
    {
        DateTime Now { get; }
    }
    public class SystemDateTime : IDateTime
    {
        public DateTime Now { get { return DateTime.Now; }  }
    }
}
