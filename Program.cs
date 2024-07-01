
using CloudinaryDotNet;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Nest;
using NupatDashboardProject.Configurations;
using NupatDashboardProject.Data;
using NupatDashboardProject.IServices;
using NupatDashboardProject.Services;


namespace NupatDashboardProject
{
	public class Program
	{
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
			builder.Services.Configure<ElasticSearchConfig>(builder.Configuration.GetSection("Elasticsearch"));
			builder.Services.AddSingleton<IElasticClient>(sp =>
			{
				var settings = new ConnectionSettings(new Uri(builder.Configuration["Elasticsearch:Url"]))
					.DefaultIndex(builder.Configuration["Elasticsearch:Index"]);
				return new ElasticClient(settings);
			});
			builder.Services.AddScoped<ISearchService, SearchService>();
			builder.Services.AddControllers();



			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();
			builder.Services.AddDbContext<LmsDbContext>(options =>
			{
				options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
			});


			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();

			app.UseAuthorization();


			app.MapControllers();

			app.Run();
		}
	}
}
