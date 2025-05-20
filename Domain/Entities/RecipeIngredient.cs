using WeCoockedChat.Domain.Common;
namespace WeCoockedChat.Domain.Entities
{
	public class RecipeIngredient : EntityBase
	{
		public int RecipeId { get; set; }
		public Recipe Recipe { get; set; } = null!;

		public string Name { get; set; } = null!;
		public string? Quantity { get; set; }
	}
}
