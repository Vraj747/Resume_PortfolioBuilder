using System.Collections.Generic;
using System.Threading.Tasks;
using ResumePortfolioBuilder.Core.Models;

namespace ResumePortfolioBuilder.Core.Interfaces
{
    public interface IAIService
    {
        Task<string> GenerateOptimizedBulletPoint(string originalText, string jobRole);
        Task<List<string>> GenerateResumeOptimizations(string resumeText, string jobDescription);
        Task<List<string>> IdentifyKeywords(string jobDescription);
        Task<List<string>> GenerateInterviewQuestions(string jobTitle, string jobDescription);
        Task<string> GenerateInterviewAnswer(string question, string jobTitle, string resumeText);
    }

    public interface IResumeAIService
    {
        Task<string> GenerateResumeOptimizations(Resume resume);
        Task<string> GenerateResumeKeywords(Resume resume);
        Task<string> GenerateResumeAIFeedback(Resume resume);
        Task<string> GenerateResumeContentAsync(string jobDescription, Resume resume);
    }

    public interface IPortfolioAIService
    {
        Task<string> GeneratePortfolioDescription(Portfolio portfolio);
        Task<string> GeneratePortfolioKeywords(Portfolio portfolio);
    }

    public interface IJobApplicationAIService
    {
        Task<string> GenerateCoverLetter(JobApplication jobApplication, Resume resume);
        Task<string> GenerateJobApplicationTips(JobApplication jobApplication);
        Task<List<InterviewQuestion>> GenerateInterviewQuestions(JobApplication jobApplication, Resume resume);
        Task<string> GenerateInterviewAnswer(string question, string jobTitle, string resumeText);
    }
} 