using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIResumeAssistant.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Azure.Storage.Blobs;

namespace AIResumeAssistant.Infrastructure
{
	public class BlobStorageService : IBlobStorageService
	{
		//private readonly IConfiguration _configuration;

		public BlobStorageService()//IConfiguration configuration)
		{
			//_configuration = configuration;
		}
		public async Task<string> UploadFileAsync(Stream fileStream, string fileName)
		{
			//string connectionString = _configuration.GetConnectionString("AzureBlobStorage");
			//var connectionString = _configuration["BlobStorageConnectionString"];
			var connectionString = "DefaultEndpointsProtocol=https;AccountName=storageairesumeassistant;AccountKey=XWZFxL0yZNIeOA2JoMd2iawKl6IKLRc59dCxjbgYhkEWxp24MQ7ReUxVMLIuJvU7fyeMczW3EAvB+AStVxT9yg==;EndpointSuffix=core.windows.net";

			string containerName = "resumes";

			BlobContainerClient containerClient =
				new BlobContainerClient(connectionString, containerName);

			await containerClient.CreateIfNotExistsAsync();

			string fileNameOnBlob =
				$"{Guid.NewGuid()}_{fileName}";

			BlobClient blobClient =
				containerClient.GetBlobClient(fileNameOnBlob);

			//using var stream = fileStream.OpenReadStream();

			// Upload directly using the stream passed from the API layer
			await blobClient.UploadAsync(fileStream, overwrite: true);

			return blobClient.Uri.ToString();
		}

		
	}
}
