# AI-Powered Resume & Portfolio Builder

A comprehensive solution featuring both desktop (WPF) and web (Blazor) applications that help users create customized resumes and portfolios using AI, advanced design options, and real-time previews.

## 🚀 Features

### 1. AI-Powered Resume Generator
- Users input their work experience, skills, and education
- AI suggests optimized bullet points based on job roles
- AI scans uploaded job descriptions and tailors resumes accordingly
- Keyword optimization to match applicant tracking systems (ATS)

### 2. Live Preview & Custom Design
- Real-time preview of resumes and portfolios
- Multiple resume templates & themes
- Drag-and-drop section organization (Skills, Experience, Projects, Certifications)
- Portfolio website builder with custom themes

### 3. Blazor Web App for Online Access
- User authentication for accessing resumes/portfolios from anywhere
- Export resumes as PDF, DOCX, or JSON
- Share portfolio via a unique URL

### 4. Smart Job Application Tracker
- Track job applications (Applied, Interviewed, Offer, Rejected)
- Automated reminders for follow-ups
- Dashboard with analytics (applications sent, response rate)

### 5. AI-Powered Interview Question Generator
- AI-suggested interview questions & answers based on job titles
- Voice-to-text practice for mock interviews

## 🛠 Tech Stack

### Desktop Application (WPF)
- .NET Core, MVVM pattern
- PDF & DOCX generation (Syncfusion / iTextSharp)
- Fluent UI for modern design

### Web Application (Blazor)
- Blazor WebAssembly (WASM)
- MudBlazor for UI components
- PDF export via Razor components

### Backend
- .NET Core Web API
- Entity Framework Core + SQL Server
- Azure OpenAI for AI features
- JWT-based authentication

## Getting Started

### Prerequisites
- .NET 8.0 SDK or later
- Visual Studio 2022 or later
- SQL Server (or SQL Server Express)
- Azure OpenAI API key (for AI features)

### Installation
1. Clone the repository
2. Open the solution in Visual Studio
3. Configure the connection string in `appsettings.json`
4. Configure the Azure OpenAI API key in `appsettings.json`
5. Run the database migrations
6. Build and run the application

## Project Structure
- `ResumePortfolioBuilder.Core` - Shared core functionality
- `ResumePortfolioBuilder.Desktop` - WPF desktop application
- `ResumePortfolioBuilder.Web` - Blazor web application
- `ResumePortfolioBuilder.API` - Backend API
- `ResumePortfolioBuilder.Data` - Data access layer
- `ResumePortfolioBuilder.AI` - AI services integration 