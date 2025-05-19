namespace WeCoockedChat.Domain.Entities
{
	public class Recipe
	{

		public string Slug { get; set; } = null!;
		public string Title { get; set; } = null!;
		public string? Summary { get; set; }
		public ICollection<RecipeIngredient> Ingredients { get; set; } = new List<RecipeIngredient>();
		public ICollection<RecipeStep> Steps { get; set; } = new List<RecipeStep>();
		public ICollection<RecipeVideo> Videos { get; set; } = new List<RecipeVideo>();

	}
}
