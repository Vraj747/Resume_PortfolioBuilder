using Microsoft.EntityFrameworkCore;
using ResumePortfolioBuilder.Core.Models;

namespace ResumePortfolioBuilder.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Resume> Resumes { get; set; }
        public DbSet<Portfolio> Portfolios { get; set; }
        public DbSet<JobApplication> JobApplications { get; set; }
        public DbSet<Interview> Interviews { get; set; }
        public DbSet<InterviewQuestion> InterviewQuestions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Resume configuration
            modelBuilder.Entity<Resume>()
                .HasKey(r => r.Id);

            modelBuilder.Entity<Resume>()
                .Property(r => r.Title)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Resume>()
                .Property(r => r.UserId)
                .IsRequired();

            modelBuilder.Entity<Resume>()
                .OwnsOne(r => r.PersonalInfo);

            modelBuilder.Entity<Resume>()
                .HasMany(r => r.WorkExperiences)
                .WithOne()
                .HasForeignKey("ResumeId")
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Resume>()
                .HasMany(r => r.Education)
                .WithOne()
                .HasForeignKey("ResumeId")
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Resume>()
                .HasMany(r => r.Skills)
                .WithOne()
                .HasForeignKey("ResumeId")
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Resume>()
                .HasMany(r => r.Projects)
                .WithOne()
                .HasForeignKey("ResumeId")
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Resume>()
                .HasMany(r => r.Certifications)
                .WithOne()
                .HasForeignKey("ResumeId")
                .OnDelete(DeleteBehavior.Cascade);

            // Portfolio configuration
            modelBuilder.Entity<Portfolio>()
                .HasKey(p => p.Id);

            modelBuilder.Entity<Portfolio>()
                .Property(p => p.Title)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Portfolio>()
                .Property(p => p.UserId)
                .IsRequired();

            modelBuilder.Entity<Portfolio>()
                .Property(p => p.UniqueUrl)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Portfolio>()
                .HasIndex(p => p.UniqueUrl)
                .IsUnique();

            modelBuilder.Entity<Portfolio>()
                .OwnsOne(p => p.PersonalInfo);

            modelBuilder.Entity<Portfolio>()
                .HasMany(p => p.Sections)
                .WithOne()
                .HasForeignKey("PortfolioId")
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Portfolio>()
                .HasMany(p => p.Projects)
                .WithOne()
                .HasForeignKey("PortfolioId")
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Portfolio>()
                .HasMany(p => p.SocialMediaLinks)
                .WithOne()
                .HasForeignKey("PortfolioId")
                .OnDelete(DeleteBehavior.Cascade);

            // JobApplication configuration
            modelBuilder.Entity<JobApplication>()
                .HasKey(j => j.Id);

            modelBuilder.Entity<JobApplication>()
                .Property(j => j.JobTitle)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<JobApplication>()
                .Property(j => j.Company)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<JobApplication>()
                .Property(j => j.UserId)
                .IsRequired();

            modelBuilder.Entity<JobApplication>()
                .HasMany(j => j.Interviews)
                .WithOne()
                .HasForeignKey("JobApplicationId")
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<JobApplication>()
                .HasMany(j => j.ActivityLog)
                .WithOne()
                .HasForeignKey("JobApplicationId")
                .OnDelete(DeleteBehavior.Cascade);

            // Interview configuration
            modelBuilder.Entity<Interview>()
                .HasKey(i => i.Id);

            modelBuilder.Entity<Interview>()
                .HasMany(i => i.Questions)
                .WithOne()
                .HasForeignKey("InterviewId")
                .OnDelete(DeleteBehavior.Cascade);

            // InterviewQuestion configuration
            modelBuilder.Entity<InterviewQuestion>()
                .HasKey(q => q.Id);

            modelBuilder.Entity<InterviewQuestion>()
                .Property(q => q.Question)
                .IsRequired();
        }
    }
} 