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
    public class PortfoliosController : ControllerBase
    {
        private readonly IPortfolioRepository _portfolioRepository;
        private readonly IPortfolioAIService _portfolioAIService;

        public PortfoliosController(IPortfolioRepository portfolioRepository, IPortfolioAIService portfolioAIService)
        {
            _portfolioRepository = portfolioRepository ?? throw new ArgumentNullException(nameof(portfolioRepository));
            _portfolioAIService = portfolioAIService ?? throw new ArgumentNullException(nameof(portfolioAIService));
        }

        [HttpGet("public/{uniqueUrl}")]
        [AllowAnonymous]
        public async Task<ActionResult<Portfolio>> GetPublicPortfolio(string uniqueUrl)
        {
            var portfolio = await _portfolioRepository.GetByUniqueUrlAsync(uniqueUrl);
            if (portfolio == null)
            {
                return NotFound();
            }

            // Only return public portfolios
            if (!portfolio.IsPublic)
            {
                return NotFound();
            }

            return Ok(portfolio);
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Portfolio>>> GetPortfolios()
        {
            var userId = User.Identity.Name;
            var portfolios = await _portfolioRepository.GetByUserIdAsync(userId);
            return Ok(portfolios);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Portfolio>> GetPortfolio(Guid id)
        {
            var portfolio = await _portfolioRepository.GetByIdAsync(id);
            if (portfolio == null)
            {
                return NotFound();
            }

            // Ensure the user can only access their own portfolios
            if (portfolio.UserId != User.Identity.Name)
            {
                return Forbid();
            }

            return Ok(portfolio);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Portfolio>> CreatePortfolio(Portfolio portfolio)
        {
            if (portfolio == null)
            {
                return BadRequest();
            }

            // Set the user ID from the authenticated user
            portfolio.UserId = User.Identity.Name;

            // Generate a unique URL if not provided
            if (string.IsNullOrEmpty(portfolio.UniqueUrl))
            {
                // Create a URL-friendly version of the title
                var baseUrl = portfolio.Title.ToLower().Replace(" ", "-");
                portfolio.UniqueUrl = $"{baseUrl}-{Guid.NewGuid().ToString().Substring(0, 8)}";
            }

            var createdPortfolio = await _portfolioRepository.CreateAsync(portfolio);
            return CreatedAtAction(nameof(GetPortfolio), new { id = createdPortfolio.Id }, createdPortfolio);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdatePortfolio(Guid id, Portfolio portfolio)
        {
            if (id != portfolio.Id)
            {
                return BadRequest();
            }

            var existingPortfolio = await _portfolioRepository.GetByIdAsync(id);
            if (existingPortfolio == null)
            {
                return NotFound();
            }

            // Ensure the user can only update their own portfolios
            if (existingPortfolio.UserId != User.Identity.Name)
            {
                return Forbid();
            }

            // Preserve the user ID
            portfolio.UserId = existingPortfolio.UserId;

            await _portfolioRepository.UpdateAsync(portfolio);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeletePortfolio(Guid id)
        {
            var portfolio = await _portfolioRepository.GetByIdAsync(id);
            if (portfolio == null)
            {
                return NotFound();
            }

            // Ensure the user can only delete their own portfolios
            if (portfolio.UserId != User.Identity.Name)
            {
                return Forbid();
            }

            await _portfolioRepository.DeleteAsync(id);
            return NoContent();
        }

        [HttpPost("{id}/generate-content")]
        [Authorize]
        public async Task<ActionResult<string>> GeneratePortfolioContent(Guid id)
        {
            var portfolio = await _portfolioRepository.GetByIdAsync(id);
            if (portfolio == null)
            {
                return NotFound();
            }

            // Ensure the user can only access their own portfolios
            if (portfolio.UserId != User.Identity.Name)
            {
                return Forbid();
            }

            var generatedContent = await _portfolioAIService.GeneratePortfolioDescription(portfolio);
            return Ok(generatedContent);
        }

        [HttpPost("{id}/suggest-improvements")]
        [Authorize]
        public async Task<ActionResult<string>> SuggestPortfolioImprovements(Guid id)
        {
            var portfolio = await _portfolioRepository.GetByIdAsync(id);
            if (portfolio == null)
            {
                return NotFound();
            }

            // Ensure the user can only access their own portfolios
            if (portfolio.UserId != User.Identity.Name)
            {
                return Forbid();
            }

            var suggestions = await _portfolioAIService.GeneratePortfolioKeywords(portfolio);
            return Ok(suggestions);
        }
    }
} 