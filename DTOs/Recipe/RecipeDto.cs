namespace WeCoockedChat.DTOs.Recipe
{
	public class RecipeDto
	{
		public string Title { get; set; }
		public string Summary { get; set; }
		public List<string> Ingredients { get; set; }
		public List<string> Steps { get; set; }
		public List<RecipeVideoDto> Videos { get; set; }
	}

	public class RecipeVideoDto
	{
		public string Provider { get; set; } = string.Empty;
		public string Title { get; set; } = string.Empty;
		public string VideoId { get; set; } = string.Empty;
		public string Thumbnail { get; set; } = string.Empty;
	}
}
