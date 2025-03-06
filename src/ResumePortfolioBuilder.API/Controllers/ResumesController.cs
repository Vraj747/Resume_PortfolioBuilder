using System;
using System.Collections.Generic;
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
    public class ResumesController : ControllerBase
    {
        private readonly IResumeRepository _resumeRepository;
        private readonly IResumeAIService _resumeAIService;

        public ResumesController(IResumeRepository resumeRepository, IResumeAIService resumeAIService)
        {
            _resumeRepository = resumeRepository ?? throw new ArgumentNullException(nameof(resumeRepository));
            _resumeAIService = resumeAIService ?? throw new ArgumentNullException(nameof(resumeAIService));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Resume>>> GetResumes()
        {
            var userId = User.Identity.Name;
            var resumes = await _resumeRepository.GetByUserIdAsync(userId);
            return Ok(resumes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Resume>> GetResume(Guid id)
        {
            var resume = await _resumeRepository.GetByIdAsync(id);

            if (resume == null)
            {
                return NotFound();
            }

            // Ensure the user can only access their own resumes
            if (resume.UserId != User.Identity.Name)
            {
                return Forbid();
            }

            return Ok(resume);
        }

        [HttpPost]
        public async Task<ActionResult<Resume>> CreateResume(Resume resume)
        {
            if (resume == null)
            {
                return BadRequest();
            }

            // Set the user ID from the authenticated user
            resume.UserId = User.Identity.Name;

            var createdResume = await _resumeRepository.CreateAsync(resume);
            return CreatedAtAction(nameof(GetResume), new { id = createdResume.Id }, createdResume);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateResume(Guid id, Resume resume)
        {
            if (id != resume.Id)
            {
                return BadRequest();
            }

            var existingResume = await _resumeRepository.GetByIdAsync(id);
            if (existingResume == null)
            {
                return NotFound();
            }

            // Ensure the user can only update their own resumes
            if (existingResume.UserId != User.Identity.Name)
            {
                return Forbid();
            }

            // Preserve the original user ID
            resume.UserId = existingResume.UserId;

            await _resumeRepository.UpdateAsync(resume);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteResume(Guid id)
        {
            var resume = await _resumeRepository.GetByIdAsync(id);
            if (resume == null)
            {
                return NotFound();
            }

            // Ensure the user can only delete their own resumes
            if (resume.UserId != User.Identity.Name)
            {
                return Forbid();
            }

            await _resumeRepository.DeleteAsync(id);
            return NoContent();
        }

        [HttpPost("{id}/generate-content")]
        public async Task<ActionResult<string>> GenerateResumeContent(Guid id, [FromBody] string jobDescription)
        {
            var resume = await _resumeRepository.GetByIdAsync(id);
            if (resume == null)
            {
                return NotFound();
            }

            // Ensure the user can only access their own resumes
            if (resume.UserId != User.Identity.Name)
            {
                return Forbid();
            }

            var generatedContent = await _resumeAIService.GenerateResumeContentAsync(jobDescription, resume);
            return Ok(generatedContent);
        }

        [HttpPost("{id}/suggest-improvements")]
        public async Task<ActionResult<string>> SuggestResumeImprovements(Guid id)
        {
            var resume = await _resumeRepository.GetByIdAsync(id);
            if (resume == null)
            {
                return NotFound();
            }

            // Ensure the user can only access their own resumes
            if (resume.UserId != User.Identity.Name)
            {
                return Forbid();
            }

            var suggestions = await _resumeAIService.GenerateResumeAIFeedback(resume);
            return Ok(suggestions);
        }
    }
} 