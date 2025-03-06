using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Azure;
using Azure.AI.OpenAI;
using Microsoft.Extensions.Configuration;
using ResumePortfolioBuilder.Core.Interfaces;
using ResumePortfolioBuilder.Core.Models;

namespace ResumePortfolioBuilder.AI.Services
{
    public class OpenAIService : IResumeAIService, IPortfolioAIService, IJobApplicationAIService
    {
        private readonly OpenAIClient _client;
        private readonly string _deploymentName;
        private readonly IConfiguration _configuration;

        public OpenAIService(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            
            string endpoint = _configuration["OpenAI:Endpoint"];
            string key = _configuration["OpenAI:Key"];
            _deploymentName = _configuration["OpenAI:DeploymentName"];

            if (string.IsNullOrEmpty(endpoint) || string.IsNullOrEmpty(key) || string.IsNullOrEmpty(_deploymentName))
            {
                throw new InvalidOperationException("OpenAI configuration is missing or incomplete.");
            }

            _client = new OpenAIClient(new Uri(endpoint), new AzureKeyCredential(key));
        }

        #region Resume AI Services

        public async Task<string> GenerateResumeContentAsync(string jobDescription, Resume resume)
        {
            var prompt = new StringBuilder();
            prompt.AppendLine("Generate a tailored resume content based on the following job description and the user's resume information:");
            prompt.AppendLine("\nJob Description:");
            prompt.AppendLine(jobDescription);
            
            prompt.AppendLine("\nUser's Resume Information:");
            prompt.AppendLine($"Name: {resume.PersonalInfo?.FullName}");
            prompt.AppendLine($"Title: {resume.Title}");
            prompt.AppendLine($"Summary: {resume.PersonalInfo?.Summary}");
            
            prompt.AppendLine("\nWork Experience:");
            foreach (var exp in resume.WorkExperiences ?? new List<WorkExperience>())
            {
                prompt.AppendLine($"- {exp.JobTitle} at {exp.CompanyName} ({exp.StartDate:MMM yyyy} - {(exp.EndDate.HasValue ? exp.EndDate.Value.ToString("MMM yyyy") : "Present")})");
                
                if (exp.Responsibilities != null && exp.Responsibilities.Count > 0)
                {
                    prompt.AppendLine("  Responsibilities:");
                    foreach (var responsibility in exp.Responsibilities)
                    {
                        prompt.AppendLine($"  - {responsibility}");
                    }
                }
                
                if (exp.Achievements != null && exp.Achievements.Count > 0)
                {
                    prompt.AppendLine("  Achievements:");
                    foreach (var achievement in exp.Achievements)
                    {
                        prompt.AppendLine($"  - {achievement}");
                    }
                }
            }
            
            prompt.AppendLine("\nEducation:");
            foreach (var edu in resume.Education ?? new List<Education>())
            {
                prompt.AppendLine($"- {edu.Degree} in {edu.FieldOfStudy} from {edu.Institution} ({edu.StartDate:yyyy} - {(edu.EndDate.HasValue ? edu.EndDate.Value.ToString("yyyy") : "Present")})");
            }
            
            prompt.AppendLine("\nSkills:");
            foreach (var skill in resume.Skills ?? new List<Skill>())
            {
                prompt.AppendLine($"- {skill.Name} ({skill.Level})");
            }
            
            prompt.AppendLine("\nPlease generate a tailored resume that highlights the most relevant experience and skills for this job. Format the content in a professional way that can be directly used in a resume.");

            return await GetCompletionAsync(prompt.ToString());
        }

