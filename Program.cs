using Microsoft.EntityFrameworkCore;
using OpenAI;
using WeCoockedChat.Data;
using WeCoockedChat.Domain.Interfaces;
using WeCoockedChat.Infrastructure.Repositories;
using WeCoockedChat.Infrastructure.Repositories.Recipes;
using WeCoockedChat.Services;
using WeCoockedChat.Services.VideoSearch;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDb>(opt =>
	opt.UseSqlServer(builder.Configuration.GetConnectionString("WebsiteContextConnection"),
		sql => sql.EnableRetryOnFailure()));

builder.Services.AddScoped<IRecipeRepository, RecipeRepository>();
builder.Services.AddScoped<IVideoSearch, YouTubeSearch>();
builder.Services.AddScoped<RecipeService>();
builder.Services.AddHttpClient<IVideoSearch, YouTubeSearch>();


builder.Services.AddSingleton(_ =>
{
	var apiKey = builder.Configuration["OpenAI:ApiKey"]; // Add this to appsettings
	return new OpenAIClient(apiKey);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
