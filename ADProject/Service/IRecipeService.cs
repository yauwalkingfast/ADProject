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

        Task<bool> DeleteRecipe(int id);

        List<RecipeIngredient> FindRecipeStepsByRecipeId(int id);
        Recipe FindRecipeById(int id);
        void AddRecipeNonAsync(Recipe recipe);

        Task<List<Recipe>> GetAllRecipes();

        Task<List<Recipe>> GetAllRecipesBasic();
        Task<Recipe> GetRecipeById(int? id);
        Task<bool> EditRecipe(int id, Recipe recipe);


    }
}
