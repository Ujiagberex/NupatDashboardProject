using Microsoft.AspNetCore.Identity;
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

		public DbSet<StudentResource> StudentResources { get; set; }
		public DbSet<RefreshToken> RefreshTokens { get; set; }
		public DbSet<Student> Students { get; set; }
		public DbSet<Facilitator> Facilitators { get; set; }
        public DbSet<Course> Courses { get; set; }
		public DbSet<Profile> Profiles { get; set; }
		public DbSet<Test> Tests { get; set; }
		public DbSet<Content> Contents { get; set; }
		public DbSet<ClassSchedule> ClassSchedules { get; set; }
		public DbSet<Event> Events { get; set; } 
		public DbSet<Assignment> Assignments { get; set; }
		public DbSet<Attendance> Attendances { get; set; }
		public DbSet<SubmitAssignment> SubmittedAssignments { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<SubmitAssignment>()
				.HasOne(sa => sa.Assignment)
				.WithMany(a => a.SubmittedAssignments)   // One assignment can have many submitted assignments
				.HasForeignKey(sa => sa.AssignmentId);   // Foreign key on SubmittedAssignment

			modelBuilder.Entity<SubmitAssignment>()
				.HasOne(sa => sa.Student)  // Navigation property
				.WithMany()  // Assuming no navigation property in ApplicationUser
				.HasForeignKey(sa => sa.StudentId)  // Foreign key
				.OnDelete(DeleteBehavior.Cascade);  // Cascade delete when a user is deleted
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<IdentityUserLogin<string>>()
				.HasKey(login => new { login.LoginProvider, login.ProviderKey });

			modelBuilder.Entity<SubmitAssignment>()
				.HasKey(s => s.Id); // Set Id as the primary key
		}



	}
}
