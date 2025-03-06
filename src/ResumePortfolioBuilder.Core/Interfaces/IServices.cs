using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ResumePortfolioBuilder.Core.Models;

namespace ResumePortfolioBuilder.Core.Interfaces
{
    public interface IResumeService
    {
        Task<Resume> GetResumeByIdAsync(Guid id);
        Task<IEnumerable<Resume>> GetResumesByUserIdAsync(string userId);
        Task<Resume> CreateResumeAsync(Resume resume);
        Task<Resume> UpdateResumeAsync(Resume resume);
        Task DeleteResumeAsync(Guid id);
        Task<Resume> OptimizeResumeForJobAsync(Guid resumeId, string jobDescription);
        Task<byte[]> ExportResumeAsPdfAsync(Guid resumeId);
        Task<byte[]> ExportResumeAsDocxAsync(Guid resumeId);
        Task<string> ExportResumeAsJsonAsync(Guid resumeId);
    }

    public interface IPortfolioService
    {
        Task<Portfolio> GetPortfolioByIdAsync(Guid id);
        Task<Portfolio> GetPortfolioByUniqueUrlAsync(string uniqueUrl);
        Task<IEnumerable<Portfolio>> GetPortfoliosByUserIdAsync(string userId);
        Task<Portfolio> CreatePortfolioAsync(Portfolio portfolio);
        Task<Portfolio> UpdatePortfolioAsync(Portfolio portfolio);
        Task DeletePortfolioAsync(Guid id);
        Task<string> GenerateUniqueUrlAsync(string title, string userId);
    }

    public interface IJobApplicationService
    {
        Task<JobApplication> GetJobApplicationByIdAsync(Guid id);
        Task<IEnumerable<JobApplication>> GetJobApplicationsByUserIdAsync(string userId);
        Task<IEnumerable<JobApplication>> GetJobApplicationsByStatusAsync(string userId, ApplicationStatus status);
        Task<JobApplication> CreateJobApplicationAsync(JobApplication jobApplication);
        Task<JobApplication> UpdateJobApplicationAsync(JobApplication jobApplication);
        Task DeleteJobApplicationAsync(Guid id);
        Task<IEnumerable<JobApplication>> GetUpcomingFollowUpsAsync(string userId, int daysAhead);
        Task<Dictionary<ApplicationStatus, int>> GetJobApplicationStatisticsAsync(string userId);
    }

    public interface IInterviewService
    {
        Task<Interview> GetInterviewByIdAsync(Guid id);
        Task<IEnumerable<Interview>> GetInterviewsByJobApplicationIdAsync(Guid jobApplicationId);
        Task<Interview> CreateInterviewAsync(Interview interview);
        Task<Interview> UpdateInterviewAsync(Interview interview);
        Task DeleteInterviewAsync(Guid id);
        Task<List<InterviewQuestion>> GenerateInterviewQuestionsAsync(string jobTitle, string jobDescription);
    }
} 