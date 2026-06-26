using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIResumeAssistant.Application.Interfaces;
using AIResumeAssistant.Domain.Entities;

namespace AIResumeAssistant.Infrastructure.Persistence.Repositories
{
	public class CandidateRepository : ICandidateRepository
	{
		public Task<Candidate> GetByIdAsync(Guid id)
		{
			throw new NotImplementedException();
		}
	}
}
