﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using RecipeBook.Domain.Dtos;
using RecipeBook.Domain.Entities;
using RecipeBook.Domain.Repositories.Recipe;

namespace RecipeBook.Infrastructure.DataAccess.Repositories
{
    public class RecipeRepository : IRecipeReadOnlyRepository, IRecipeWriteOnlyRepository, IRecipeUpdateOnlyRepository
    {
        private readonly RecipeBookDbContext _dbContext;

        public RecipeRepository(RecipeBookDbContext dbContext) => _dbContext = dbContext;

        public async Task Add(Recipe recipe) => await _dbContext.Recipes.AddAsync(recipe);

        public async Task<IList<Recipe>> Filter(User user, FilterRecipesDto filters)
        {
            var query = _dbContext
                           .Recipes
                           .AsNoTracking()
                           .Include(recipe => recipe.Ingredients)
                           .Where(recipe => recipe.Active && recipe.UserId == user.ID);

            if (filters.Difficulties.Any())
            {
                query = query.Where(recipe => recipe.Difficulty.HasValue && filters.Difficulties.Contains(recipe.Difficulty.Value));
            }

            if (filters.CookingTimes.Any())
            {
                query = query.Where(recipe => recipe.CookingTime.HasValue && filters.CookingTimes.Contains(recipe.CookingTime.Value));
            }

            if (filters.DishTypes.Any())
            {
                query = query.Where(recipe => recipe.DishTypes.Any(dishType => filters.DishTypes.Contains(dishType.Type)));
            }

            if (!string.IsNullOrWhiteSpace(filters.RecipeTitle_Ingredient))
            {
                query = query.Where(
                    recipe => recipe.Title.Contains(filters.RecipeTitle_Ingredient)
                    || recipe.Ingredients.Any(ingredient => ingredient.Name.Contains(filters.RecipeTitle_Ingredient)));
            }

            return await query.ToListAsync();
        }

        async Task<Recipe?> IRecipeReadOnlyRepository.GetById(User user, long recipeId)
        {
            return await GetFullRecipe()
                .AsNoTracking() 
                .FirstOrDefaultAsync(recipe => recipe.ID == recipeId && recipe.Active && recipe.UserId == user.ID);
        }

        async Task<Recipe?> IRecipeUpdateOnlyRepository.GetById(User user, long recipeId)
        {
            return await GetFullRecipe()
                .FirstOrDefaultAsync(recipe => recipe.ID == recipeId && recipe.Active && recipe.UserId == user.ID);
        }

        public async Task<bool> IsValidRecipeOwner(User user, long recipeId) => 
            await _dbContext.Recipes.AnyAsync(recipe => recipe.ID == recipeId && recipe.UserId == user.ID && recipe.Active);

        public async Task<IList<Recipe>> GetForDashbord(User user)
        {
            return await _dbContext.Recipes
                .AsNoTracking()
                .Include(recipe => recipe.Ingredients)
                .Where(recipe => recipe.Active && recipe.UserId == user.ID)
                .OrderByDescending(recipe => recipe.CreatedOn)
                .Take(5)
                .ToListAsync();
        }

        public void Update(Recipe recipe) => _dbContext.Recipes.Update(recipe);

        public async Task Delete(long recipeId)
        {
            var recipe = await _dbContext.Recipes.FindAsync(recipeId);
            _dbContext.Recipes.Remove(recipe!);
        }

        private IIncludableQueryable<Recipe, IList<DishType>> GetFullRecipe()
        {
            return _dbContext
                .Recipes
                .Include(recipe => recipe.Ingredients)
                .Include(recipe => recipe.Instructions)
                .Include(recipe => recipe.DishTypes);
        }
    }
}
