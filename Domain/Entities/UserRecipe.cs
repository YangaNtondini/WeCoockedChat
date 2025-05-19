namespace WeCoockedChat.Domain.Entities
{
	public class UserRecipe
	{
		public Guid UserId { get; set; }
		public int RecipeId { get; set; }
		public Recipe Recipe { get; set; } = null!;
	}
}
