using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIResumeAssistant.Domain.Entities;

namespace AIResumeAssistant.Application.Interfaces
{
	public interface ICandidateRepository
	{
		//Task AddAsync(Candidate candidate);

		Task<Candidate?> GetByIdAsync(Guid candidateId);

		//Task UpdateAsync(Candidate candidate);
	}
}
