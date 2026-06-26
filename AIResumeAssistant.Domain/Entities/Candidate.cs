using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIResumeAssistant.Domain.Entities
{
	public class Candidate
	{
		public Guid Id { get; set; }

		public string Name { get; private set; }

		public string Email { get; private set; }

		public Resume Resume { get; private set; }

		public MatchResult MatchResult { get; private set; }

		private Candidate() { }

		public Candidate(string name, string email, Resume resume)
		{
			Id = Guid.NewGuid();
			Name = name;
			Email = email;
			Resume = resume;
		}

		public void SetMatchResult(MatchResult result)
		{
			MatchResult = result;
		}
	}
}
