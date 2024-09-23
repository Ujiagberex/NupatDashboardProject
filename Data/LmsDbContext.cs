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

		public DbSet<FacilitatorProfile> facilitatorProfiles { get; set; }
		public DbSet<RefreshToken> RefreshTokens { get; set; }
		public DbSet<Student> Students { get; set; }
		public DbSet<Facilitator> Facilitators { get; set; }
        public DbSet<Course> Courses { get; set; }
		public DbSet<Profile> Profiles { get; set; }
		public DbSet<Content> Contents { get; set; }
		public DbSet<ClassSchedule> ClassSchedules { get; set; }
		public DbSet<Event> Events { get; set; } 
		public DbSet<Assignment> Assignments { get; set; }
		public DbSet<Attendance> Attendances { get; set; }
		public DbSet<SubmitAssignment> SubmittedAssignments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
            base.OnModelCreating(modelBuilder);
            // Configure many-to-many relationship between Course and Student
            modelBuilder.Entity<Course>()
                .HasMany(c => c.Students)
                .WithMany(s => s.Courses)
                .UsingEntity(j => j.ToTable("CourseStudents"));

            // Configure one-to-many relationship between Cohort and Student
            modelBuilder.Entity<Student>()
                .HasOne(s => s.Cohort)
                .WithMany(c => c.Students)
                .HasForeignKey(s => s.CohortId);

            // Configure one-to-many relationship between Course and Facilitator
            modelBuilder.Entity<Course>()
                .HasOne(c => c.Facilitator)
                .WithMany(f => f.Courses)
                .HasForeignKey(c => c.FacilitatorId);

            // Configure one-to-many relationship between Course and Assignment
            modelBuilder.Entity<Assignment>()
                .HasOne(a => a.Course)
                .WithMany(c => c.Assignments)
                .HasForeignKey(a => a.CourseId);

            // Configure one-to-many relationship between Course and Content
            modelBuilder.Entity<Content>()
                .HasOne(co => co.Course)
                .WithMany(c => c.Contents)
                .HasForeignKey(co => co.CourseId);

            // Configure one-to-many relationship between Assignment and SubmitAssignment
            modelBuilder.Entity<SubmitAssignment>()
                .HasOne(sa => sa.Assignment)
                .WithMany(a => a.SubmittedAssignments)
                .HasForeignKey(sa => sa.AssignmentId);

            // Configure one-to-many relationship between Student and SubmitAssignment
            modelBuilder.Entity<SubmitAssignment>()
            .HasOne(sa => sa.Student) // Assuming 'Student' is the navigation property in SubmitAssignment
            .WithMany(s => s.SubmittedAssignments) // Navigation property for the collection in Student
            .HasForeignKey(sa => sa.Id) // Foreign key property referencing Student
                .OnDelete(DeleteBehavior.NoAction);

        }



	}
}
