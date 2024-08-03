using CloudinaryDotNet;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Nest;
using NupatDashboardProject.Configurations;
using NupatDashboardProject.Data;
using NupatDashboardProject.IServices;
using NupatDashboardProject.Models;
using NupatDashboardProject.Services;
using Swashbuckle.AspNetCore.Filters;


namespace NupatDashboardProject
{
	public class Program
	{
		private readonly static string _env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			builder.Services.AddControllers();
			builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings"));
			builder.Services.AddSingleton(sp =>
			{
				var config = sp.GetRequiredService<IOptions<CloudinarySettings>>().Value;
				return new Cloudinary(new Account(config.CloudName, config.ApiKey, config.ApiSecret));
			});
			builder.Services.AddScoped<IProfile, ProfileServices>();
			builder.Services.AddScoped<IStudent, StudentService>();
			builder.Services.AddScoped<ISearchService, SearchService>();
			builder.Services.AddScoped<IPhotoService, PhotoService>();
			builder.Services.AddScoped<ICourseRepository, PopularCourseRepo>();
			builder.Services.AddScoped<IFacilitator, FacilitatorService>();
			builder.Services.AddScoped<ICourse, CourseServices>();
			builder.Services.AddScoped<IAuth, AuthService>();
			builder.Services.AddScoped<ITokenGenerator, TokenGeneratorService>();
			builder.Services.Configure<ElasticSearchConfig>(builder.Configuration.GetSection("Elasticsearch"));
			builder.Services.AddSingleton<IElasticClient>(sp =>
			{
				var settings = new ConnectionSettings(new Uri(builder.Configuration["Elasticsearch:Url"]))
					.DefaultIndex(builder.Configuration["Elasticsearch:Index"]);
				return new ElasticClient(settings);
			});
			builder.Services.AddScoped<ISearchService, SearchService>();
			builder.Services.AddControllers();

			//Add Identity
			builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
			{
				options.User.RequireUniqueEmail = true;
				options.User.AllowedUserNameCharacters =
		   "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
				// Password settings.
				options.Password.RequireDigit = true;
				options.Password.RequireLowercase = true;
				options.Password.RequireNonAlphanumeric = true;
				options.Password.RequireUppercase = true;
				options.Password.RequiredLength = 7;
				options.Password.RequiredUniqueChars = 1;
			}).AddEntityFrameworkStores<LmsDbContext>().AddDefaultTokenProviders().AddSignInManager();

			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen(option =>
			{
				option.AddSecurityDefinition("Oauth2", new OpenApiSecurityScheme
				{
					In = ParameterLocation.Header,
					Name = "Authorization",
					Type = SecuritySchemeType.ApiKey
				});
				option.OperationFilter<SecurityRequirementsOperationFilter>();
			});

			//Add dbcontext to project
			string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
					
			string environConnectivity = Environment.GetEnvironmentVariable("DefaultConnection");
			if (_env != "Development")
			{
				builder.Services.AddDbContext<LmsDbContext>(options =>
				options.UseSqlServer(environConnectivity));
			}
			else
			{
				builder.Services.AddDbContext<LmsDbContext>(options =>
				{
					options.UseSqlServer(connectionString);
				});
			}


			var app = builder.Build();

			//Configure the HTTP request pipeline.
			//if (app.Environment.IsDevelopment())
			//{
			//	app.UseSwagger();
			//	app.UseSwaggerUI();

			//}

			app.UseSwagger();
			app.UseSwaggerUI();

			app.UseHttpsRedirection();
			app.UseRouting();
			app.UseCors("AllowSpecificOrigin");

			app.UseAuthentication();
			app.UseAuthorization();


			app.MapControllers();

			app.Run();
		}
	}
}