        public async Task<string> SuggestResumeImprovementsAsync(Resume resume)
        {
            var prompt = new StringBuilder();
            prompt.AppendLine("Analyze the following resume and suggest improvements for better impact and readability:");
            
            prompt.AppendLine($"Name: {resume.PersonalInfo?.FullName}");
            prompt.AppendLine($"Title: {resume.Title}");
            prompt.AppendLine($"Summary: {resume.PersonalInfo?.Summary}");
            
            prompt.AppendLine("\nWork Experience:");
            foreach (var exp in resume.WorkExperiences ?? new List<WorkExperience>())
            {
                prompt.AppendLine($"- {exp.JobTitle} at {exp.CompanyName} ({exp.StartDate:MMM yyyy} - {(exp.EndDate.HasValue ? exp.EndDate.Value.ToString("MMM yyyy") : "Present")})");
                
                if (exp.Responsibilities != null && exp.Responsibilities.Count > 0)
                {
                    prompt.AppendLine("  Responsibilities:");
                    foreach (var responsibility in exp.Responsibilities)
                    {
                        prompt.AppendLine($"  - {responsibility}");
                    }
                }
                
                if (exp.Achievements != null && exp.Achievements.Count > 0)
                {
                    prompt.AppendLine("  Achievements:");
                    foreach (var achievement in exp.Achievements)
                    {
                        prompt.AppendLine($"  - {achievement}");
                    }
                }
            }
            
            prompt.AppendLine("\nEducation:");
            foreach (var edu in resume.Education ?? new List<Education>())
            {
                prompt.AppendLine($"- {edu.Degree} in {edu.FieldOfStudy} from {edu.Institution} ({edu.StartDate:yyyy} - {(edu.EndDate.HasValue ? edu.EndDate.Value.ToString("yyyy") : "Present")})");
            }
            
            prompt.AppendLine("\nSkills:");
            foreach (var skill in resume.Skills ?? new List<Skill>())
            {
                prompt.AppendLine($"- {skill.Name} ({skill.Level})");
            }
            
            prompt.AppendLine("\nPlease provide specific suggestions to improve this resume, including:");
            prompt.AppendLine("1. Content improvements (better wording, missing information, etc.)");
            prompt.AppendLine("2. Structure improvements");
            prompt.AppendLine("3. Skills presentation");
            prompt.AppendLine("4. Overall impact and readability");

            return await GetCompletionAsync(prompt.ToString());
        }

        public async Task<string> GenerateResumeOptimizations(Resume resume)
        {
            var prompt = new StringBuilder();
            prompt.AppendLine("Analyze the following resume and suggest optimizations for better impact and readability:");
            
            prompt.AppendLine($"Name: {resume.PersonalInfo?.FullName}");
            prompt.AppendLine($"Title: {resume.Title}");
            prompt.AppendLine($"Summary: {resume.PersonalInfo?.Summary}");
            
            prompt.AppendLine("\nWork Experience:");
            foreach (var exp in resume.WorkExperiences ?? new List<WorkExperience>())
            {
                prompt.AppendLine($"- {exp.JobTitle} at {exp.CompanyName} ({exp.StartDate:MMM yyyy} - {(exp.EndDate.HasValue ? exp.EndDate.Value.ToString("MMM yyyy") : "Present")})");
                
                if (exp.Responsibilities != null && exp.Responsibilities.Count > 0)
                {
                    prompt.AppendLine("  Responsibilities:");
                    foreach (var responsibility in exp.Responsibilities)
                    {
                        prompt.AppendLine($"  - {responsibility}");
                    }
                }
                
                if (exp.Achievements != null && exp.Achievements.Count > 0)
                {
                    prompt.AppendLine("  Achievements:");
                    foreach (var achievement in exp.Achievements)
                    {
                        prompt.AppendLine($"  - {achievement}");
                    }
                }
            }
            
            prompt.AppendLine("\nEducation:");
            foreach (var edu in resume.Education ?? new List<Education>())
            {
                prompt.AppendLine($"- {edu.Degree} in {edu.FieldOfStudy} from {edu.Institution} ({edu.StartDate:yyyy} - {(edu.EndDate.HasValue ? edu.EndDate.Value.ToString("yyyy") : "Present")})");
            }
            
            prompt.AppendLine("\nSkills:");
            foreach (var skill in resume.Skills ?? new List<Skill>())
            {
                prompt.AppendLine($"- {skill.Name} ({skill.Level})");
            }
            
            prompt.AppendLine("\nPlease provide specific suggestions to improve this resume, including:");
            prompt.AppendLine("1. Content improvements (better wording, missing information, etc.)");
            prompt.AppendLine("2. Structure improvements");
            prompt.AppendLine("3. Skills presentation");
            prompt.AppendLine("4. Overall impact and readability");

            return await GetCompletionAsync(prompt.ToString());
        }

