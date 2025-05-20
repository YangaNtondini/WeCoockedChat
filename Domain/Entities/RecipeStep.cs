using WeCoockedChat.Domain.Common;

namespace WeCoockedChat.Domain.Entities
{
	public class RecipeStep : EntityBase
	{
		public int RecipeId { get; set; }
		public Recipe Recipe { get; set; } = null!;

		public int Order { get; set; }
		public string Text { get; set; } = null!;
		public string? ImageUrl { get; set; }
		public string? VideoId { get; set; }
	}
}
