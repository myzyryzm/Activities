using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Persistence;
using Microsoft.EntityFrameworkCore;
using MediatR;
using Application.Activities;

namespace API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        //dependency injection controller
        public void ConfigureServices(IServiceCollection services)
        {
            //so <T> means that AddDbContext is a generic method
            //in the definition it says that DataContext has to inherit from DbContext
            //it takes in this <DataContext> and uses its type to add to the IServiceCollection and it returns that service collection
            //it takes in the DataContext u send in (and forces it to have) 
            services.AddDbContext<DataContext>(opt => 
            {
                opt.UseSqlite(Configuration.GetConnectionString("DefaultConnection"));
            });
            //need to add CORS
            services.AddCors(opt => 
            {
                //what this does is says that any request coming from our client application (i.e. localhost 3000 can use anyheader and any method)
                opt.AddPolicy("CorsPolicy", policy => 
                {
                    policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:3000");
                });
            });
            //need to add assembly of handlers
            services.AddMediatR(typeof(List.Handler).Assembly);
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        //middleware we add to our pipeline
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //dont use https redirection for now; tells http request to be https
            // app.UseHttpsRedirection();
            
            app.UseRouting();

            app.UseAuthorization();
            app.UseCors("CorsPolicy");
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
