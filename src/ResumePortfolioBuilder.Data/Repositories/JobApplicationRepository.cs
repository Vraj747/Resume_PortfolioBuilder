using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ResumePortfolioBuilder.Core.Interfaces.Repositories;
using ResumePortfolioBuilder.Core.Models;

namespace ResumePortfolioBuilder.Data.Repositories
{
    public class JobApplicationRepository : IJobApplicationRepository
    {
        private readonly ApplicationDbContext _context;

        public JobApplicationRepository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<JobApplication> GetByIdAsync(Guid id)
        {
            return await _context.JobApplications
                .Include(j => j.Interviews)
                    .ThenInclude(i => i.Questions)
                .Include(j => j.ActivityLog)
                .FirstOrDefaultAsync(j => j.Id == id);
        }

        public async Task<IEnumerable<JobApplication>> GetByUserIdAsync(string userId)
        {
            return await _context.JobApplications
                .Where(j => j.UserId == userId)
                .Include(j => j.Interviews)
                .ToListAsync();
        }

        public async Task<IEnumerable<JobApplication>> GetByStatusAsync(string userId, ApplicationStatus status)
        {
            return await _context.JobApplications
                .Where(j => j.UserId == userId && j.Status == status)
                .Include(j => j.Interviews)
                .ToListAsync();
        }

        public async Task<JobApplication> CreateAsync(JobApplication jobApplication)
        {
            await _context.JobApplications.AddAsync(jobApplication);
            await _context.SaveChangesAsync();
            return jobApplication;
        }

        public async Task DeleteAsync(Guid id)
        {
            var jobApplication = await _context.JobApplications.FindAsync(id);
            if (jobApplication == null)
                return;

            _context.JobApplications.Remove(jobApplication);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<JobApplication>> GetUpcomingFollowUpsAsync(string userId, DateTime startDate, DateTime endDate)
        {
            return await _context.JobApplications
                .Where(j => j.UserId == userId && 
                           j.NextFollowUpDate >= startDate && 
                           j.NextFollowUpDate <= endDate)
                .Include(j => j.Interviews)
                .ToListAsync();
        }

        public async Task<JobApplication> UpdateAsync(JobApplication jobApplication)
        {
            _context.Entry(jobApplication).State = EntityState.Modified;
            
            // Handle collections
            UpdateCollection(jobApplication.Interviews, "JobApplicationId", jobApplication.Id);
            UpdateCollection(jobApplication.ActivityLog, "JobApplicationId", jobApplication.Id);
            
            // Handle nested collections (interview questions)
            foreach (var interview in jobApplication.Interviews ?? Enumerable.Empty<Interview>())
            {
                UpdateCollection(interview.Questions, "InterviewId", interview.Id);
            }
            
            await _context.SaveChangesAsync();
            return jobApplication;
        }

        private void UpdateCollection<T>(ICollection<T> collection, string foreignKeyName, Guid foreignKeyValue) where T : class
        {
            if (collection == null) return;

            foreach (var item in collection)
            {
                var entry = _context.Entry(item);
                if (entry.State == EntityState.Detached)
                {
                    // Set foreign key for new items
                    var property = entry.Property(foreignKeyName);
                    property.CurrentValue = foreignKeyValue;
                    _context.Set<T>().Add(item);
                }
            }
        }
    }
} 