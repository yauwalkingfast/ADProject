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

using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

using Microsoft.AspNetCore.Authorization;

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

        [HttpPost]
        public async Task<IActionResult> Index([FromForm] String search)
        {
           
            if (!String.IsNullOrEmpty(search))
            {
                return View(await _recipesService.GetAllRecipesSearch(search));
            }
            else
                
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

        // GET: Recipes/Create
        [Authorize]
        public IActionResult Create()
        {
            ViewData["UserId"] = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name).UserId;
            ViewData["Recipe"] = new Recipe();
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> GroupNameAutocomplete()
        {
            var groupnames = await _context.Groups.Select(g => g.GroupName).ToListAsync();
            return Json(groupnames);
        }

        // POST: Recipes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromBody] Recipe recipe)
        {
            DateTime now = DateTime.Now;
            recipe.DateCreated = now;

            if(await _recipesService.AddRecipe(recipe));
            {
                return Ok(new { id = recipe.RecipeId });
            }

            ViewData["UserId"] = recipe.UserId;
            return BadRequest();
        }

        // GET: Recipes/Edit/5
        [Authorize]
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

            if(!User.Identity.Name.Equals(recipe.User.UserName))
            {
                return RedirectToAction("Details", new { id = id });
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
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [FromBody] Recipe recipe)
        {
            if (id != recipe.RecipeId)
            {
                return NotFound();
            }

            if (!User.Identity.Name.Equals(recipe.User.UserName))
            {
                return Unauthorized();
            }

            if(await _recipesService.EditRecipe(id, recipe))
            {
                return Ok(new { id = recipe.RecipeId });
            }

            return NotFound();
        }

        // GET: Recipes/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipe = await _recipesService.GetRecipeById(id);

            if (!User.Identity.Name.Equals(recipe.User.UserName))
            {
                return RedirectToAction("Details", new { id = id });
            }

            if (recipe == null)
            {
                return NotFound();
            }

            return View(recipe);
        }

        // POST: Recipes/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var recipe = await _recipesService.GetRecipeById(id);

            if (!User.Identity.Name.Equals(recipe.User.UserName))
            {
                return RedirectToAction("Details", new { id = id });
            }

            var successful = await _recipesService.DeleteRecipe(id);
            if (successful)
            {
                return RedirectToAction(nameof(Index));
            }

            return View("Error");

        }

        [Authorize]
        public async Task<IActionResult> SaveRecipe(int? id)
        {
            if(id == null)
            {
                return View("Error");
            }

            int recipeId = id.Value;

            if(await _recipesService.SaveRecipe(recipeId, User.Identity.Name))
            {
                return RedirectToAction("Details", new { id = id });
            }

            return RedirectToAction("Index");
        }

        [Authorize]
        public async Task<IActionResult> RemoveRecipe(int? id)
        {
            if(id == null)
            {
                return View("Error");
            }

            int recipeId = id.Value;

            if(await _recipesService.RemoveRecipe(recipeId, User.Identity.Name))
            {
                return RedirectToAction("Index");
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
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult GenerateAllergenTag([FromBody] List<RecipeIngredient> recipeIngredients)
        {
            GenerateTag trial = new GenerateTag(_recipesService);

            string allergens = trial.GetAllergenTag(recipeIngredients);

            List<RecipeTag> tags = new List<RecipeTag>();

            tempAllergenTags tempAlTags = JsonConvert.DeserializeObject<tempAllergenTags>(allergens);
            if (tempAlTags.allergens != null)
            {
                Debug.WriteLine(tempAlTags.allergens[0]);
                for (int i = 0; i < tempAlTags.allergens.Count; i++)
                {
                    tags.Add(new RecipeTag
                    {
                        IsAllergenTag = true,
                        Tag = new Tag
                        {
                            TagName = tempAlTags.allergens[i],
                            Warning = tempAlTags.allergens[i]
                        }
                    });
                }
            }

            string json = JsonConvert.SerializeObject(tags, Formatting.Indented);
            return Json(new { tags = json });
        }

        /*        [HttpPost]
                [Authorize]
                public IActionResult FileUpload([FromForm] FileModel file)
                {
                    try
                    {
                        file.FileName = Guid.NewGuid().ToString() + ".jpg";
                        string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", file.FileName);

                        *//*                string path = Path.Combine(Directory.GetCurrentDirectory(), "RecipesImage", file.FileName);
                        *//*
                        using (Stream stream = new FileStream(path, FileMode.Create))
                        {
                            file.FormFile.CopyTo(stream);
                        }

                        string imageUrl = "images/" + file.FileName;

        *//*                string imageUrl = "RecipesImage/" + file.FileName;
        *//*                
                        return Json(new { fileUrl = imageUrl });
                    }
                    catch
                    {
                        return StatusCode(400);
                    }
                }*/

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> FileUpload([FromForm] FileModel file)
        {
            IFormFile toUpload = file.FormFile;
            string imageUrl = await ImageUpload.ImageUpload.UploadImage(toUpload);
            if(imageUrl != "")
            {
                return Ok(new { fileUrl = imageUrl });
            } 
            else
            {
                return StatusCode(400);
            }
        }
    }
}
