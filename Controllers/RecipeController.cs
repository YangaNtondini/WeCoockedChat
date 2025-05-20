using Microsoft.AspNetCore.Mvc;
using WeCoockedChat.DTOs.Recipe;
using WeCoockedChat.Services;

namespace WeCoockedChat.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class RecipeController : ControllerBase
	{
		private readonly RecipeService _service;

		public RecipeController(RecipeService service)
		{
			_service = service;
		}

		[HttpGet]
		public async Task<ActionResult<RecipeDto>> GetByQuery([FromQuery] string query, CancellationToken ct)
		{
			if (string.IsNullOrWhiteSpace(query))
				return BadRequest("Query is required.");

			var recipe = await _service.GetOrCreateAsync(query, ct);

			var dto = new RecipeDto
			{
				Title = recipe.Title,
				Summary = recipe.Summary,
				Ingredients = recipe.Ingredients.Select(i => i.Name).ToList(),
				Steps = recipe.Steps.OrderBy(s => s.Order).Select(s => s.Text).ToList(),
				Videos = recipe.Videos.Select(v => new RecipeVideoDto
				{
					Provider = v.Provider,
					Title = v.Title,
					VideoId = v.VideoId,
					Thumbnail = v.Thumbnail
				}).ToList()
			};

			return Ok(dto);
		}
	}
}
