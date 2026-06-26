using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIResumeAssistant.Domain.Entities
{
	public class Resume
	{
		public Guid Id { get; private set; }
		public string Summary { get; private set; }
		public int YearsOfExperience { get; private set; }

		public List<string> Skills { get; private set; }

		private Resume() { }     // Required by EF Core

		public Resume(
			string summary,
			int yearsOfExperience,
			List<string> skills)
		{
			Id = Guid.NewGuid();
			Summary = summary;
			YearsOfExperience = yearsOfExperience;
			Skills = skills;
		}



	}
}
