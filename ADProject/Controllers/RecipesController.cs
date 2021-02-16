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

        public async Task<IActionResult> Index(int? pageNumber, string search)
        {

            ViewData["search"] = search;
            int pageSize = 3;
            var recipeList = await _recipesService.GetAllRecipesQueryable();

            if (!String.IsNullOrEmpty(search))
            {
                recipeList = await _recipesService.GetAllRecipeSearchQueryable(search);
            }

            PaginatedList<Recipe> paginatedList = await PaginatedList<Recipe>.CreateAsync(recipeList, pageNumber ?? 1, pageSize);
            ViewData["paginatedList"] = paginatedList;

            return View();
        }

        // GET: Recipes/Details/5
        public async Task<IActionResult> Details(int? id, string gobackurl)
        {
            if (gobackurl.Contains("Groups"))
            {
                var urls = gobackurl.Split("/");
                ViewData["Controller"] = "Groups";
                ViewData["Action"] = "Details";
                ViewData["GoBackId"] = urls[2];
            }
            else if (gobackurl.Contains("UserProfile"))
            {
                var urls = gobackurl.Split("/");
                ViewData["Controller"] = "UserProfile";
                ViewData["Action"] = "Index";
                ViewData["GoBackId"] = urls[1];
            } 
            else
            {
                ViewData["Controller"] = "Recipes";
                ViewData["Action"] = "Index";
                ViewData["GoBackId"] = "";
            }

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
        public IActionResult Create(string gobackurl)
        {
            if (gobackurl.Contains("Groups"))
            {
                var urls = gobackurl.Split("/");
                ViewData["Controller"] = "Groups";
                ViewData["Action"] = "Details";
                ViewData["GoBackId"] = urls[2];
            }
            else if (gobackurl.Contains("UserProfile"))
            {
                var urls = gobackurl.Split("/");
                ViewData["Controller"] = "UserProfile";
                ViewData["Action"] = "Index";
                ViewData["GoBackId"] = urls[1];
            }
            else
            {
                ViewData["Controller"] = "Recipes";
                ViewData["Action"] = "Index";
                ViewData["GoBackId"] = "";
            }

            ViewData["gobackurl"] = gobackurl;
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
        public async Task<IActionResult> Create([FromBody] Recipe recipe, string gobackurl)
        {
            DateTime now = DateTime.Now;
            recipe.DateCreated = now;

            if(await _recipesService.AddRecipe(recipe))
            {
                return Ok(new { id = recipe.RecipeId, gobackurl = gobackurl });
            }

            ViewData["UserId"] = recipe.UserId;
            return BadRequest();
        }

        // GET: Recipes/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id, string gobackurl)
        {
            if (gobackurl.Contains("Groups"))
            {
                var urls = gobackurl.Split("/");
                ViewData["Controller"] = "Groups";
                ViewData["Action"] = "Details";
                ViewData["GoBackId"] = urls[2];
            }
            else if (gobackurl.Contains("UserProfile"))
            {
                var urls = gobackurl.Split("/");
                ViewData["Controller"] = "UserProfile";
                ViewData["Action"] = "Index";
                ViewData["GoBackId"] = urls[1];
            }
            else
            {
                ViewData["Controller"] = "Recipes";
                ViewData["Action"] = "Index";
                ViewData["GoBackId"] = "";
            }

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
                return RedirectToAction("Details", new { id = id, gobackurl = gobackurl });
            }

            ViewData["UserId"] = _context.Users.FirstOrDefault().UserId;
            string json = JsonConvert.SerializeObject(recipe, Formatting.Indented);
            ViewData["Recipe"] = json;
            ViewData["gobackurl"] = gobackurl;

            return View();
        }

        // POST: Recipes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [FromBody] Recipe recipe, string gobackurl)
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
                return Ok(new { id = recipe.RecipeId, gobackurl = gobackurl });
            }

            return NotFound();
        }

        // GET: Recipes/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id, string gobackurl)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (gobackurl.Contains("Groups"))
            {
                var urls = gobackurl.Split("/");
                ViewData["Controller"] = "Groups";
                ViewData["Action"] = "Details";
                ViewData["GoBackId"] = urls[2];
            }
            else if (gobackurl.Contains("UserProfile"))
            {
                var urls = gobackurl.Split("/");
                ViewData["Controller"] = "UserProfile";
                ViewData["Action"] = "Index";
                ViewData["GoBackId"] = urls[1];
            }
            else
            {
                ViewData["Controller"] = "Recipes";
                ViewData["Action"] = "Index";
                ViewData["GoBackId"] = "";
            }

            var recipe = await _recipesService.GetRecipeById(id);

            if (!User.Identity.Name.Equals(recipe.User.UserName))
            {
                return RedirectToAction("Details", new { id = id, gobackurl = gobackurl });
            }

            if (recipe == null)
            {
                return NotFound();
            }

            ViewData["gobackurl"] = gobackurl;

            return View(recipe);
        }

        // POST: Recipes/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, string gobackurl)
        {
            var recipe = await _recipesService.GetRecipeById(id);

            if (!User.Identity.Name.Equals(recipe.User.UserName))
            {
                return RedirectToAction("Details", new { id = id });
            }

            var successful = await _recipesService.DeleteRecipe(id);
            if(successful && gobackurl.Contains("Groups"))
            {
                var urls = gobackurl.Split("/");
                return RedirectToAction("Details", "Groups", new { id = urls[2] });
            }
            else if(successful && gobackurl.Contains("UserProfile"))
            {
                var urls = gobackurl.Split("/");
                return RedirectToAction("Index", "UserProfile", new { id = urls[1] });
            }
            else if (successful)
            {
                var urls = gobackurl.Split("/");
                return RedirectToAction(nameof(Index), new { search = urls[2] });
            }

            return View("Error");

        }

        [Authorize]
        public async Task<IActionResult> SaveRecipe(int? id, string gobackurl)
        {
            if(id == null)
            {
                return View("Error");
            }

            int recipeId = id.Value;

            if(await _recipesService.SaveRecipe(recipeId, User.Identity.Name))
            {
                if (gobackurl.Contains("Details"))
                {
                    return RedirectToAction("Details", new { id = id, gobackurl = gobackurl });
                }
                else
                {
                    var urls = gobackurl.Split("/");
                    return RedirectToAction(nameof(Index), new { search = urls[2] });
                }
            }

            return RedirectToAction("Index");
        }

        [Authorize]
        public async Task<IActionResult> RemoveRecipe(int? id, string gobackurl)
        {
            if(id == null)
            {
                return View("Error");
            }

            int recipeId = id.Value;

            if(await _recipesService.RemoveRecipe(recipeId, User.Identity.Name))
            {
                if (gobackurl.Contains("SavedRecipes"))
                {
                    return RedirectToAction("SavedRecipes", "UserProfile");
                }
                else if(gobackurl.Contains("Details"))
                {
                    return RedirectToAction("Details", new { id = id, gobackurl = gobackurl });
                }
                else
                {
                    var urls = gobackurl.Split("/");
                    return RedirectToAction(nameof(Index), new { search = urls[2] });
                }
            }

            return View("Error");
        }

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
