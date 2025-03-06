using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ResumePortfolioBuilder.Core.Interfaces.Repositories;
using ResumePortfolioBuilder.Core.Models;

namespace ResumePortfolioBuilder.Data.Repositories
{
    public class PortfolioRepository : IPortfolioRepository
    {
        private readonly ApplicationDbContext _context;

        public PortfolioRepository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Portfolio> GetByIdAsync(Guid id)
        {
            return await _context.Portfolios
                .Include(p => p.Sections)
                .Include(p => p.Projects)
                .Include(p => p.SocialMediaLinks)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Portfolio> GetByUniqueUrlAsync(string uniqueUrl)
        {
            return await _context.Portfolios
                .Include(p => p.Sections)
                .Include(p => p.Projects)
                .Include(p => p.SocialMediaLinks)
                .FirstOrDefaultAsync(p => p.UniqueUrl == uniqueUrl);
        }

        public async Task<IEnumerable<Portfolio>> GetByUserIdAsync(string userId)
        {
            return await _context.Portfolios
                .Where(p => p.UserId == userId)
                .Include(p => p.Sections)
                .Include(p => p.Projects)
                .Include(p => p.SocialMediaLinks)
                .ToListAsync();
        }

        public async Task<Portfolio> CreateAsync(Portfolio portfolio)
        {
            await _context.Portfolios.AddAsync(portfolio);
            await _context.SaveChangesAsync();
            return portfolio;
        }

        public async Task<Portfolio> UpdateAsync(Portfolio portfolio)
        {
            _context.Entry(portfolio).State = EntityState.Modified;
            
            // Handle collections
            UpdateCollection(portfolio.Sections, "PortfolioId", portfolio.Id);
            UpdateCollection(portfolio.Projects, "PortfolioId", portfolio.Id);
            UpdateCollection(portfolio.SocialMediaLinks, "PortfolioId", portfolio.Id);
            
            await _context.SaveChangesAsync();
            return portfolio;
        }

        public async Task DeleteAsync(Guid id)
        {
            var portfolio = await _context.Portfolios.FindAsync(id);
            if (portfolio == null)
                return;

            _context.Portfolios.Remove(portfolio);
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