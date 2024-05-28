
using AspNetCore.Identity.MongoDbCore.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TransportTicketApp.API.Services;
using TransportTicketApp.Data.Mongo.Configurations;
using TransportTicketApp.Data.Mongo.Extensions;
using TransportTicketApp.Entities;

namespace TransportTicketApp.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);




            builder.Services.AddMongoDbContext<MongoEntityTypeProvider>(builder.Configuration, typeof(ApplicationUser));

            //builder.Services.Configure<MongoDbOptions>(o => builder.Configuration.GetSection(MongoDbOptions.SectionName).Bind(o));

            string? conStr = builder.Configuration.GetSection("MongoDb:ConnectionString").Value;
            string? dbName = builder.Configuration.GetSection("MongoDb:DatabaseName").Value;

            builder.Services.Configure<MongoDbSettings>(options =>
            {
                options.ConnectionString = conStr;
                options.DatabaseName = dbName;
            });
            builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
       .AddMongoDbStores<ApplicationUser, ApplicationRole, Guid>(
           conStr,
           dbName)
       .AddDefaultTokenProviders();

            builder.Services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 6;
            });


            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
            });

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });

            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
                options.IdleTimeout = TimeSpan.FromMinutes(30);
            });


            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                };
            });

            builder.Services.AddScoped<ITokenService, TokenService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IJourneyService, JourneyService>();
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseCors("AllowAll");


            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSession();

            CreateAdminUser(app.Services).Wait();




            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");


            app.Run();
        }

        private static async Task CreateAdminUser(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

                var adminRoleExists = await roleManager.RoleExistsAsync(UserRoles.Admin);
                if (!adminRoleExists)
                {
                    await roleManager.CreateAsync(new ApplicationRole(UserRoles.Admin));
                }

                var customerRoleExists = await roleManager.RoleExistsAsync(UserRoles.Customer);
                if (!customerRoleExists)
                {
                    await roleManager.CreateAsync(new ApplicationRole(UserRoles.Customer));
                }

                var adminUser = new ApplicationUser
                {
                    UserName = "admin",
                    Email = "admin@example.com",
                    FirstName = "Admin",
                    LastName = "User",
                    EmailConfirmed = true
                };

                var user = await userManager.FindByEmailAsync(adminUser.Email);
                if (user == null)
                {
                    var result = await userManager.CreateAsync(adminUser, "Admin@123");
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(adminUser, UserRoles.Admin);
                    }
                }
            }
        }
    }
}
