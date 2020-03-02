using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Persistence;
using Microsoft.EntityFrameworkCore;
using MediatR;
using Application.Activities;
using FluentValidation.AspNetCore;
using API.Middleware;
using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Infrastructure.Security;
using Application.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using AutoMapper;
using Infrastructure.Photos;
using Application.Photos;

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
                opt.UseLazyLoadingProxies();
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
            //mediator pattern basically controls how a set of objects communicate
            //it encapsulates a collection of objects and controls how they can message each other
            //instead of our service (IServiceCollection/IService) handling how our handlers interact; our mediator now controls how messages are sent between them
            //mediator is a fucking middle (wo)man; what a fucking slimeball!!!!!!! jk lol mediator is boss
            //MediatR can either 'send and receive messages' or broadcast them
            //here we are adding an assembly of all our handlers
            //if we are using a pattern where the object that is sending a message does not know what is being done with the message then we can say we use a mediator pattern
            services.AddMediatR(typeof(List.Handler).Assembly);
            //automapper is used for mapping object to object; if we return just regular activities from the server than activitydtos we get in an loop of sort 
            services.AddAutoMapper(typeof(List.Handler));
            //registering the validators to our service via .AddFluentValidation
            //the RegisterValidators... looks at the create class and looks through the classes it contains and it finds the AbstractValidator class so it knows to scan through the whole project and find any classes that are of the AbstractValidator type
            services.AddControllers(opt => 
            {
                //any of our requests now require authorization (we override it in UserController so those requests dont require)
                var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                opt.Filters.Add(new AuthorizeFilter(policy));
            }).AddFluentValidation(cfg => 
            {
                cfg.RegisterValidatorsFromAssemblyContaining<Create>();
            });
            // services.AddHttpContextAccessor();
            // services.TryAddScoped<IUserValidator<AppUser>, UserValidator<AppUser>>();
            // services.TryAddScoped<IPasswordValidator<AppUser>, PasswordValidator<AppUser>>();
            // services.TryAddScoped<IPasswordHasher<AppUser>, PasswordHasher<AppUser>>();
            // services.TryAddScoped<ILookupNormalizer, UpperInvariantLookupNormalizer>();
            // services.TryAddScoped<IRoleValidator<IdentityRole>, RoleValidator<IdentityRole>>();
            // services.TryAddScoped<IdentityErrorDescriber>();
            // services.TryAddScoped<ISecurityStampValidator, SecurityStampValidator<AppUser>>();
            // services.TryAddScoped<ITwoFactorSecurityStampValidator, TwoFactorSecurityStampValidator<AppUser>>();
            // services.TryAddScoped<IUserClaimsPrincipalFactory<AppUser>, UserClaimsPrincipalFactory<AppUser, IdentityRole>>();
            // services.TryAddScoped<UserManager<AppUser>>();
            // services.TryAddScoped<SignInManager<AppUser>>();
            // services.TryAddScoped<RoleManager<IdentityRole>>();
            //addIdentity (not aIC) is for MVC pages
            // var builder = services.AddIdentityCore<AppUser>().AddEntityFrameworkStores<DataContext>();
            // var identityBuilder = new IdentityBuilder(builder.UserType, builder.Services);
            // //lets identity builder handle teh users; user manager service comes from AddEntityFrameworkStores
            // identityBuilder.AddEntityFrameworkStores<DataContext>();
            // //controls how users can sign in
            // identityBuilder.AddSignInManager<SignInManager<AppUser>>();

            services.AddDefaultIdentity<AppUser>()
            .AddEntityFrameworkStores<DataContext>();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["TokenKey"]));
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt => 
            {
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,
                    ValidateAudience = false,
                    ValidateIssuer = false
                };
            });
            services.AddAuthorization(opt => 
            {
                opt.AddPolicy("IsActivityHost", policy => 
                {
                    policy.Requirements.Add(new IsHostRequirement());
                });
            });
            services.AddTransient<IAuthorizationHandler, IsHostRequirementHandler>();
            //IJWTGenerator and inject it into classes in it; those classes will have access to use the methods
            services.AddScoped<IJwtGenerator, JwtGenerator>();
            services.AddScoped<IUserAccessor, UserAccessor>();
            services.AddScoped<IPhotoAccessor, PhotoAccessor>();
            //tell where to get secret keys
            services.Configure<CloudinarySettings>(Configuration.GetSection("Cloudinary"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        //middleware we add to our pipeline
        //ordering is important in this configure
        //exception handling delegates should be called early in the pipeline so they can catch exceptions that occur in later stages of the pipeline
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<ErrorHandlingMiddleware>();
            if (env.IsDevelopment())
            {
                // app.UseDeveloperExceptionPage();
            }
            //dont use https redirection for now; tells http request to be https
            // app.UseHttpsRedirection();
            
            app.UseRouting();
            app.UseCors("CorsPolicy");

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
