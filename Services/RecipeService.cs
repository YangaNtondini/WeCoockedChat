using System.Text.Json;
using System.Text.Json.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using OpenAI;
using OpenAI.Chat;
using WeCoockedChat.Domain.Entities;
using WeCoockedChat.Domain.Interfaces;
using WeCoockedChat.Services.VideoSearch;
namespace WeCoockedChat.Services;

public class RecipeService
{
	private readonly IRecipeRepository _repo;
	private readonly IVideoSearch _videoSearch;
	private readonly OpenAIClient _openAi;

	public RecipeService(IRecipeRepository repo, IVideoSearch videoSearch, OpenAIClient openAi)
	{
		_repo = repo;
		_videoSearch = videoSearch;
		_openAi = openAi;
	}

	public async Task<Recipe> GetOrCreateAsync(string query, CancellationToken ct = default)
	{
		var cached = await _repo.GetByQueryAsync(query, ct);
		if (cached is not null) return cached;

		var recipeJson = await GenerateRecipeJsonAsync(query, ct);
		var options = new JsonSerializerOptions
		{
			PropertyNameCaseInsensitive = true,
			DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
		};
		var dto = JsonSerializer.Deserialize<LlmRecipeDto>(recipeJson, options)
				  ?? throw new InvalidOperationException("Invalid JSON from LLM");

		var recipe = new Recipe
		{
			Slug = Slugify(dto.Title),
			Title = dto.Title,
			Summary = dto.Summary,
			Ingredients = dto.Ingredients.Select(i => new RecipeIngredient { Name = i }).ToList(),
			Steps = dto.Steps.Select((s, idx) => new RecipeStep { Order = idx + 1, Text = s }).ToList()
		};

		var vids = await _videoSearch.SearchAsync(query, ct);
		recipe.Videos = vids.Select(v => new RecipeVideo
		{
			Provider = v.Provider,
			VideoId = v.VideoId,
			Title = v.Title,
			Thumbnail = v.Thumbnail
		}).ToList();

		await _repo.AddAsync(recipe, ct);
		await _repo.SaveChangesAsync(ct);

		return recipe;
	}

	private async Task<string> GenerateRecipeJsonAsync(string query, CancellationToken ct)
	{
		var prompt = $$"""
        Return a concise JSON with keys: title, summary, ingredients[], steps[]
        for the recipe "{{query}}". No other text.
        """;

		var chatMessages = new List<ChatMessage> 
		{ 
			new SystemChatMessage("Your system message here"),
			new UserChatMessage("Your user message here")
		};
		var chatCompletionOptions = new ChatCompletionOptions
		{
			Temperature = 0.7f,
			MaxOutputTokenCount = 150
		};

		var chatClient = _openAi.GetChatClient("gpt-4o-mini");

		var chatResponse = await chatClient.CompleteChatAsync(chatMessages, chatCompletionOptions, ct);
		var reponse = chatResponse.Value.Content.ToString() ?? string.Empty;
		return reponse;
	}

	private static string Slugify(string text)
		=> text.Trim().ToLower().Replace(' ', '-');

	private record LlmRecipeDto(string Title, string Summary, List<string> Ingredients, List<string> Steps);
}
