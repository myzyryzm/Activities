using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Domain;

namespace API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            //only want to use it here this allows this code to be cleaned up after it is done
            //[using] provides the scope for IDisposable objects which this interface provides a mechanism for releasing unmanaged resources
            //using calls the Dispose method on the object in the correct way, and (as we are using it here) it causes the object to go out of scope as soon as Dispose is called
            //so basically this var scope is accessing unmanaged resources so u want 2 unload them right after using them; hence how we set up our code here
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<DataContext>();
                    var userManager = services.GetRequiredService<UserManager<AppUser>>();
                    //applies any pending migrations and will create a databse if it aint there
                    context.Database.Migrate();
                    //seeddate is async but this method isnt async so we call wait to hol up till that shit is done
                    Seed.SeedData(context, userManager).Wait();
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred during migration");
                }
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}


//CQRS
//Commands use write database
//queries use read database
//will be using read a lot more
//this allows us to separate each of the databases for an increase in performance (can be faster)
//however the databases do not update at the same time so it could lead to query database not being as up to date as the command database
//we r using event store to keep it up to date (read db)
//keep track of what events happened rather than directly writing to database () 
//pros ==> scalability flexibility event sourcing
//can allocate our reads 
//cons ==> more complex; does not modify state (so relational databases cant be used); event sourcing costs
//HOWEVER WE ARENT USING AN EVENT STORE
//mediatR takes object in handles it and then object out
//API controller uses Mediator.send and pass in the object; mediater will handle it and then ouput a new object