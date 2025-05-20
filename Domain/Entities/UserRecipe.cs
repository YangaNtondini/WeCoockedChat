using WeCoockedChat.Domain.Common;

namespace WeCoockedChat.Domain.Entities
{
	public class UserRecipe : EntityBase
	{
		public Guid UserId { get; set; }
		public int RecipeId { get; set; }
		public Recipe Recipe { get; set; } = null!;
	}
}
