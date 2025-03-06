using System;
using System.Collections.Generic;

namespace ResumePortfolioBuilder.Core.Models
{
    public class JobApplication
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public string JobTitle { get; set; }
        public string Company { get; set; }
        public string Location { get; set; }
        public string JobDescription { get; set; }
        public string JobUrl { get; set; }
        public decimal? SalaryMin { get; set; }
        public decimal? SalaryMax { get; set; }
        public string SalaryCurrency { get; set; }
        public string ContactName { get; set; }
        public string ContactEmail { get; set; }
        public string ContactPhone { get; set; }
        public DateTime ApplicationDate { get; set; }
        public DateTime? FollowUpDate { get; set; }
        public DateTime? NextFollowUpDate { get; set; }
        public ApplicationStatus Status { get; set; }
        public Guid? ResumeId { get; set; }
        public string Notes { get; set; }
        public List<Interview> Interviews { get; set; } = new List<Interview>();
        public List<JobApplicationNote> ActivityLog { get; set; } = new List<JobApplicationNote>();
    }

    public enum ApplicationStatus
    {
        Bookmarked,
        Applied,
        PhoneScreen,
        Interview,
        Offer,
        Accepted,
        Rejected,
        Withdrawn
    }

    public class Interview
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public string InterviewType { get; set; }
        public string InterviewerName { get; set; }
        public string InterviewerTitle { get; set; }
        public string Location { get; set; }
        public string Notes { get; set; }
        public List<InterviewQuestion> Questions { get; set; } = new List<InterviewQuestion>();
        public InterviewStatus Status { get; set; }
    }

    public enum InterviewStatus
    {
        Scheduled,
        Completed,
        Cancelled,
        Rescheduled
    }

    public class InterviewQuestion
    {
        public Guid Id { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public string Notes { get; set; }
    }

    public class JobApplicationNote
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public string Content { get; set; }
        public NoteType Type { get; set; }
        
        // Alias for compatibility with Razor components
        public string Description { get => Content; set => Content = value; }
    }

    public enum NoteType
    {
        StatusChange,
        FollowUp,
        Note,
        Reminder
    }
} 