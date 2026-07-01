using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIResumeAssistant.Application.Interfaces;
using Azure;
using Azure.AI.DocumentIntelligence;
using Azure.Storage.Blobs;

namespace AIResumeAssistant.Infrastructure
{
	internal class DocumentIntelligenceService : IDocumentIntelligenceService
	{
		public async Task<string> ExtractTextAsync(string blobUrl)
		{
			//var connectionString = _configuration["BlobStorageConnectionString"];
			var connectionString = "DefaultEndpointsProtocol=https;AccountName=storageairesumeassistant;AccountKey=XWZFxL0yZNIeOA2JoMd2iawKl6IKLRc59dCxjbgYhkEWxp24MQ7ReUxVMLIuJvU7fyeMczW3EAvB+AStVxT9yg==;EndpointSuffix=core.windows.net";
			string containerName = "resumes";

			BlobContainerClient containerClient =
				new BlobContainerClient(connectionString, containerName);

			await containerClient.CreateIfNotExistsAsync();
			//string fileName = "0466be3b-3937-4e03-a61b-1c793cecd9f6_John Doe Resume.docx";

			//string fileName =
			//	$"{Guid.NewGuid()}_{file.FileName}";

			BlobClient blobClient =
				containerClient.GetBlobClient(blobUrl);

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
			return resumeExtractedText;

		}
	}
}
