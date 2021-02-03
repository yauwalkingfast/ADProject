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

        [HttpGet]
        [Route("{id}")]
        public ActionResult<Recipe> GetRecipe(int id)
        {
            Recipe recipe = _recipesService.FindRecipeById(id);
            
            if (recipe == null)
            {
                return null;
            }
            return recipe;
        }

        [HttpPost]
        //[Route("post")]
        public ActionResult<Recipe> CreateRecipe([FromBody] Recipe recipe)
        {
            try
            {
                recipe.User = _context.Users.FirstOrDefault();
                DateTime now = DateTime.Now;
                recipe.DateCreated = now;
                _recipesService.AddRecipeNonAsync(recipe);
                
                return recipe;

            } catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return null;
            }
            
        }

    }
}
