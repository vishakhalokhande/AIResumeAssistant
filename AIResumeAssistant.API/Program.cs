using AIResumeAssistant.API;
using AIResumeAssistant.Application;
using AIResumeAssistant.Infrastructure;
using Azure;
using Azure.AI.DocumentIntelligence;
using Azure.Identity;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddWebAPIDI(builder.Configuration);

builder.Services.AddScoped<BlobStorageService>();

//builder.Configuration.AddAzureKeyVault(
//	new Uri("https://airesume-keyvault.vault.azure.net/"),
//	new DefaultAzureCredential());
//var apiKey = builder.Configuration["AzureOpenAIApiKey"];

builder.Services.AddSingleton<DocumentIntelligenceClient>(
	provider =>
	{
		IConfiguration config = provider.GetRequiredService<IConfiguration>();

		return new DocumentIntelligenceClient(
			new Uri(config["DocumentIntelligence:Endpoint"]),
			new AzureKeyCredential(config["DocumentIntelligence:ApiKey"]));
	});

builder.Services.AddHttpClient<InterviewNotificationService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
	options.AddPolicy("ReactPolicy",
		policy =>
		{
			policy
				.WithOrigins(
				"http://localhost:5173",
				"https://lemon-meadow-0a825890f.7.azurestaticapps.net"
				)
				.AllowAnyHeader()
				.AllowAnyMethod();
		});
});

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
	app.UseSwagger();
	app.UseSwaggerUI();
//}


app.UseHttpsRedirection();
app.UseCors("ReactPolicy");


app.MapControllers();
app.Run();

