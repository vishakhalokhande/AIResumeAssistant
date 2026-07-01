using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIResumeAssistant.Application.Interfaces
{
	public interface IBlobStorageService
	{
		Task<string> UploadFileAsync(Stream fileStream, string fileName);
		
	}
}
