using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ResumePortfolioBuilder.AI.Services;
using ResumePortfolioBuilder.Core.Interfaces;

namespace ResumePortfolioBuilder.AI
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAIServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Register OpenAI service for all AI interfaces
            services.AddScoped<OpenAIService>();
            services.AddScoped<IResumeAIService>(provider => provider.GetRequiredService<OpenAIService>());
            services.AddScoped<IPortfolioAIService>(provider => provider.GetRequiredService<OpenAIService>());
            services.AddScoped<IJobApplicationAIService>(provider => provider.GetRequiredService<OpenAIService>());

            return services;
        }
    }
} 