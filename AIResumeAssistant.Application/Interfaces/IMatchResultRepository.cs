using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIResumeAssistant.Domain.Entities;

namespace AIResumeAssistant.Application.Interfaces
{
	internal interface IMatchResultRepository
	{
		Task AddAsync(MatchResult result);

		Task<MatchResult?> GetByCandidateIdAsync(Guid candidateId);
	}
}
