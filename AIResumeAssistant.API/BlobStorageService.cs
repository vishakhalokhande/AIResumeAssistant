using System.Text;
using Azure.Storage.Blobs;
namespace AIResumeAssistant.API
{
	public class BlobStorageService
	{
		private readonly IConfiguration _configuration;

		public BlobStorageService(IConfiguration configuration)
		{
			_configuration = configuration;
		}
		public async Task<string> UploadFileAsync(IFormFile file)
		{
			//string connectionString = _configuration.GetConnectionString("AzureBlobStorage");
			var connectionString = _configuration["BlobStorageConnectionString"];

			string containerName = "resumes";

			BlobContainerClient containerClient =
				new BlobContainerClient(connectionString, containerName);

			await containerClient.CreateIfNotExistsAsync();

			string fileName =
				$"{Guid.NewGuid()}_{file.FileName}";

			BlobClient blobClient =
				containerClient.GetBlobClient(fileName);

			using var stream = file.OpenReadStream();

			await blobClient.UploadAsync(stream, overwrite: true);

			return blobClient.Uri.ToString();
		}

		public async Task<MemoryStream> DownloadFileAsync()
		{
			var connectionString = _configuration["BlobStorageConnectionString"];

			string containerName = "resumes";

			BlobContainerClient containerClient =
				new BlobContainerClient(connectionString, containerName);

			await containerClient.CreateIfNotExistsAsync();
			string fileName = "add7459b-2c80-4345-8b3f-c7f732523bf4_John Doe Resume.docx";

			//string fileName =
			//	$"{Guid.NewGuid()}_{file.FileName}";

			BlobClient blobClient =
				containerClient.GetBlobClient(fileName);

			MemoryStream stream = new MemoryStream();

			await blobClient.DownloadToAsync(stream);
			//string result = Encoding.UTF8.GetString(stream.ToArray());

			return stream;
		}
	}
}
