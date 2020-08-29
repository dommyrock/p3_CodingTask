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
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //CORS enabled for my personal site wher I'll use filashare api for Drag n drop file uploads
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

            services.AddSingleton<IS3Service, S3Service>();
            services.AddAWSService<IAmazonS3>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseHttpsRedirection();
            //app.UseAuthorization();

            app.UseRouting();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseCors("parser-app");

            //Help : https://stackoverflow.com/questions/57684093/using-usemvc-to-configure-mvc-is-not-supported-while-using-endpoint-routing

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}