using System.Text;
using System.Text.Json;
using AIResumeAssistant.Application.Interfaces;
using Azure;
using Azure.AI.DocumentIntelligence;
using Azure.AI.OpenAI;
using Azure.Core;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Mvc;
using OpenAI.Chat;



namespace AIResumeAssistant.API.Controllers
{
	[Route("api/resume")]
	[ApiController]

	public class ResumeController : ControllerBase
	{
		private readonly IBlobStorageService _blobStorageService;
		private readonly IDocumentIntelligenceService _documentIntelligenceService;
		private readonly IOpenAIService _openAIService;
		
		public ResumeController(IBlobStorageService blobStorageService, IDocumentIntelligenceService documentIntelligenceService, IOpenAIService openAIService)
		{
			_blobStorageService = blobStorageService;
			_documentIntelligenceService = documentIntelligenceService;
			_openAIService = openAIService;
		}

		[HttpPost("api/resume/upload")]
		public async Task<IActionResult> UploadResumeUploadResume(IFormFile file)
		{
			if (file == null || file.Length == 0)
			{
				return BadRequest("No file selected.");
			}

			// Convert IFormFile to Stream right at the boundary
			string blobUrl = String.Empty;
			using (var stream = file.OpenReadStream())
			{
				blobUrl = await _blobStorageService.UploadFileAsync(stream,file.FileName);
			}

			return Ok(new
			{
				Message = "Resume uploaded successfully",
				BlobUrl = blobUrl
			});
		}

		

		[HttpGet("api/resume/extract/{fileName}")]
		public async Task<IActionResult> ExtractDocumentText(string fileName)
		{
			string resumeExtractedText = await _documentIntelligenceService.ExtractTextAsync(fileName);
			return Ok(new
			{
				Message = "File downloaded successfully",
				Resume = resumeExtractedText
			});

		}

		[HttpGet("api/resume/parse/{resumeText}")]
		public async Task<IActionResult> ParseResume(string resumeText)
		{
			string parsedResumeText = await _openAIService.ParseResume(resumeText);
			return Ok(new
			{
				Message = "File downloaded successfully",
				ParsedResume = parsedResumeText
			});
		}

		[HttpGet("api/resume/match/{resumeText}")]
		public async Task<IActionResult> GetSkillsMatchingScore(string resumeText)
		{
			string skillsMatchingScore = await _openAIService.GetSkillsMatchingScore(resumeText);
			return Ok(new
			{
				Message = "File downloaded successfully",
				ParsedResume = skillsMatchingScore
			});
		}


	}
}
