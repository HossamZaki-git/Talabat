using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using System.Text;
using Talabat.Core.Domain_Models.Identity;
using Talabat.Core.GenericRepository;
using Talabat.Repository.Data;
using Talabat.Repository.Identity;
using Talabat.Repository.Repositories;
using Talabat.WebAPI.Errors;
using Talabat.WebAPI.Extensions;
using Talabat.WebAPI.Middlewares;
using Talabat.WebAPI.Utilities;

namespace Talabat.WebAPI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();

            // Adds swagger services to the container
            builder.Services.AddSwaggerServices();

            builder.Services.AddDbContext<StoreContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionString"));
            });

            builder.Services.AddDbContext<ApplicationIdentityContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
            });

            // Adds most of the application services to the container
            builder.Services.AddApplicationServices();

            builder.Services.AddSingleton<IConnectionMultiplexer>(options =>
            {
                var connectionString = builder.Configuration.GetConnectionString("redis");
                // ConnectionMultiplexer.Connect("connectionString") => returns an instance of type ConnectionMultiplexer which represents the connection
                // with the redis server. ConnectionMultiplexer implements IConnectionMultiplexer
                return ConnectionMultiplexer.Connect(connectionString);
            });

            builder.Services.AddScoped(typeof(IBasketsRepository), typeof(BasketsRepository));

            builder.Services.AddIdentityServices();

            // now the default authentication scheme is "Bearer"
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                // Without this TokenValidationParameters instance no validation will take place
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"])),
                    ValidIssuer = builder.Configuration["JWT:Issuer"],
                    ValidAudience = builder.Configuration["JWT:Audience"],

                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    // The expiration date is already defined in the JwtSecurityToken constructor
                    ValidateLifetime = true
                };
            });

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("My_CORS_Policy", policyBuilder =>
                {
                    policyBuilder.WithOrigins(builder.Configuration["AllowedOrigins:TalabatOrigin"])
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
            });

            var app = builder.Build();

            app.UseMiddleware<ExceptionHandlerMiddleware>();

            //app.UseStatusCodePagesWithRedirects("/NonExisting/{0}");
            app.UseStatusCodePagesWithReExecute("/NonExisting/{0}");

            // This IServiceScope instance can be used to resolve any scoped service
            /* But those services must be registered in the container first with a scoped lifetime */
            using (var scope = app.Services.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;

                StoreContext context = serviceProvider.GetRequiredService<StoreContext>();
                ApplicationIdentityContext IdentityContext = serviceProvider.GetRequiredService<ApplicationIdentityContext>();
                UserManager<ApplicationUser> userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                RoleManager<IdentityRole> roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                ILoggerFactory loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();

                try
                {
                    context.Database.Migrate();
                    IdentityContext.Database.Migrate();
                    DataSeeder.Seed(context);
                    await IdentitySeeder.Seed(userManager, roleManager, IdentityContext);
                }
                catch (Exception ex)
                {
                    var logger = loggerFactory.CreateLogger<Program>();
                    logger.LogError(ex, $"While applying a migration OR seeding data the following exception has occurred:\n{ex.Message}");
                }
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwaggerMiddlewares();
            }

            app.UseStaticFiles();

            app.UseCors("My_CORS_Policy");

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
