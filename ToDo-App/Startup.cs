using Infrastructure.Interfaces;
using Infrastructure.Data;
using Infrastructure.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Core.Settings;
using ToDo_App.Filters;
using System;

namespace ToDo_App
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
            services.AddDbContext<TodoContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))
            );

            services.AddOptions();
            services.Configure<TodoSettings>(Configuration.GetSection("TodoSettings"));
            services.Configure<AuditSettings>(Configuration.GetSection("AuditSettings"));

            services.AddHealthChecks();
            services.AddCors();

            services.AddHttpClient("github", c =>
            {
                c.BaseAddress = new Uri("https://api.github.com/");
                c.DefaultRequestHeaders.Add("Accept", "application/vnd.github.v3+json");
                c.DefaultRequestHeaders.Add("User-Agent", "ToDo-App");
            });

            services.AddScoped(typeof(ITodoItemService), typeof(TodoItemService));
            services.AddScoped(typeof(ICategoryService), typeof(CategoryService));

            services.AddMvc(options => 
                    options.Filters.Add(typeof(AuditFilter)))
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseHealthChecks("/health");
            app.UseCors(options => options
                .AllowAnyOrigin()
            );

            app.UseMvc();
        }
    }
}
