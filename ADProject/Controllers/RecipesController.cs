using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ADProject.Models;
using ADProject.Service;
using System.Net.Http;
using System.Net.Http.Headers;
using ADProject.JsonObjects;
using Newtonsoft.Json;
using System.Diagnostics;
using ADProject.GenerateTagsClass;

namespace ADProject.Controllers
{
    public class RecipesController : Controller
    {
        private readonly ADProjContext _context;
        private readonly IRecipeService _recipesService;


        public RecipesController(ADProjContext context, IRecipeService recipeService)
        {
            _context = context;
            _recipesService = recipeService;
        }

        // GET: Recipes
        public async Task<IActionResult> Index()
        {
            return View(await _recipesService.GetAllRecipes());
        }

        // GET: Recipes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipe = await _recipesService.GetRecipeById(id);

            if (recipe == null)
            {
                return NotFound();
            }

            return View(recipe);
        }

        //GET: Recipes/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = _context.Users.FirstOrDefault().UserId; 
            ViewData["Recipe"] = new Recipe();
            return View();
        }

        // POST: Recipes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromBody] Recipe recipe)
        {
            User user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == recipe.UserId);
            recipe.User = user;
            DateTime now = DateTime.Now;
            recipe.DateCreated = now;
            
            var successful = await _recipesService.AddRecipe(recipe);
            if (successful)
            {
                return Ok();
            }

            ViewData["UserId"] = recipe.UserId;
            return BadRequest();
        }


        // GET: Recipes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipe = await _recipesService.GetRecipeById(id);

            if (recipe == null)
            {
                return NotFound();
            }

            ViewData["UserId"] = _context.Users.FirstOrDefault().UserId;
            string json = JsonConvert.SerializeObject(recipe, Formatting.Indented);
            ViewData["Recipe"] = json;

            return View();
        }

        // POST: Recipes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [FromBody] Recipe recipe)
        {
            if (id != recipe.RecipeId)
            {
                return NotFound();
            }

            if(await _recipesService.EditRecipe(id, recipe))
            {
                return Ok();
            }

            return NotFound();
;        }

        // GET: Recipes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipe = await _recipesService.GetRecipeById(id);

            if (recipe == null)
            {
                return NotFound();
            }

            return View(recipe);
        }

        // POST: Recipes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            var successful = await _recipesService.DeleteRecipe(id);
            if (successful)
            {
                return RedirectToAction(nameof(Index));
            }
            return View("Error");
            
        }


/*        [HttpPost]
        public IActionResult GenerateAllergenTag([FromBody] int recipeId)
        {
            GenerateTag trial = new GenerateTag(_recipesService);

            string allergens = trial.GetAllergenTag(recipeId);

            tempAllergenTags tempAlTags = JsonConvert.DeserializeObject<tempAllergenTags>(allergens);
            if (tempAlTags.allergens != null)
            {
                Debug.WriteLine(tempAlTags.allergens[0]);
            }

            //Saving the recipe into the DB first before generating the tags
            *//*if (ModelState.IsValid)
            {   //uses Service class to add Recipe
                var successful = await _recipesService.AddRecipe(recipe);
                if (successful)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Email", recipe.UserId);
            return View(recipe);*//*

            
            return RedirectToAction("Create");
        }*/

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult GenerateAllergenTag([FromBody] List<RecipeIngredient> recipeIngredients)
        {
            GenerateTag trial = new GenerateTag(_recipesService);

            string allergens = trial.GetAllergenTag(recipeIngredients);

            List<Tag> tags = new List<Tag>();

            tempAllergenTags tempAlTags = JsonConvert.DeserializeObject<tempAllergenTags>(allergens);
            if (tempAlTags.allergens != null)
            {
                Debug.WriteLine(tempAlTags.allergens[0]);
                for(int i = 0; i < tempAlTags.allergens.Count; i++)
                {
                    tags.Add(new Tag
                    {
                        TagName = tempAlTags.allergens[i],
                        Warning = tempAlTags.allergens[i]
                    });
                }
            }

            string json = JsonConvert.SerializeObject(tags, Formatting.Indented);
            return Json(new { tags = json });
        }

    }
}