        public async Task<string> GenerateResumeKeywords(Resume resume)
        {
            var prompt = new StringBuilder();
            prompt.AppendLine("Extract relevant keywords from the following resume that would be valuable for ATS (Applicant Tracking Systems):");
            
            prompt.AppendLine($"Name: {resume.PersonalInfo?.FullName}");
            prompt.AppendLine($"Title: {resume.Title}");
            prompt.AppendLine($"Summary: {resume.PersonalInfo?.Summary}");
            
            prompt.AppendLine("\nWork Experience:");
            foreach (var exp in resume.WorkExperiences ?? new List<WorkExperience>())
            {
                prompt.AppendLine($"- {exp.JobTitle} at {exp.CompanyName} ({exp.StartDate:MMM yyyy} - {(exp.EndDate.HasValue ? exp.EndDate.Value.ToString("MMM yyyy") : "Present")})");
                
                if (exp.Responsibilities != null && exp.Responsibilities.Count > 0)
                {
                    foreach (var responsibility in exp.Responsibilities)
                    {
                        prompt.AppendLine($"  - {responsibility}");
                    }
                }
            }
            
            prompt.AppendLine("\nSkills:");
            foreach (var skill in resume.Skills ?? new List<Skill>())
            {
                prompt.AppendLine($"- {skill.Name} ({skill.Level})");
            }
            
            prompt.AppendLine("\nPlease extract and categorize keywords from this resume that would be valuable for ATS systems, including:");
            prompt.AppendLine("1. Technical skills");
            prompt.AppendLine("2. Soft skills");
            prompt.AppendLine("3. Industry-specific terminology");
            prompt.AppendLine("4. Action verbs");
            prompt.AppendLine("5. Certifications and qualifications");

            return await GetCompletionAsync(prompt.ToString());
        }

        public async Task<string> GenerateResumeAIFeedback(Resume resume)
        {
            var prompt = new StringBuilder();
            prompt.AppendLine("Provide comprehensive AI feedback on the following resume:");
            
            prompt.AppendLine($"Name: {resume.PersonalInfo?.FullName}");
            prompt.AppendLine($"Title: {resume.Title}");
            prompt.AppendLine($"Summary: {resume.PersonalInfo?.Summary}");
            
            prompt.AppendLine("\nWork Experience:");
            foreach (var exp in resume.WorkExperiences ?? new List<WorkExperience>())
            {
                prompt.AppendLine($"- {exp.JobTitle} at {exp.CompanyName} ({exp.StartDate:MMM yyyy} - {(exp.EndDate.HasValue ? exp.EndDate.Value.ToString("MMM yyyy") : "Present")})");
                
                if (exp.Responsibilities != null && exp.Responsibilities.Count > 0)
                {
                    foreach (var responsibility in exp.Responsibilities)
                    {
                        prompt.AppendLine($"  - {responsibility}");
                    }
                }
                
                if (exp.Achievements != null && exp.Achievements.Count > 0)
                {
                    foreach (var achievement in exp.Achievements)
                    {
                        prompt.AppendLine($"  - {achievement}");
                    }
                }
            }
            
            prompt.AppendLine("\nEducation:");
            foreach (var edu in resume.Education ?? new List<Education>())
            {
                prompt.AppendLine($"- {edu.Degree} in {edu.FieldOfStudy} from {edu.Institution} ({edu.StartDate:yyyy} - {(edu.EndDate.HasValue ? edu.EndDate.Value.ToString("yyyy") : "Present")})");
            }
            
            prompt.AppendLine("\nSkills:");
            foreach (var skill in resume.Skills ?? new List<Skill>())
            {
                prompt.AppendLine($"- {skill.Name} ({skill.Level})");
            }
            
            prompt.AppendLine("\nPlease provide comprehensive AI feedback on this resume, including:");
            prompt.AppendLine("1. Overall impression and impact");
            prompt.AppendLine("2. Content quality and relevance");
            prompt.AppendLine("3. Structure and organization");
            prompt.AppendLine("4. Language and tone");
            prompt.AppendLine("5. ATS compatibility");
            prompt.AppendLine("6. Specific recommendations for improvement");

            return await GetCompletionAsync(prompt.ToString());
        }

