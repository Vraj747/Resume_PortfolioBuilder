using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ResumePortfolioBuilder.Core.Interfaces.Repositories;
using ResumePortfolioBuilder.Core.Models;

namespace ResumePortfolioBuilder.Data.Repositories
{
    public class ResumeRepository : IResumeRepository
    {
        private readonly ApplicationDbContext _context;

        public ResumeRepository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Resume> GetByIdAsync(Guid id)
        {
            return await _context.Resumes
                .Include(r => r.WorkExperiences)
                .Include(r => r.Education)
                .Include(r => r.Skills)
                .Include(r => r.Projects)
                .Include(r => r.Certifications)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<IEnumerable<Resume>> GetByUserIdAsync(string userId)
        {
            return await _context.Resumes
                .Where(r => r.UserId == userId)
                .Include(r => r.WorkExperiences)
                .Include(r => r.Education)
                .Include(r => r.Skills)
                .Include(r => r.Projects)
                .Include(r => r.Certifications)
                .ToListAsync();
        }

        public async Task<Resume> CreateAsync(Resume resume)
        {
            await _context.Resumes.AddAsync(resume);
            await _context.SaveChangesAsync();
            return resume;
        }

        public async Task<Resume> UpdateAsync(Resume resume)
        {
            _context.Entry(resume).State = EntityState.Modified;
            
            // Handle collections
            UpdateCollection(resume.WorkExperiences, "ResumeId", resume.Id);
            UpdateCollection(resume.Education, "ResumeId", resume.Id);
            UpdateCollection(resume.Skills, "ResumeId", resume.Id);
            UpdateCollection(resume.Projects, "ResumeId", resume.Id);
            UpdateCollection(resume.Certifications, "ResumeId", resume.Id);
            
            await _context.SaveChangesAsync();
            return resume;
        }

        public async Task DeleteAsync(Guid id)
        {
            var resume = await _context.Resumes.FindAsync(id);
            if (resume == null)
                return;

            _context.Resumes.Remove(resume);
            await _context.SaveChangesAsync();
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