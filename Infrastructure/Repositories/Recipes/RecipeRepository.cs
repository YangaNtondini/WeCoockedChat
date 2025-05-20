using Microsoft.EntityFrameworkCore;
using WeCoockedChat.Data;
using WeCoockedChat.Domain.Entities;
using WeCoockedChat.Domain.Interfaces;

namespace WeCoockedChat.Infrastructure.Repositories.Recipes
{
		public class RecipeRepository : IRecipeRepository
		{
			private readonly AppDb _db;

			public RecipeRepository(AppDb db)
			{
				_db = db;
			}

			public async Task<Recipe?> GetByQueryAsync(string query, CancellationToken ct = default)
			{
				var slug = Slugify(query);

				return await _db.Recipes
					.Include(r => r.Ingredients)
					.Include(r => r.Steps)
					.Include(r => r.Videos)
					.FirstOrDefaultAsync(r => r.Slug == slug, ct);
			}

			public async Task<Recipe> AddAsync(Recipe recipe, CancellationToken ct = default)
			{
				await _db.Recipes.AddAsync(recipe, ct);
				return recipe;
			}

			public async Task SaveChangesAsync(CancellationToken ct = default)
			{
				await _db.SaveChangesAsync(ct);
			}

			private static string Slugify(string text)
				=> text.Trim().ToLower().Replace(' ', '-');
		}
}
