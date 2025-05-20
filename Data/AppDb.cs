using Microsoft.EntityFrameworkCore;
using WeCoockedChat.Domain.Common;
using WeCoockedChat.Domain.Entities;

namespace WeCoockedChat.Data;

public class AppDb : DbContext
{
	public AppDb(DbContextOptions<AppDb> opts) : base(opts) { }

	public DbSet<Recipe> Recipes => Set<Recipe>();
	public DbSet<RecipeIngredient> RecipeIngredients => Set<RecipeIngredient>();
	public DbSet<RecipeStep> RecipeSteps => Set<RecipeStep>();
	public DbSet<RecipeVideo> RecipeVideos => Set<RecipeVideo>();
	public DbSet<UserRecipe> UserRecipes => Set<UserRecipe>();

	protected override void OnModelCreating(ModelBuilder b)
	{

		b.Entity<Recipe>()
			.HasIndex(r => r.Slug)
			.IsUnique();

		foreach (var entity in b.Model.GetEntityTypes()
									  .Where(t => typeof(EntityBase).IsAssignableFrom(t.ClrType)))
		{
			b.Entity(entity.ClrType).Property<DateTime>("CreatedOn")
				.HasDefaultValueSql("SYSUTCDATETIME()");
		}
	}

	public override async Task<int> SaveChangesAsync(CancellationToken ct = default)
	{
		foreach (var entry in ChangeTracker.Entries<EntityBase>())
		{
			if (entry.State == EntityState.Modified)
			{
				entry.Entity.ModifiedOn = DateTime.UtcNow;
				entry.Entity.Rev++;
			}
		}

		return await base.SaveChangesAsync(ct);
	}
}
