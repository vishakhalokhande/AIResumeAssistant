using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIResumeAssistant.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AIResumeAssistant.Infrastructure.Persistence.Context
{
	public class AppDbContext : DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
		{
				
		}
		public DbSet<Candidate> candidates { get; set; }
		public DbSet<Resume> resumes { get; set; }
		public DbSet<MatchResult> matches { get; set; }
	}
}
