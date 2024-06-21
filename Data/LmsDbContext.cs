using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NupatDashboardProject.Models;

namespace NupatDashboardProject.Data
{
    public class LmsDbContext : IdentityDbContext<ApplicationUser>
	{
		public LmsDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
		{

		}


		public DbSet<Student> Students { get; set; }
		public DbSet<Facilitator> Facilitators { get; set; }
        public DbSet<Course> Courses { get; set; }
		public DbSet<Schedule> Schedules { get; set; }
		public DbSet<Profile> Profiles { get; set; }
		public DbSet<Test> Tests { get; set; }
		public DbSet<Content> Contents { get; set; }
		public DbSet<Assignment> Assignments { get; set; }
		public DbSet<Attendance> Attendances { get; set; }
		public DbSet<Cohort> Cohorts { get; set; }
		public DbSet<IndustryInterest> IndustryInterests { get; set; }
		public DbSet<SocialMediaAccount> SocialMediaAccounts { get; set; }

		//protected override void OnModelCreating(ModelBuilder modelBuilder)
		//{
		//	//// Configure the Cohort entity
		//	modelBuilder.Entity<Cohort>(entity =>
		//	{
		//		entity.HasKey(e => e.CohortId); // Specify primary key

		//		entity.Property(e => e.Name)
		//			.IsRequired() // Make Name property required
		//			.HasMaxLength(100); // Set a maximum length for Name
		//			.OnDelete(DeleteBehavior.Restrict); // Prevent cascading deletes
		//	});

		//	// Configure the Student entity
		//	modelBuilder.Entity<Student>(entity =>
		//	{
		//		entity.HasKey(e => e.StudentId); // Specify primary key

		//		entity.HasOne(s => s.Cohort) // Configure many-to-one relationship
		//			.WithMany(c => c.Students)
		//			.HasForeignKey(s => s.CohortId)
		//			.OnDelete(DeleteBehavior.Restrict);

		//		entity.HasOne(s => s.CourseOfInterest) // Configure many-to-one relationship
		//			.WithMany(c => c.Students)
		//			.HasForeignKey(s => s.CourseId)
		//			.OnDelete(DeleteBehavior.Restrict);
		//	});

		//	// Configure the Course entity
		//	modelBuilder.Entity<Course>(entity =>
		//	{
		//		entity.HasKey(e => e.CourseId); // Specify primary key

		//		entity.Property(e => e.Title)
		//			.IsRequired() // Make Title property required
		//			.HasMaxLength(200); // Set a maximum length for Title

		//		entity.HasMany(c => c.Students) // Configure one-to-many relationship
		//			.WithOne(s => s.CourseOfInterest)
		//			.HasForeignKey(s => s.CourseId)
		//			.OnDelete(DeleteBehavior.Restrict); // Prevent cascading deletes

		//		entity.HasOne(c => c.Facilitator) // Configure many-to-one relationship
		//			.WithMany(f => f.Courses)
		//			.HasForeignKey(c => c.FacilitatorId)
		//			.OnDelete(DeleteBehavior.Restrict);
		//	});

		//	// Configure the Facilitator entity
		//	modelBuilder.Entity<Facilitator>(entity =>
		//	{
		//		entity.HasKey(e => e.FacilitatorId); // Specify primary key

		//		entity.HasMany(f => f.Courses) // Configure one-to-many relationship
		//			.WithOne(c => c.Facilitator)
		//			.HasForeignKey(c => c.FacilitatorId)
		//			.OnDelete(DeleteBehavior.Restrict);
		//	});

		//	base.OnModelCreating(modelBuilder);
		//}



	}
}
