using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace AIResumeAssistant.Application
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddApplicationDI(this IServiceCollection services)
		{
			return services;
		}
	}
}
