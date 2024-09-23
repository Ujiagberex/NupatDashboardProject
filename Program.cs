using System.Text;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NupatDashboardProject.Configurations;
using NupatDashboardProject.Data;
using NupatDashboardProject.DTO;
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

			builder.Services.AddScoped<IClassService, ClassService>();
			builder.Services.AddScoped<IEventService, EventService>();
			builder.Services.AddScoped<IProfile, ProfileServices>();
			builder.Services.AddScoped<IStudent, StudentService>();
			builder.Services.AddScoped<IPhotoService, PhotoService>();
			builder.Services.AddScoped<ICourseRepository, PopularCourseRepo>();
			builder.Services.AddScoped<IFacilitator, FacilitatorService>();
			builder.Services.AddScoped<ICourse, CourseServices>();
			builder.Services.AddScoped<IAuth, AuthService>();
			builder.Services.AddScoped<IFacilitatorProfileService, FacilitatorProfileService>();
			builder.Services.AddScoped<ITokenGenerator, TokenGeneratorService>();
			builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JWT"));
			builder.Services.Configure<ElasticSearchConfig>(builder.Configuration.GetSection("Elasticsearch"));
			

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
			}).AddEntityFrameworkStores<LmsDbContext>().AddDefaultTokenProviders();

			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen(option =>
			{
				option.AddSecurityDefinition("Oauth2", new OpenApiSecurityScheme
				{
					In = ParameterLocation.Header,
					Description = "Please enter into field the word 'Bearer' followed by a space and the JWT",
					Name = "Authorization",
					Type = SecuritySchemeType.ApiKey,
					Scheme = "Bearer"
				});
				option.OperationFilter<SecurityRequirementsOperationFilter>();
			});

			//Add Authentication and Configure JWT
			var jwtSettings = builder.Configuration.GetSection("JWT").Get<JwtSettings>();
			var key = Encoding.ASCII.GetBytes(jwtSettings.Secret);

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
				ValidateIssuerSigningKey = true,
				ValidateLifetime = true,
				ValidIssuer = jwtSettings.Site,
				ValidAudience = jwtSettings.Audience,
				IssuerSigningKey = new SymmetricSecurityKey(key),
				ClockSkew = TimeSpan.Zero
			}; 
		});
			//builder.Services.AddCors(options =>
			//{
			//	options.AddPolicy("AllowAllOrigins",
			//	builder =>
			//	{
			//		builder.WithOrigins("https://nupat-student-dashboard-chi.vercel.app", "https://student-dashboard-azure.vercel.app") // Add allowed origins here
			//			   .AllowAnyMethod()
			//			   .AllowAnyHeader()
			//			   .AllowCredentials(); // Allows cookies/authentication tokens
			//	});

			//});

			builder.Services.AddCors(options =>
			{
				options.AddPolicy("AllowAll",
					builder =>
					{
						builder.AllowAnyOrigin()
							   .AllowAnyMethod()
							   .AllowAnyHeader();
					});
			});
			builder.Services.AddControllers();

			//Add dbcontext to project
			string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
					
			string environConnectivity = Environment.GetEnvironmentVariable("DefaultConnection");

			builder.Services.AddDbContext<LmsDbContext>(options =>
			{
				options.UseSqlServer(connectionString);
			});


			//if (_env != "Development")
			//{
			//	builder.Services.AddDbContext<LmsDbContext>(options =>
			//	options.UseSqlServer(environConnectivity));
			//}
			//else
			//{
			//	builder.Services.AddDbContext<LmsDbContext>(options =>
			//	{
			//		options.UseSqlServer(connectionString);
			//	});
			//}


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
			app.UseCors("AllowAll");
			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();


			app.MapControllers();

			app.Run();
		}
	}
}
