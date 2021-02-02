using ADProject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ADProject.Service
{
    public class RecipeService : IRecipeService
    {
        private readonly ADProjContext _context;

        public RecipeService(ADProjContext context)
        {
            _context = context;
        }

        public async Task<bool> AddRecipe(Recipe recipe)
        {
            _context.Add(recipe);
            var saveResult = await _context.SaveChangesAsync();
            return saveResult >= 1;
        }

        public async Task<bool> AddRecipeIngredient(RecipeIngredient recipeIngredient)
        {
            _context.Add(recipeIngredient);
            var saveResult = await _context.SaveChangesAsync();
            return saveResult == 1;
        }

        public async Task<bool> AddRecipeSteps(RecipeStep recipeStep)
        {
            _context.Add(recipeStep);
            var saveResult = await _context.SaveChangesAsync();
            return saveResult == 1;
        }


        public async Task<bool> DeleteRecipe(int id)
        {
            var recipe = await _context.Recipes
                .Include(r => r.User)
                //.Include(r => r.RecipeSteps)
                //.Include(r => r.RecipeIngredients)
                .FirstOrDefaultAsync(m => m.RecipeId == id);
            
            _context.Recipes.Remove(recipe); 
            var saveResult = await _context.SaveChangesAsync();
            return saveResult == 1;


        }

        public List<RecipeIngredient> FindRecipeStepsByRecipeId(int id)
        {
            List<RecipeIngredient> recipeIngredients = _context.RecipeIngredients.Where(
                x => x.Recipe.RecipeId == id).ToList();

            return recipeIngredients;
        }

        public async Task<bool> EditRecipe(int id, Recipe recipe)
        {
            try
            {
                var dbRecipe = await _context.Recipes
                    .Include(r => r.RecipeSteps)
                    .Include(r => r.RecipeIngredients)
                    .FirstOrDefaultAsync(r => r.RecipeId == id);

                _context.RecipeIngredients.RemoveRange(dbRecipe.RecipeIngredients);
                _context.RecipeSteps.RemoveRange(dbRecipe.RecipeSteps);

                dbRecipe.Title = recipe.Title;
                dbRecipe.Description = recipe.Description;
                dbRecipe.DurationInMins = recipe.DurationInMins;
                dbRecipe.Calories = recipe.Calories;
                dbRecipe.ServingSize = recipe.ServingSize;
                dbRecipe.IsPublished = recipe.IsPublished;
                dbRecipe.MainMediaUrl = recipe.MainMediaUrl;
                dbRecipe.RecipeIngredients = recipe.RecipeIngredients;
                dbRecipe.RecipeSteps = recipe.RecipeSteps;

                await _context.SaveChangesAsync();
                return true;
            } catch(DbUpdateConcurrencyException)
            {
                return false;
            }

        }

        public async Task<List<Recipe>> GetAllRecipes()
        {
            return await _context.Recipes
                .Include(r => r.User)
                .Include(r => r.RecipeSteps)
                .Include(r => r.RecipeIngredients)
                .Include(r => r.Comments)
                .Include(r => r.LikesDislikes)
                .ToListAsync();
        }

        public async Task<Recipe> GetRecipeById(int? id)
        {
            return await _context.Recipes
                .Include(r => r.User)
                .Include(r => r.RecipeSteps)
                .Include(r => r.RecipeIngredients)
                .Include(r => r.Comments)
                .Include(r => r.LikesDislikes)
                .FirstOrDefaultAsync(r => r.RecipeId == id);

        }
    }
}