        #endregion

        #region Portfolio AI Services

        public async Task<string> GeneratePortfolioContentAsync(Portfolio portfolio)
        {
            var prompt = new StringBuilder();
            prompt.AppendLine("Generate professional content for a portfolio website based on the following information:");
            
            prompt.AppendLine($"Name: {portfolio.PersonalInfo?.FullName}");
            prompt.AppendLine($"Title: {portfolio.Title}");
            prompt.AppendLine($"Description: {portfolio.Description}");
            
            prompt.AppendLine("\nProjects:");
            foreach (var project in portfolio.Projects ?? new List<PortfolioProject>())
            {
                prompt.AppendLine($"- {project.Title}");
                prompt.AppendLine($"  Description: {project.Description}");
                prompt.AppendLine($"  Technologies: {string.Join(", ", project.Technologies ?? new List<string>())}");
            }
            
            prompt.AppendLine("\nPlease generate professional content for the following portfolio sections:");
            prompt.AppendLine("1. About Me section (250-300 words)");
            prompt.AppendLine("2. Skills section with brief descriptions of expertise areas");
            prompt.AppendLine("3. Project descriptions that highlight achievements and technical challenges");
            prompt.AppendLine("4. A professional bio that can be used for the homepage");
            
            prompt.AppendLine("\nThe content should be engaging, professional, and highlight the person's expertise and achievements.");

            return await GetCompletionAsync(prompt.ToString());
        }

        public async Task<string> SuggestPortfolioImprovementsAsync(Portfolio portfolio)
        {
            var prompt = new StringBuilder();
            prompt.AppendLine("Analyze the following portfolio information and suggest improvements for better impact and professional presentation:");
            
            prompt.AppendLine($"Name: {portfolio.PersonalInfo?.FullName}");
            prompt.AppendLine($"Title: {portfolio.Title}");
            prompt.AppendLine($"Description: {portfolio.Description}");
            
            prompt.AppendLine("\nProjects:");
            foreach (var project in portfolio.Projects ?? new List<PortfolioProject>())
            {
                prompt.AppendLine($"- {project.Title}");
                prompt.AppendLine($"  Description: {project.Description}");
                prompt.AppendLine($"  Technologies: {string.Join(", ", project.Technologies ?? new List<string>())}");
            }
            
            prompt.AppendLine("\nPlease provide specific suggestions to improve this portfolio, including:");
            prompt.AppendLine("1. Content improvements for the About section");
            prompt.AppendLine("2. Better ways to present projects and skills");
            prompt.AppendLine("3. Additional sections that might enhance the portfolio");
            prompt.AppendLine("4. Overall structure and organization improvements");
            prompt.AppendLine("5. Professional branding suggestions");

            return await GetCompletionAsync(prompt.ToString());
        }

        public async Task<string> GeneratePortfolioDescription(Portfolio portfolio)
        {
            var prompt = new StringBuilder();
            prompt.AppendLine("Generate a professional portfolio description based on the following information:");
            
            prompt.AppendLine($"Name: {portfolio.PersonalInfo?.FullName}");
            prompt.AppendLine($"Title: {portfolio.Title}");
            prompt.AppendLine($"Description: {portfolio.Description}");
            
            prompt.AppendLine("\nProjects:");
            foreach (var project in portfolio.Projects ?? new List<PortfolioProject>())
            {
                prompt.AppendLine($"- {project.Title}");
                prompt.AppendLine($"  Description: {project.Description}");
                prompt.AppendLine($"  Technologies: {string.Join(", ", project.Technologies ?? new List<string>())}");
            }
            
            prompt.AppendLine("\nPlease generate a compelling portfolio description that:");
            prompt.AppendLine("1. Highlights key skills and expertise");
            prompt.AppendLine("2. Showcases professional achievements");
            prompt.AppendLine("3. Communicates unique value proposition");
            prompt.AppendLine("4. Is engaging and professional in tone");
            prompt.AppendLine("5. Is optimized for both human readers and search engines");

            return await GetCompletionAsync(prompt.ToString());
        }

