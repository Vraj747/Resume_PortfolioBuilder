using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ResumePortfolioBuilder.Core.Models;

namespace ResumePortfolioBuilder.Core.Interfaces.Repositories
{
    public interface IResumeRepository
    {
        Task<Resume> GetByIdAsync(Guid id);
        Task<IEnumerable<Resume>> GetByUserIdAsync(string userId);
        Task<Resume> CreateAsync(Resume resume);
        Task<Resume> UpdateAsync(Resume resume);
        Task DeleteAsync(Guid id);
    }

    public interface IPortfolioRepository
    {
        Task<Portfolio> GetByIdAsync(Guid id);
        Task<Portfolio> GetByUniqueUrlAsync(string uniqueUrl);
        Task<IEnumerable<Portfolio>> GetByUserIdAsync(string userId);
        Task<Portfolio> CreateAsync(Portfolio portfolio);
        Task<Portfolio> UpdateAsync(Portfolio portfolio);
        Task DeleteAsync(Guid id);
    }

    public interface IJobApplicationRepository
    {
        Task<JobApplication> GetByIdAsync(Guid id);
        Task<IEnumerable<JobApplication>> GetByUserIdAsync(string userId);
        Task<IEnumerable<JobApplication>> GetByStatusAsync(string userId, ApplicationStatus status);
        Task<JobApplication> CreateAsync(JobApplication jobApplication);
        Task<JobApplication> UpdateAsync(JobApplication jobApplication);
        Task DeleteAsync(Guid id);
        Task<IEnumerable<JobApplication>> GetUpcomingFollowUpsAsync(string userId, DateTime startDate, DateTime endDate);
    }

    public interface IInterviewRepository
    {
        Task<Interview> GetByIdAsync(Guid id);
        Task<IEnumerable<Interview>> GetByJobApplicationIdAsync(Guid jobApplicationId);
        Task<Interview> CreateAsync(Interview interview);
        Task<Interview> UpdateAsync(Interview interview);
        Task DeleteAsync(Guid id);
    }
} 