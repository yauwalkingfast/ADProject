using ADProject.Models;
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
            return saveResult == 1;
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

        public List<RecipeIngredient> FindRecipeStepsByRecipeId(int id)
        {
            List<RecipeIngredient> recipeIngredients = _context.RecipeIngredients.Where(
                x => x.Recipe.RecipeId == id).ToList();

            return recipeIngredients;

        }
    }
}