        public async Task<string> GeneratePortfolioKeywords(Portfolio portfolio)
        {
            var prompt = new StringBuilder();
            prompt.AppendLine("Extract relevant keywords from the following portfolio information:");
            
            prompt.AppendLine($"Name: {portfolio.PersonalInfo?.FullName}");
            prompt.AppendLine($"Title: {portfolio.Title}");
            prompt.AppendLine($"Description: {portfolio.Description}");
            
            prompt.AppendLine("\nProjects:");
            foreach (var project in portfolio.Projects ?? new List<PortfolioProject>())
            {
                prompt.AppendLine($"- {project.Title}");
                prompt.AppendLine($"  Description: {project.Description}");
                prompt.AppendLine($"  Technologies: {string.Join(", ", project.Technologies ?? new List<string>())}");
            }
            
            prompt.AppendLine("\nPlease extract and categorize keywords from this portfolio that would be valuable for SEO and professional branding, including:");
            prompt.AppendLine("1. Technical skills and technologies");
            prompt.AppendLine("2. Industry-specific terminology");
            prompt.AppendLine("3. Professional qualifications");
            prompt.AppendLine("4. Project types and domains");
            prompt.AppendLine("5. Career highlights and achievements");

            return await GetCompletionAsync(prompt.ToString());
        }

        #endregion

        #region Job Application AI Services

        public async Task<string> GenerateCoverLetter(JobApplication jobApplication, Resume resume)
        {
            var prompt = new StringBuilder();
            prompt.AppendLine("Generate a professional cover letter based on the following job application and resume:");
            
            prompt.AppendLine("\nJob Details:");
            prompt.AppendLine($"Job Title: {jobApplication.JobTitle}");
            prompt.AppendLine($"Company: {jobApplication.Company}");
            prompt.AppendLine($"Job Description: {jobApplication.JobDescription}");
            
            prompt.AppendLine("\nResume Information:");
            prompt.AppendLine($"Name: {resume.PersonalInfo?.FullName}");
            prompt.AppendLine($"Title: {resume.Title}");
            prompt.AppendLine($"Summary: {resume.PersonalInfo?.Summary}");
            
            prompt.AppendLine("\nWork Experience:");
            foreach (var exp in resume.WorkExperiences ?? new List<WorkExperience>())
            {
                prompt.AppendLine($"- {exp.JobTitle} at {exp.CompanyName} ({exp.StartDate:MMM yyyy} - {(exp.EndDate.HasValue ? exp.EndDate.Value.ToString("MMM yyyy") : "Present")})");
                
                if (exp.Responsibilities != null && exp.Responsibilities.Count > 0)
                {
                    foreach (var responsibility in exp.Responsibilities)
                    {
                        prompt.AppendLine($"  - {responsibility}");
                    }
                }
                
                if (exp.Achievements != null && exp.Achievements.Count > 0)
                {
                    foreach (var achievement in exp.Achievements)
                    {
                        prompt.AppendLine($"  - {achievement}");
                    }
                }
            }
            
            prompt.AppendLine("\nPlease generate a professional cover letter that:");
            prompt.AppendLine("1. Addresses the hiring manager or recruiter");
            prompt.AppendLine("2. Introduces the candidate and their interest in the position");
            prompt.AppendLine("3. Highlights relevant skills and experiences from the resume that match the job requirements");
            prompt.AppendLine("4. Explains why the candidate is a good fit for the company and role");
            prompt.AppendLine("5. Includes a call to action and contact information");
            prompt.AppendLine("6. Uses a professional tone and format");

            return await GetCompletionAsync(prompt.ToString());
        }

