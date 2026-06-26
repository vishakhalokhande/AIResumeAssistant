namespace AIResumeAssistant.API
{
	public class CandidateInterviewRequest
	{
		public string CandidateName { get; set; }
		public string CandidateEmail { get; set; }
		public int MatchScore { get; set; }
		public string JobTitle { get; set; }
	}
}
