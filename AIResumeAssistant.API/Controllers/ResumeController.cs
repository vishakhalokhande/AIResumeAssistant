using System.Text;
using System.Text.Json;
using Azure;
using Azure.AI.DocumentIntelligence;
using Azure.AI.OpenAI;
using Azure.Core;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Mvc;
using OpenAI.Chat;
using DocumentLine = Azure.AI.DocumentIntelligence.DocumentLine;
using DocumentPage = Azure.AI.DocumentIntelligence.DocumentPage;


namespace AIResumeAssistant.API.Controllers
{
	[Route("api/resume")]
	[ApiController]

	public class ResumeController : ControllerBase
	{
		private readonly BlobStorageService _blobStorageService;
		private readonly DocumentIntelligenceClient _client;
		private readonly IConfiguration _configuration;
		private readonly InterviewNotificationService _interviewNotificationService;
		public string matchingScoreJson = String.Empty; 
		public ResumeController(BlobStorageService blobStorageService, DocumentIntelligenceClient client, IConfiguration configuration, InterviewNotificationService interviewNotificationService)
		{
			_blobStorageService = blobStorageService;
			_client = client;
			_configuration = configuration;
			_interviewNotificationService = interviewNotificationService;
		}

		[HttpPost("upload")]
		public async Task<IActionResult> UploadResume(IFormFile file)
		{
			if (file == null || file.Length == 0)
			{
				return BadRequest("No file selected.");
			}

			var blobUrl =
				await _blobStorageService.UploadFileAsync(file);

			return Ok(new
			{
				Message = "Resume uploaded successfully",
				BlobUrl = blobUrl
			});
		}

		

		[HttpGet("extract")]
		public async Task<IActionResult> Extract()
		{
			//var connectionString = _configuration["BlobStorageConnectionString"];
			var connectionString = "DefaultEndpointsProtocol=https;AccountName=storageairesumeassistant;AccountKey=XWZFxL0yZNIeOA2JoMd2iawKl6IKLRc59dCxjbgYhkEWxp24MQ7ReUxVMLIuJvU7fyeMczW3EAvB+AStVxT9yg==;EndpointSuffix=core.windows.net";
			string containerName = "resumes";

			BlobContainerClient containerClient =
				new BlobContainerClient(connectionString, containerName);

			await containerClient.CreateIfNotExistsAsync();
			string fileName = "0466be3b-3937-4e03-a61b-1c793cecd9f6_John Doe Resume.docx";

			//string fileName =
			//	$"{Guid.NewGuid()}_{file.FileName}";

			BlobClient blobClient =
				containerClient.GetBlobClient(fileName);

			MemoryStream stream = new MemoryStream();

			await blobClient.DownloadToAsync(stream);



			stream.Position = 0;

			// 2. Wrap stream into BinaryData
			BinaryData documentData = BinaryData.FromStream(stream);

			// 3. Initialize Document Intelligence Client
			//string diURI = _configuration["DocumentIntelligenceURI"];
			//string diKey = _configuration["DocumentIntelligenceKey"];
			string diURI = "https://di-resume-scanning.cognitiveservices.azure.com/";
			string diKey = "5O5W6nu66VgQboRB7v9EZ1BGSM3UHgp2ZUPLS7OtR5H2WILYD1IAJQQJ99CFACYeBjFXJ3w3AAALACOGiLiR";

			DocumentIntelligenceClient client =
				new DocumentIntelligenceClient(
					new Uri(diURI),
					new AzureKeyCredential(diKey));

			// 4. Construct AnalyzeDocumentOptions (Passes ModelId and BinaryData into constructor)
			var options = new AnalyzeDocumentOptions("prebuilt-layout", documentData)
			{
				Locale = "en-US"
				//Pages = "1-2" // Optional configuration: only parse first two pages
			};
			
				// 5. Call the method signature accepting AnalyzeDocumentOptions
				Operation<AnalyzeResult> operation = await client.AnalyzeDocumentAsync(
					WaitUntil.Completed,
					options
				);

				AnalyzeResult result = operation.Value;

				StringBuilder extractedText = new();

				foreach (var paragraph in result.Paragraphs)
				{
					extractedText.AppendLine(paragraph.Content);
				}

			// 6 .We have text stored in extractedText.ToString()
			string resumeExtractedText = extractedText.ToString();

			// 7.  Send Text to Azure OpenAI
			// create client 

			//string openAIEndpoint = _configuration["AIResumeOpenAIEndpoint"];

			//string apiKey = _configuration["AIResumeOpenAIKey"];
			string openAIEndpoint = "https://craftaffairvish-4955-resource.cognitiveservices.azure.com/";
			string apiKey = "D02vYXmPnu1aWByI45Tt9XR0v4kcloUEIvm6JNLMKCCYVEoT0aNzJQQJ99CFACHYHv6XJ3w3AAAAACOG5iWL";
			AzureOpenAIClient openAIClient = new AzureOpenAIClient(
			new Uri(openAIEndpoint),
			new AzureKeyCredential(apiKey));

			// Get chat client:
			ChatClient chatClient = openAIClient.GetChatClient("gpt-5-mini");

			// Step 8: Prompt GPT to Return JSON
			string prompt = $@"Extract the following resume into JSON.
			Return only valid JSON.
			Schema:
			{{
			  ""FullName"": """",
			  ""Email"": """",
			  ""Phone"": """",
			  ""Skills"": [],
			  ""Experience"": [],
			  ""Education"": []
			}}
			Resume:
			{resumeExtractedText}";

			// Step 9: Call GPT
			ChatCompletion completion =
			chatClient.CompleteChat(
			[
				new SystemChatMessage(
					"You are an expert resume parser. Return JSON only."),
				new UserChatMessage(prompt)
			]);

			string resumeJson = completion.Content[0].Text;

			// Step 10: Deserialize
			//		var resume =
			//JsonSerializer.Deserialize<ResumeDto>(json);


			// Give a prompt to openAI to match the score of resume with the job description 
			//string jobDescription = "" +
			//"Need: " +
			//"- C#" +
			//"- .NET 8" +
			//"- Azure" +
			//"- Docker" +
			//"- Kubernetes" +
			//"Experience 5 years";

			string jobDescription = "" +
			"Need: " +
			"- C#" +
			"- .NET Core" +
			"- React";
			

			string promptForMatchingScore = $@"
											Job Description:
											{jobDescription}
											Resume JSON:
											{resumeJson}
											Calculate:
											1. Match percentage (0-100)
											2. Matching skills
											3. Missing skills
											4. Short explanation

											Return ONLY JSON.";

			// Step 9: Call GPT
			ChatCompletion completionResumeScore =
			chatClient.CompleteChat(
			[
				new SystemChatMessage(
					"You are an expert resume parser. Return JSON only."),
				new UserChatMessage(promptForMatchingScore)
			]);

			matchingScoreJson = completionResumeScore.Content[0].Text;

			await _interviewNotificationService
		.SendInterviewEmailAsync(
			new CandidateInterviewRequest
			{
				CandidateName = "Kiaan Bhatter",
				CandidateEmail = "vishakha.lokhande@gmail.com",
				MatchScore = 90,
				JobTitle = "Senior .NET Developer"
			});

			return Ok(matchingScoreJson);

		}
	}
}