        public async Task<List<InterviewQuestion>> PrepareInterviewQuestionsAsync(JobApplication jobApplication)
        {
            var prompt = new StringBuilder();
            prompt.AppendLine("Generate potential interview questions for the following job application:");
            
            prompt.AppendLine($"Job Title: {jobApplication.JobTitle}");
            prompt.AppendLine($"Company: {jobApplication.Company}");
            prompt.AppendLine($"Job Description: {jobApplication.JobDescription}");
            
            prompt.AppendLine("\nPlease generate 10 potential interview questions that might be asked during an interview for this position, including:");
            prompt.AppendLine("1. Technical questions related to the job requirements");
            prompt.AppendLine("2. Behavioral questions to assess soft skills");
            prompt.AppendLine("3. Questions about experience and background");
            prompt.AppendLine("4. Questions about the company and industry");
            prompt.AppendLine("5. Challenging questions that test problem-solving abilities");
            
            prompt.AppendLine("\nFormat each question as a separate line with no additional text.");

            var response = await GetCompletionAsync(prompt.ToString());
            
            // Parse the response into a list of interview questions
            var questions = new List<InterviewQuestion>();
            var lines = response.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            
            foreach (var line in lines)
            {
                var trimmedLine = line.Trim();
                if (!string.IsNullOrEmpty(trimmedLine))
                {
                    questions.Add(new InterviewQuestion
                    {
                        Id = Guid.NewGuid(),
                        Question = trimmedLine,
                        Answer = string.Empty,
                        Notes = string.Empty
                    });
                }
            }
            
            return questions;
        }

        public async Task<string> GenerateInterviewAnswerAsync(string question, Resume resume, JobApplication jobApplication)
        {
            var prompt = new StringBuilder();
            prompt.AppendLine("Generate a professional answer to the following interview question based on the resume information:");
            
            prompt.AppendLine($"\nQuestion: {question}");
            prompt.AppendLine($"\nJob Title: {jobApplication.JobTitle}");
            prompt.AppendLine($"\nCompany: {jobApplication.Company}");
            
            prompt.AppendLine("\nResume Information:");
            prompt.AppendLine($"Name: {resume.PersonalInfo?.FullName}");
            prompt.AppendLine($"Title: {resume.Title}");
            prompt.AppendLine($"Summary: {resume.PersonalInfo?.Summary}");
            
            prompt.AppendLine("\nWork Experience:");
            foreach (var exp in resume.WorkExperiences ?? new List<WorkExperience>())
            {
                prompt.AppendLine($"- {exp.JobTitle} at {exp.CompanyName} ({exp.StartDate:MMM yyyy} - {(exp.EndDate.HasValue ? exp.EndDate.Value.ToString("MMM yyyy") : "Present")})");
                
                if (exp.Responsibilities != null && exp.Responsibilities.Count > 0)
                {
                    foreach (var responsibility in exp.Responsibilities)
                    {
                        prompt.AppendLine($"  - {responsibility}");
                    }
                }
                
                if (exp.Achievements != null && exp.Achievements.Count > 0)
                {
                    foreach (var achievement in exp.Achievements)
                    {
                        prompt.AppendLine($"  - {achievement}");
                    }
                }
            }
            
            prompt.AppendLine("\nSkills:");
            foreach (var skill in resume.Skills ?? new List<Skill>())
            {
                prompt.AppendLine($"- {skill.Name}");
            }
            
            prompt.AppendLine("\nPlease generate a professional, concise, and effective answer to this interview question that highlights relevant experience and skills from the resume.");

            return await GetCompletionAsync(prompt.ToString());
        }

        public async Task<string> GenerateJobApplicationTips(JobApplication jobApplication)
        {
            var prompt = new StringBuilder();
            prompt.AppendLine("Provide tips for the following job application:");
            
            prompt.AppendLine($"Job Title: {jobApplication.JobTitle}");
            prompt.AppendLine($"Company: {jobApplication.Company}");
            prompt.AppendLine($"Job Description: {jobApplication.JobDescription}");
            prompt.AppendLine($"Current Status: {jobApplication.Status}");
            
            prompt.AppendLine("\nPlease provide tips on:");
            prompt.AppendLine("1. How to improve the application");
            prompt.AppendLine("2. What to focus on in interviews");
            prompt.AppendLine("3. Questions to ask the employer");
            prompt.AppendLine("4. Research to do about the company");
            prompt.AppendLine("5. Next steps in the application process");

            return await GetCompletionAsync(prompt.ToString());
        }

