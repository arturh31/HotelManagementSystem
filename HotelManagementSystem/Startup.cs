using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using HotelManagementSystem.Data;
using HotelManagementSystem.Models;
using HotelManagementSystem.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ReflectionIT.Mvc.Paging;

namespace HotelManagementSystem
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

	
		public void ConfigureServices(IServiceCollection services)
		{
			services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));
			services.AddScoped<IRoomService, RoomService>();
			services.AddScoped<IEmployeeService, EmployeeService>();
			services.AddScoped<IReservationService, ReservationService>();
			services.AddScoped<IGuestService, GuestService>();
			services.AddScoped<ITicketService, TicketService>();
			services.AddSingleton<IEmailSender, EmailSender>();
			services.Configure<CookiePolicyOptions>(options =>
			{
				
				
				options.CheckConsentNeeded = context => true;
				options.MinimumSameSitePolicy = SameSiteMode.None;
			});			

			services.AddDbContext<ApplicationDbContext>(options =>
				options.UseSqlServer(Configuration.GetConnectionString("IdentityConnection")));
			services.AddDbContext<HMSdbContext>(options =>
				options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

			services.AddIdentity<ApplicationUser, ApplicationRole>()
				.AddEntityFrameworkStores<ApplicationDbContext>()
				.AddDefaultTokenProviders();

			services.AddRazorPages();
			services.AddPaging();



			services.Configure<IdentityOptions>(options =>
			{
				// Password settings.
				options.Password.RequireDigit = true;
				options.Password.RequireLowercase = true;
				options.Password.RequireNonAlphanumeric = true;
				options.Password.RequireUppercase = true;
				options.Password.RequiredLength = 6;
				options.Password.RequiredUniqueChars = 1;

				// Lockout settings.
				options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
				options.Lockout.MaxFailedAccessAttempts = 5;
				options.Lockout.AllowedForNewUsers = true;

				// User settings.
				options.User.AllowedUserNameCharacters =
					"abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
				options.User.RequireUniqueEmail = false;
			});

			services.ConfigureApplicationCookie(options =>
			{
				// Cookie settings
				options.Cookie.HttpOnly = true;
				options.ExpireTimeSpan = TimeSpan.FromMinutes(120);
				options.LoginPath = "/Account/Login";
				options.LogoutPath = "/Account/Logout";
				options.AccessDeniedPath = "/Account/AccessDenied";
				options.SlidingExpiration = true;

			});

			// Add application services.
			services.AddTransient<IEmailSender, EmailSender>();
			services.AddControllersWithViews();
		}

		
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseDatabaseErrorPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");				
				app.UseHsts();
			}

			
			app.UseStaticFiles();
			app.UseCookiePolicy();
			app.UseRouting();
			app.UseAuthentication();
			app.UseAuthorization();
			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller=Home}/{action=Index}/{id?}");
				endpoints.MapRazorPages();
			});


			//CreateUserRoles(serviceProvider).Wait();


		}

		//USED AS A WORKAORUND TO CREATE ROLES IN INDENTITY DATRABASE


	/*	private async Task CreateUserRoles(IServiceProvider serviceProvider)
		{
			var RoleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
			var UserManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

			IdentityResult roleResult;
			//Adding Admin Role
			var roleCheck = await RoleManager.RoleExistsAsync("User");
			if (!roleCheck)
			{
				//create the roles and seed them to the database
				roleResult = await RoleManager.CreateAsync(new ApplicationRole("User"));
			}
			//Assign Admin role to the main User here we have given our newly registered 
			//login id for Admin management
			ApplicationUser user = await UserManager.FindByEmailAsync("mwasilewska@hotel.pl");
			var User = new ApplicationUser();
			await UserManager.AddToRoleAsync(user, "User");
		}*/
	}

}
	
