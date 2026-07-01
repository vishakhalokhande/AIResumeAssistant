using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIResumeAssistant.Application.Interfaces
{
	public interface IOpenAIService
	{
		Task<string> ParseResume(string resume);
		Task<string> GetSkillsMatchingScore(string resume);
	}
}
