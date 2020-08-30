using Amazon.S3;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using p3CodingTask.Interfaces;
using p3CodingTask.Services;

namespace p3CodingTask
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //CORS enabled for my personal site where I'll be using filashare api in future
            services.AddCors(options =>
            {
                options.AddPolicy("parser-app",
                    builder =>
                    {
                        builder.WithOrigins("https://aws-hosted-parser-app.vercel.app")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                    });
            });
            services.AddMvc();

            #region Register other services

            services.AddSingleton<IS3Service, S3Service>();
            services.AddAWSService<IAmazonS3>();

            #endregion Register other services
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            //app.UseHttpsRedirection();
            //app.UseAuthorization();

            app.UseRouting();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseCors("parser-app");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}

//Help : https://stackoverflow.com/questions/57684093/using-usemvc-to-configure-mvc-is-not-supported-while-using-endpoint-routing