        public async Task<List<InterviewQuestion>> GenerateInterviewQuestions(JobApplication jobApplication, Resume resume)
        {
            var prompt = new StringBuilder();
            prompt.AppendLine("Generate likely interview questions for the following job application:");
            
            prompt.AppendLine($"Job Title: {jobApplication.JobTitle}");
            prompt.AppendLine($"Company: {jobApplication.Company}");
            prompt.AppendLine($"Job Description: {jobApplication.JobDescription}");
            
            if (resume != null)
            {
                prompt.AppendLine("\nCandidate Resume Information:");
                prompt.AppendLine($"Title: {resume.Title}");
                prompt.AppendLine($"Skills: {string.Join(", ", resume.Skills?.Select(s => s.Name) ?? new List<string>())}");
                
                prompt.AppendLine("\nWork Experience:");
                foreach (var exp in resume.WorkExperiences?.Take(2) ?? new List<WorkExperience>())
                {
                    prompt.AppendLine($"- {exp.JobTitle} at {exp.CompanyName}");
                }
            }
            
            prompt.AppendLine("\nGenerate 10 likely interview questions that:");
            prompt.AppendLine("1. Include technical questions related to the job");
            prompt.AppendLine("2. Include behavioral questions");
            prompt.AppendLine("3. Include questions about the candidate's experience");
            prompt.AppendLine("4. Include questions about the candidate's fit for the role");
            prompt.AppendLine("5. Format each question on a new line starting with 'Q: '");

            var response = await GetCompletionAsync(prompt.ToString());
            
            var questions = new List<InterviewQuestion>();
            var lines = response.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            
            foreach (var line in lines)
            {
                if (line.StartsWith("Q:") || line.StartsWith("Question:"))
                {
                    var questionText = line.StartsWith("Q:") ? line.Substring(2).Trim() : line.Substring(9).Trim();
                    questions.Add(new InterviewQuestion
                    {
                        Id = Guid.NewGuid(),
                        Question = questionText
                    });
                }
            }
            
            return questions;
        }

        public async Task<string> GenerateInterviewAnswer(string question, string jobTitle, string resumeText)
        {
            var prompt = new StringBuilder();
            prompt.AppendLine("Generate a professional answer to the following interview question:");
            prompt.AppendLine($"\nJob Title: {jobTitle}");
            prompt.AppendLine($"\nQuestion: {question}");
            
            if (!string.IsNullOrEmpty(resumeText))
            {
                prompt.AppendLine("\nCandidate Resume Information:");
                prompt.AppendLine(resumeText);
            }
            
            prompt.AppendLine("\nPlease provide a professional answer that:");
            prompt.AppendLine("1. Is concise and to the point");
            prompt.AppendLine("2. Uses the STAR method where appropriate (Situation, Task, Action, Result)");
            prompt.AppendLine("3. Highlights relevant skills and experiences");
            prompt.AppendLine("4. Demonstrates value to the employer");
            prompt.AppendLine("5. Sounds natural and conversational");

            return await GetCompletionAsync(prompt.ToString());
        }

        #endregion

        private async Task<string> GetCompletionAsync(string prompt)
        {
            try
            {
                var completionsOptions = new ChatCompletionsOptions
                {
                    DeploymentName = _deploymentName,
                    Messages =
                    {
                        new ChatRequestSystemMessage("You are an AI assistant that helps with professional resume and portfolio content creation, job applications, and interview preparation."),
                        new ChatRequestUserMessage(prompt)
                    },
                    Temperature = 0.7f,
                    MaxTokens = 1000
                };

                Response<ChatCompletions> response = await _client.GetChatCompletionsAsync(completionsOptions);
                return response.Value.Choices[0].Message.Content;
            }
            catch (Exception ex)
            {
                return $"Error generating AI content: {ex.Message}";
            }
        }
    }
} 