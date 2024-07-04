using CsApiBaseHub;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OnlineManager.Models.Customized;
using Relation.Middlewares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Threading.Tasks;

namespace Relation
{
    public class Startup
    {
        public IConfiguration Configuration { get; set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            var emailsetting = Configuration.GetSection("MailSettings").Get<MailSettings>();

            services.AddSingleton(emailsetting);
            services.AddHttpClient();

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(120);
            });
            services.AddMvc().AddRazorPagesOptions(x => { x.RootDirectory = "/"; });


            //Exception  Mail�H�X
            CsExceptionEmail cee = new(Configuration.GetValue<string>("Mail:ExceptionEmailTarget"), Configuration.GetValue<string>("Mail:subject"));
            services.AddSingleton<ICsExceptionEmail>(cee);
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                //app.UseDeveloperExceptionPage();
            }
            else
            {
                // �j��ϥ� Https
                //app.UseHsts();
            }

            // �j��Https��V
            //app.UseHttpsRedirection();

            app.UseSession();

            app.UseStaticFiles();

            app.UseRouting();

            // �ϥιw�] [Authorize] �ݩ�
            //app.UseAuthorization();

            // ���UMiddleware �`�N�������Y
            app.UseMiddleware<LogMiddleware>();
            app.UseMiddleware<ExceptionMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                // MVC Route �]�w��n�J����
                endpoints.MapControllerRoute(
                    name: "default",

                    pattern: "{controller=Login}/{action=Index}/{id?}");
            });
        }
    }
}
