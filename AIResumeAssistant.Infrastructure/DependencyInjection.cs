using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIResumeAssistant.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;


namespace AIResumeAssistant.Infrastructure
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddInfrastructureDI(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddDbContext<AppDbContext>(options =>
			options.UseSqlServer(
				configuration.GetConnectionString("AIResumeAssistantDb")));

			return services;
		}
	}
}
