using WeCoockedChat.Domain.Entities;

namespace WeCoockedChat.Domain.Interfaces
{
	public interface IRecipeRepository
	{
		Task<Recipe?> GetByQueryAsync(string query, CancellationToken ct = default);
		Task<Recipe> AddAsync(Recipe recipe, CancellationToken ct = default);
		Task SaveChangesAsync(CancellationToken ct = default);
	}
}
