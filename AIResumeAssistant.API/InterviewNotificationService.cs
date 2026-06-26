using static AIResumeAssistant.API.Controllers.ResumeController;

namespace AIResumeAssistant.API
{
	public class InterviewNotificationService
	{
		private readonly HttpClient _httpClient;
		private readonly IConfiguration _configuration;

		public InterviewNotificationService(
			HttpClient httpClient,
			IConfiguration configuration)
		{
			_httpClient = httpClient;
			_configuration = configuration;
		}

		public async Task SendInterviewEmailAsync(
			CandidateInterviewRequest request)
		{
			var logicAppUrl =
				_configuration["LogicApp:InterviewEmailUrl"];

			await _httpClient.PostAsJsonAsync(
				logicAppUrl,
				request);
		}
	}

	
}
