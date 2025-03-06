using System;
using System.Collections.Generic;

namespace ResumePortfolioBuilder.Core.Models
{
    public class Resume
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string UserId { get; set; }
        public string Summary { get; set; }
        public DateTime LastUpdated { get => ModifiedDate; }
        public PersonalInfo PersonalInfo { get; set; }
        public List<WorkExperience> WorkExperiences { get; set; } = new List<WorkExperience>();
        public List<Education> Education { get; set; } = new List<Education>();
        public List<Skill> Skills { get; set; } = new List<Skill>();
        public List<Project> Projects { get; set; } = new List<Project>();
        public List<Certification> Certifications { get; set; } = new List<Certification>();
        public string TemplateId { get; set; }
        public string ThemeId { get; set; }
    }

    public class PersonalInfo
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
        public string LinkedInUrl { get; set; }
        public string GitHubUrl { get; set; }
        public string PortfolioUrl { get; set; }
        public string Summary { get; set; }
        public string Location { get => $"{City}, {State} {ZipCode}"; }
    }

    public class WorkExperience
    {
        public Guid Id { get; set; }
        public string CompanyName { get; set; }
        public string JobTitle { get; set; }
        public string Location { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsCurrentJob { get; set; }
        public List<string> Responsibilities { get; set; } = new List<string>();
        public List<string> Achievements { get; set; } = new List<string>();
        
        // Aliases for compatibility with Razor components
        public string Company { get => CompanyName; set => CompanyName = value; }
        public bool IsCurrent { get => IsCurrentJob; set => IsCurrentJob = value; }
        public string Description { get => string.Join("\n", Responsibilities); set => Responsibilities = new List<string> { value }; }
    }

    public class Education
    {
        public Guid Id { get; set; }
        public string Institution { get; set; }
        public string Degree { get; set; }
        public string FieldOfStudy { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsCurrentEducation { get; set; }
        public string Location { get; set; }
        public string GPA { get; set; }
        public List<string> Achievements { get; set; } = new List<string>();
    }

    public class Skill
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public SkillLevel Level { get; set; }
        public SkillCategory Category { get; set; }
    }

    public enum SkillLevel
    {
        Beginner,
        Intermediate,
        Advanced,
        Expert
    }

    public enum SkillCategory
    {
        Technical,
        SoftSkill,
        Language,
        Tool,
        Other
    }

    public class Project
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsCurrentProject { get; set; }
        public string Url { get; set; }
        public List<string> Technologies { get; set; } = new List<string>();
        public List<string> Achievements { get; set; } = new List<string>();
    }

    public class Certification
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string IssuingOrganization { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public bool DoesNotExpire { get; set; }
        public string CredentialId { get; set; }
        public string CredentialUrl { get; set; }
    }
} 