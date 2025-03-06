using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using ResumePortfolioBuilder.Core.Models;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;

namespace ResumePortfolioBuilder.Web.Services
{
    public class ApiService : IApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        private readonly JsonSerializerOptions _jsonOptions;

        public ApiService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _baseUrl = configuration["ApiSettings:BaseUrl"];
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        private void SetAuthorizationHeader(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        // Authentication
        public async Task<string> LoginAsync(string username, string password)
        {
            var loginData = new { Username = username, Password = password };
            var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/auth/login", loginData);
            
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
                return result.Token;
            }
            
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Login failed: {errorContent}");
        }

        public async Task<bool> RegisterAsync(string username, string email, string password)
        {
            var registerData = new { Username = username, Email = email, Password = password };
            var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/auth/register", registerData);
            
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Registration failed: {errorContent}");
        }

        // Resumes
        public async Task<IEnumerable<Resume>> GetResumesAsync()
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/resumes");
            
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<IEnumerable<Resume>>(_jsonOptions);
            }
            
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Failed to get resumes: {errorContent}");
        }

        public async Task<Resume> GetResumeAsync(Guid id)
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/resumes/{id}");
            
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Resume>(_jsonOptions);
            }
            
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Failed to get resume: {errorContent}");
        }

        public async Task<Resume> CreateResumeAsync(Resume resume)
        {
            var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/resumes", resume);
            
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Resume>(_jsonOptions);
            }
            
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Failed to create resume: {errorContent}");
        }

        public async Task<Resume> UpdateResumeAsync(Resume resume)
        {
            var response = await _httpClient.PutAsJsonAsync($"{_baseUrl}/resumes/{resume.Id}", resume);
            
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Resume>(_jsonOptions);
            }
            
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Failed to update resume: {errorContent}");
        }

        public async Task<bool> DeleteResumeAsync(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"{_baseUrl}/resumes/{id}");
            
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Failed to delete resume: {errorContent}");
        }

        public async Task<string> GenerateResumeContentAsync(Guid resumeId, string jobDescription)
        {
            var requestData = new { JobDescription = jobDescription };
            var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/resumes/{resumeId}/generate-content", requestData);
            
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<GenerationResponse>();
                return result.Content;
            }
            
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Failed to generate resume content: {errorContent}");
        }

        public async Task<string> SuggestResumeImprovementsAsync(Guid resumeId)
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/resumes/{resumeId}/suggest-improvements");
            
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<GenerationResponse>();
                return result.Content;
            }
            
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Failed to suggest resume improvements: {errorContent}");
        }

        // Portfolios
        public async Task<IEnumerable<Portfolio>> GetPortfoliosAsync()
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/portfolios");
            
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<IEnumerable<Portfolio>>(_jsonOptions);
            }
            
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Failed to get portfolios: {errorContent}");
        }

        public async Task<Portfolio> GetPortfolioAsync(Guid id)
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/portfolios/{id}");
            
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Portfolio>(_jsonOptions);
            }
            
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Failed to get portfolio: {errorContent}");
        }

        public async Task<Portfolio> CreatePortfolioAsync(Portfolio portfolio)
        {
            var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/portfolios", portfolio);
            
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Portfolio>(_jsonOptions);
            }
            
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Failed to create portfolio: {errorContent}");
        }

        public async Task<Portfolio> UpdatePortfolioAsync(Portfolio portfolio)
        {
            var response = await _httpClient.PutAsJsonAsync($"{_baseUrl}/portfolios/{portfolio.Id}", portfolio);
            
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Portfolio>(_jsonOptions);
            }
            
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Failed to update portfolio: {errorContent}");
        }

        public async Task<bool> DeletePortfolioAsync(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"{_baseUrl}/portfolios/{id}");
            
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Failed to delete portfolio: {errorContent}");
        }

        // Job Applications
        public async Task<IEnumerable<JobApplication>> GetJobApplicationsAsync()
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/job-applications");
            
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<IEnumerable<JobApplication>>(_jsonOptions);
            }
            
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Failed to get job applications: {errorContent}");
        }

        public async Task<IEnumerable<JobApplication>> GetJobApplicationsByStatusAsync(string status)
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/job-applications/status/{status}");
            
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<IEnumerable<JobApplication>>(_jsonOptions);
            }
            
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Failed to get job applications by status: {errorContent}");
        }

        public async Task<JobApplication> GetJobApplicationAsync(Guid id)
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/job-applications/{id}");
            
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<JobApplication>(_jsonOptions);
            }
            
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Failed to get job application: {errorContent}");
        }

        public async Task<JobApplication> CreateJobApplicationAsync(JobApplication jobApplication)
        {
            var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/job-applications", jobApplication);
            
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<JobApplication>(_jsonOptions);
            }
            
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Failed to create job application: {errorContent}");
        }

        public async Task<JobApplication> UpdateJobApplicationAsync(JobApplication jobApplication)
        {
            var response = await _httpClient.PutAsJsonAsync($"{_baseUrl}/job-applications/{jobApplication.Id}", jobApplication);
            
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<JobApplication>(_jsonOptions);
            }
            
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Failed to update job application: {errorContent}");
        }

        public async Task<bool> DeleteJobApplicationAsync(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"{_baseUrl}/job-applications/{id}");
            
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Failed to delete job application: {errorContent}");
        }

        public async Task<string> GenerateCoverLetterAsync(Guid jobApplicationId)
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/job-applications/{jobApplicationId}/generate-cover-letter");
            
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<GenerationResponse>();
                return result.Content;
            }
            
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Failed to generate cover letter: {errorContent}");
        }

        public async Task<Portfolio> GetPublicPortfolioAsync(string uniqueUrl)
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/portfolios/public/{uniqueUrl}");
            
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Portfolio>(_jsonOptions);
            }
            
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Failed to get public portfolio: {errorContent}");
        }

        public async Task<string> GeneratePortfolioContentAsync(Guid id)
        {
            var response = await _httpClient.PostAsync($"{_baseUrl}/portfolios/{id}/generate-content", null);
            
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<GenerationResponse>();
                return result.Content;
            }
            
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Failed to generate portfolio content: {errorContent}");
        }

        public async Task<string> SuggestPortfolioImprovementsAsync(Guid id)
        {
            var response = await _httpClient.PostAsync($"{_baseUrl}/portfolios/{id}/suggest-improvements", null);
            
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<GenerationResponse>();
                return result.Content;
            }
            
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Failed to suggest portfolio improvements: {errorContent}");
        }

        public async Task<string> PrepareInterviewQuestionsAsync(Guid jobApplicationId)
        {
            var response = await _httpClient.PostAsync($"{_baseUrl}/job-applications/{jobApplicationId}/prepare-interview-questions", null);
            
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<GenerationResponse>();
                return result.Content;
            }
            
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Failed to prepare interview questions: {errorContent}");
        }

        public async Task<string> GenerateInterviewAnswerAsync(Guid questionId, Guid resumeId)
        {
            var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/job-applications/interview-questions/{questionId}/generate-answer", resumeId);
            
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<GenerationResponse>();
                return result.Content;
            }
            
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Failed to generate interview answer: {errorContent}");
        }

        private class LoginResponse
        {
            public string Token { get; set; }
        }

        private class GenerationResponse
        {
            public string Content { get; set; }
        }
    }
} 