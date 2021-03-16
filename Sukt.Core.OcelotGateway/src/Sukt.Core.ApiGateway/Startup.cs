using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.OpenApi.Models;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using System;
using System.IO;
using System.Linq;

namespace Sukt.Core.ApiGateway
{
    /// <summary>
    /// 
    /// </summary>
    public class Startup
    {
        private string _corePolicyName = string.Empty;
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddOcelot().AddKubernetesFixed();
            services.Configure<AppOptionSettings>(Configuration.GetSection("SuktCore"));
            var settings = services.GetAppSettings();
            if (!settings.Cors.PolicyName.IsNullOrEmpty() && !settings.Cors.Url.IsNullOrEmpty()) //添加跨域
            {
                _corePolicyName = settings.Cors.PolicyName;
                services.AddCors(c =>
                {
                    c.AddPolicy(settings.Cors.PolicyName, policy =>
                    {
                        policy.WithOrigins(settings.Cors.Url
                          .Split(",", StringSplitOptions.RemoveEmptyEntries).ToArray())
                        //policy.WithOrigins("http://localhost:5001")//支持多个域名端口，注意端口号后不要带/斜杆：比如localhost:8000/，是错的
                        .AllowAnyHeader().AllowAnyMethod().AllowCredentials();//允许cookie;
                    });
                });
            }
            services.AddSwaggerGen(x =>
            {
                x.SwaggerDoc(Configuration["Swagger:Version"], new OpenApiInfo { Title = Configuration["Swagger:DocName"], Version = Configuration["Swagger:Version"] });
                var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                var files = Directory.GetFiles(basePath, "*.xml");
                foreach (var fiel in files)
                {
                    x.IncludeXmlComments(fiel, true);
                }
            });

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            if (!string.IsNullOrEmpty(_corePolicyName))
            {
                app.UseCors(_corePolicyName); //添加跨域中间件
            }
            var apiList = Configuration["Swagger:ServiceDocNames"].Split(',').ToList();
            if (!string.IsNullOrEmpty(_corePolicyName))
            {
                app.UseCors(_corePolicyName); //添加跨域中间件
            }
            //配置Swagger中间件
            app.UseSwagger();
            app.UseSwaggerUI(x =>
            {
                apiList.ForEach(a =>
                {
                    //Api-版本
                    var api = a.Split('-');
                    var t = $"/doc/{api[0]}/{api[1]}/swagger.json";
                    x.SwaggerEndpoint(t, a);
                });
                x.RoutePrefix = string.Empty;
            });
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseOcelot().Wait();//引入Ocelot中间件
        }
    }
}
