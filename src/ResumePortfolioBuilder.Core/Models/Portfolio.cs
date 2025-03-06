using System;
using System.Collections.Generic;

namespace ResumePortfolioBuilder.Core.Models
{
    public class Portfolio
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string UserId { get; set; }
        public string Description { get; set; }
        public string ThemeId { get; set; }
        public string CustomDomain { get; set; }
        public string UniqueUrl { get; set; }
        public bool IsPublic { get; set; }
        public PersonalInfo PersonalInfo { get; set; }
        public List<PortfolioSection> Sections { get; set; } = new List<PortfolioSection>();
        public List<PortfolioProject> Projects { get; set; } = new List<PortfolioProject>();
        public List<SocialMediaLink> SocialMediaLinks { get; set; } = new List<SocialMediaLink>();
        
        // Aliases for compatibility with Razor components
        public DateTime LastUpdated { get => ModifiedDate; }
        public string About { get => Description; set => Description = value; }
    }

    public class PortfolioSection
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int Order { get; set; }
        public SectionType Type { get; set; }
    }

    public enum SectionType
    {
        About,
        Skills,
        Experience,
        Education,
        Projects,
        Contact,
        Custom
    }

    public class PortfolioProject
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string ProjectUrl { get; set; }
        public string GitHubUrl { get; set; }
        public DateTime Date { get; set; }
        public List<string> Technologies { get; set; } = new List<string>();
        public List<string> Features { get; set; } = new List<string>();
        public int Order { get; set; }
    }

    public class SocialMediaLink
    {
        public Guid Id { get; set; }
        public string Platform { get; set; }
        public string Url { get; set; }
        public string Icon { get; set; }
    }
} 