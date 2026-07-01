using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIResumeAssistant.Application.Interfaces;
using AIResumeAssistant.Infrastructure.Persistence.Context;
using Azure;
using Azure.AI.DocumentIntelligence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace AIResumeAssistant.Infrastructure
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddInfrastructureDI(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddDbContext<AppDbContext>(options =>
			options.UseSqlServer(
				configuration.GetConnectionString("AIResumeAssistantDb")));

			services.AddSingleton<DocumentIntelligenceClient>(
			provider =>
			{
				IConfiguration config = provider.GetRequiredService<IConfiguration>();

				return new DocumentIntelligenceClient(
					new Uri(config["DocumentIntelligence:Endpoint"]),
					new AzureKeyCredential(config["DocumentIntelligence:ApiKey"]));
			});

			// Azure Blob Storage
			services.AddScoped<IBlobStorageService, BlobStorageService>();

			// Azure Document Intelligence service
			services.AddScoped<IDocumentIntelligenceService, DocumentIntelligenceService>();

			// Azure OpenAI service
			services.AddScoped<IOpenAIService,OpenAIService>();

			return services;
		}
	}
}
