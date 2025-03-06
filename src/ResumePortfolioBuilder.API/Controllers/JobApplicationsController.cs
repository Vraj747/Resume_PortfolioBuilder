using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResumePortfolioBuilder.Core.Interfaces;
using ResumePortfolioBuilder.Core.Interfaces.Repositories;
using ResumePortfolioBuilder.Core.Models;

namespace ResumePortfolioBuilder.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class JobApplicationsController : ControllerBase
    {
        private readonly IJobApplicationRepository _jobApplicationRepository;
        private readonly IResumeRepository _resumeRepository;
        private readonly IJobApplicationAIService _jobApplicationAIService;

        public JobApplicationsController(
            IJobApplicationRepository jobApplicationRepository,
            IResumeRepository resumeRepository,
            IJobApplicationAIService jobApplicationAIService)
        {
            _jobApplicationRepository = jobApplicationRepository ?? throw new ArgumentNullException(nameof(jobApplicationRepository));
            _resumeRepository = resumeRepository ?? throw new ArgumentNullException(nameof(resumeRepository));
            _jobApplicationAIService = jobApplicationAIService ?? throw new ArgumentNullException(nameof(jobApplicationAIService));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<JobApplication>>> GetJobApplications()
        {
            var userId = User.Identity.Name;
            var jobApplications = await _jobApplicationRepository.GetByUserIdAsync(userId);
            return Ok(jobApplications);
        }

        [HttpGet("status/{status}")]
        public async Task<ActionResult<IEnumerable<JobApplication>>> GetJobApplicationsByStatus(string status)
        {
            var userId = User.Identity.Name;
            
            // Parse the status string to ApplicationStatus enum
            if (!Enum.TryParse<ApplicationStatus>(status, true, out var applicationStatus))
            {
                return BadRequest("Invalid status value");
            }
            
            var jobApplications = await _jobApplicationRepository.GetByStatusAsync(userId, applicationStatus);
            return Ok(jobApplications);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<JobApplication>> GetJobApplication(Guid id)
        {
            var jobApplication = await _jobApplicationRepository.GetByIdAsync(id);
            if (jobApplication == null)
            {
                return NotFound();
            }

            // Ensure the user can only access their own job applications
            if (jobApplication.UserId != User.Identity.Name)
            {
                return Forbid();
            }

            return Ok(jobApplication);
        }

        [HttpPost]
        public async Task<ActionResult<JobApplication>> CreateJobApplication(JobApplication jobApplication)
        {
            if (jobApplication == null)
            {
                return BadRequest();
            }

            // Set the user ID from the authenticated user
            jobApplication.UserId = User.Identity.Name;

            var createdJobApplication = await _jobApplicationRepository.CreateAsync(jobApplication);
            return CreatedAtAction(nameof(GetJobApplication), new { id = createdJobApplication.Id }, createdJobApplication);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateJobApplication(Guid id, JobApplication jobApplication)
        {
            if (id != jobApplication.Id)
            {
                return BadRequest();
            }

            var existingJobApplication = await _jobApplicationRepository.GetByIdAsync(id);
            if (existingJobApplication == null)
            {
                return NotFound();
            }

            // Ensure the user can only update their own job applications
            if (existingJobApplication.UserId != User.Identity.Name)
            {
                return Forbid();
            }

            // Preserve the user ID
            jobApplication.UserId = existingJobApplication.UserId;

            await _jobApplicationRepository.UpdateAsync(jobApplication);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteJobApplication(Guid id)
        {
            var jobApplication = await _jobApplicationRepository.GetByIdAsync(id);
            if (jobApplication == null)
            {
                return NotFound();
            }

            // Ensure the user can only delete their own job applications
            if (jobApplication.UserId != User.Identity.Name)
            {
                return Forbid();
            }

            await _jobApplicationRepository.DeleteAsync(id);
            return NoContent();
        }

        [HttpPost("{id}/generate-cover-letter")]
        public async Task<ActionResult<string>> GenerateCoverLetter(Guid id, [FromBody] Guid resumeId)
        {
            var jobApplication = await _jobApplicationRepository.GetByIdAsync(id);
            if (jobApplication == null)
            {
                return NotFound();
            }

            // Ensure the user can only access their own job applications
            if (jobApplication.UserId != User.Identity.Name)
            {
                return Forbid();
            }

            var resume = await _resumeRepository.GetByIdAsync(resumeId);
            if (resume == null)
            {
                return NotFound("Resume not found");
            }

            // Ensure the user can only access their own resumes
            if (resume.UserId != User.Identity.Name)
            {
                return Forbid();
            }

            var coverLetter = await _jobApplicationAIService.GenerateCoverLetter(jobApplication, resume);
            return Ok(coverLetter);
        }

        [HttpPost("{id}/prepare-interview-questions")]
        public async Task<ActionResult<string>> PrepareInterviewQuestions(Guid id)
        {
            var jobApplication = await _jobApplicationRepository.GetByIdAsync(id);
            if (jobApplication == null)
            {
                return NotFound();
            }

            // Ensure the user can only access their own job applications
            if (jobApplication.UserId != User.Identity.Name)
            {
                return Forbid();
            }

            var questions = await _jobApplicationAIService.GenerateInterviewQuestions(jobApplication, null);
            return Ok(questions);
        }

        [HttpPost("interview-questions/{questionId}/generate-answer")]
        public async Task<ActionResult<string>> GenerateInterviewAnswer(Guid questionId, [FromBody] Guid resumeId)
        {
            // Get the user ID
            var userId = User.Identity.Name;
            
            // Get all job applications for the user
            var jobApplications = await _jobApplicationRepository.GetByUserIdAsync(userId);
            
            // Find the interview question
            InterviewQuestion question = null;
            foreach (var jobApp in jobApplications)
            {
                if (jobApp.Interviews != null)
                {
                    foreach (var interview in jobApp.Interviews)
                    {
                        if (interview.Questions != null)
                        {
                            question = interview.Questions.FirstOrDefault(q => q.Id == questionId);
                            if (question != null)
                            {
                                break;
                            }
                        }
                    }
                    
                    if (question != null)
                    {
                        break;
                    }
                }
            }
            
            if (question == null)
            {
                return NotFound("Interview question not found");
            }
            
            // Get the resume
            var resume = await _resumeRepository.GetByIdAsync(resumeId);
            if (resume == null)
            {
                return NotFound("Resume not found");
            }
            
            // Ensure the user can only access their own resumes
            if (resume.UserId != userId)
            {
                return Forbid();
            }
            
            var answer = await _jobApplicationAIService.GenerateInterviewAnswer(question.Question, resume.Title, resume.ToString());
            return Ok(answer);
        }
    }
} 