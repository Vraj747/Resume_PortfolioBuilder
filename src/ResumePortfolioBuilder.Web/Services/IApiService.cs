using ResumePortfolioBuilder.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ResumePortfolioBuilder.Web.Services
{
    public interface IApiService
    {
        // Authentication
        Task<string> LoginAsync(string username, string password);
        Task<bool> RegisterAsync(string username, string email, string password);
        
        // Resumes
        Task<IEnumerable<Resume>> GetResumesAsync();
        Task<Resume> GetResumeAsync(Guid id);
        Task<Resume> CreateResumeAsync(Resume resume);
        Task<Resume> UpdateResumeAsync(Resume resume);
        Task<bool> DeleteResumeAsync(Guid id);
        Task<string> GenerateResumeContentAsync(Guid resumeId, string jobDescription);
        Task<string> SuggestResumeImprovementsAsync(Guid resumeId);
        
        // Portfolios
        Task<IEnumerable<Portfolio>> GetPortfoliosAsync();
        Task<Portfolio> GetPortfolioAsync(Guid id);
        Task<Portfolio> GetPublicPortfolioAsync(string uniqueUrl);
        Task<Portfolio> CreatePortfolioAsync(Portfolio portfolio);
        Task<Portfolio> UpdatePortfolioAsync(Portfolio portfolio);
        Task<bool> DeletePortfolioAsync(Guid id);
        Task<string> GeneratePortfolioContentAsync(Guid id);
        Task<string> SuggestPortfolioImprovementsAsync(Guid id);
        
        // Job Applications
        Task<IEnumerable<JobApplication>> GetJobApplicationsAsync();
        Task<IEnumerable<JobApplication>> GetJobApplicationsByStatusAsync(string status);
        Task<JobApplication> GetJobApplicationAsync(Guid id);
        Task<JobApplication> CreateJobApplicationAsync(JobApplication jobApplication);
        Task<JobApplication> UpdateJobApplicationAsync(JobApplication jobApplication);
        Task<bool> DeleteJobApplicationAsync(Guid id);
        Task<string> GenerateCoverLetterAsync(Guid jobApplicationId);
        Task<string> PrepareInterviewQuestionsAsync(Guid jobApplicationId);
        Task<string> GenerateInterviewAnswerAsync(Guid questionId, Guid resumeId);
    }
} 