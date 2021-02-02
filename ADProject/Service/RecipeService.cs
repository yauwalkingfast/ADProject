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

        public async Task<Recipe> FindById(int? id)
        {
            var  recipe = await _context.Recipes
                .Include(r => r.User)
                .Include(r => r.RecipeSteps)
                .Include(r => r.RecipeIngredients)
                .FirstOrDefaultAsync(m => m.RecipeId == id);
            return recipe;
        }
    }
}
