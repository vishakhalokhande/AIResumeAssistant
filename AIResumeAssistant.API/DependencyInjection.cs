using AIResumeAssistant.Application;
using AIResumeAssistant.Infrastructure;

namespace AIResumeAssistant.API
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddWebAPIDI(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddApplicationDI().AddInfrastructureDI(configuration);
			return services;
		}
	}
}
