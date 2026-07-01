using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIResumeAssistant.Application.Interfaces
{
	public interface IDocumentIntelligenceService
	{
		Task<string> ExtractTextAsync(string blobUrl);
	}
}
