using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIResumeAssistant.Domain.Entities
{
	public class MatchResult
	{
		public Guid Id { get; private set; }

		public int MatchPercentage { get; private set; }

		public bool InterviewEligible { get; private set; }

		private MatchResult() { }

		public MatchResult(int matchPercentage)
		{
			Id = Guid.NewGuid();

			MatchPercentage = matchPercentage;

			InterviewEligible = matchPercentage >= 80;
		}
	}
}
