using WeCoockedChat.Domain.Common;

namespace WeCoockedChat.Domain.Entities
{
	public class RecipeVideo : EntityBase
	{
		public int RecipeId { get; set; }
		public Recipe Recipe { get; set; } = null!;

		public string Provider { get; set; } = null!;
		public string VideoId { get; set; } = null!;
		public string Title { get; set; } = null!;
		public string Thumbnail { get; set; } = null!;
	}
}
