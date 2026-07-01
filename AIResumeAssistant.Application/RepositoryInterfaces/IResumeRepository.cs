using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIResumeAssistant.Domain.Entities;

namespace AIResumeAssistant.Application.RepositoryInterfaces
{
	internal interface IResumeRepository
	{
		Task AddResumeAsync(Resume resume);

		Task<Resume> GetResumeById(Guid resumeId);

		Task UpdateResumeAsync(Resume resume);
	}
}
