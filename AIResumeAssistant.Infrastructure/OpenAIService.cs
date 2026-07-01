using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIResumeAssistant.Application.Interfaces;
using Azure;
using Azure.AI.OpenAI;
using OpenAI.Chat;

namespace AIResumeAssistant.Infrastructure
{
	public class OpenAIService : IOpenAIService
	{
		public async Task<string> ParseResume(string resume)
		{
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
			{resume}";

			// Step 9: Call GPT
			ChatCompletion completion =
			chatClient.CompleteChat(
			[
				new SystemChatMessage(
					"You are an expert resume parser. Return JSON only."),
				new UserChatMessage(prompt)
			]);

			string resumeJson = completion.Content[0].Text;
			return resumeJson;
		}

		public async Task<string> GetSkillsMatchingScore(string resume)
		{
			string openAIEndpoint = "https://craftaffairvish-4955-resource.cognitiveservices.azure.com/";
			string apiKey = "D02vYXmPnu1aWByI45Tt9XR0v4kcloUEIvm6JNLMKCCYVEoT0aNzJQQJ99CFACHYHv6XJ3w3AAAAACOG5iWL";

			AzureOpenAIClient openAIClient = new AzureOpenAIClient(
			new Uri(openAIEndpoint),
			new AzureKeyCredential(apiKey));

			// Get chat client:
			ChatClient chatClient = openAIClient.GetChatClient("gpt-5-mini");

			string jobDescription = "" +
			"Need: " +
			"- C#" +
			"- .NET Core" +
			"- React";


			string promptForMatchingScore = $@"
											Job Description:
											{jobDescription}
											Resume JSON:
											{resume}
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

			string matchingScoreJson = completionResumeScore.Content[0].Text;
			return matchingScoreJson;

		}
	}
}
