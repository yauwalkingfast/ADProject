using ADProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ADProject.Service
{
    public interface IRecipeService
    {
        Task<bool> AddRecipe(Recipe recipe);
        Task<bool> AddRecipeSteps(RecipeStep recipeStep);
        Task<bool> AddRecipeIngredient(RecipeIngredient recipeIngredient);
        //Task<bool> Addfile(byte[] imgbyte);

        List<RecipeIngredient> FindRecipeStepsByRecipeId(int id);
    }
}
