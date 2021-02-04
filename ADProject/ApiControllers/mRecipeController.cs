using ADProject.JsonObjects;
using ADProject.Models;
using ADProject.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ADProject.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class mRecipeController : ControllerBase
    {
        private readonly ADProjContext _context;
        private readonly IRecipeService _recipesService;

        public mRecipeController(ADProjContext context, IRecipeService recipeService)
        {
            _context = context;
            _recipesService = recipeService;
        }

        /*[HttpGet]
        [Route("{id}")]
        public ActionResult<Recipe> GetRecipe(int id)
        {
            Recipe recipe = _recipesService.FindRecipeById(id);

            if (recipe == null)
            {
                return null;
            }
            return recipe;
        }*/

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<Recipe>> GetRecipeById(int id)
        {
            var recipe = await _recipesService.GetRecipeById(id);

            if (recipe == null)
            {
                return NotFound();
            }

            return recipe;
        }

        [HttpPost]
        //[Route("post")]
        public async Task<ActionResult<Recipe>> CreateRecipe([FromBody] Recipe recipe)
        {
            try
            {
                recipe.User = _context.Users.FirstOrDefault();
                DateTime now = DateTime.Now;
                recipe.DateCreated = now;
                await _recipesService.AddRecipe(recipe);
                //_recipesService.AddRecipeNonAsync(recipe);
                
                return recipe;

            } catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return null;
            }
            
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<booleanJson> DeleteRecipe(int id)
        {
            booleanJson isDeleted = new booleanJson();

            isDeleted.flag = await _recipesService.DeleteRecipe(id);
            return isDeleted;
        }

        /*[HttpDelete]
        [Route("deleterecipe/{​​id}​​")]
        public async Task<ActionResult<booleanJson> DeleteRecipe(int id)
        {​​
            booleanJson sample = new booleanJson();
            try
            {​​
                await _recipesService.DeleteRecipe(id);
                sample.flag = true;
                return sample;
            }​​ catch
            {​​
                sample.flag = false;
                return sample;
            }​​
        }*/


    }
}
