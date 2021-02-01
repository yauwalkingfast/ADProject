using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ADProject.Models;
using ADProject.Service;

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
            var aDProjContext = _context.Recipes
                .Include(r => r.User)
                .Include(r => r.RecipeSteps)
                .Include(r => r.RecipeIngredients);
            return View(await aDProjContext.ToListAsync());
        }

        // GET: Recipes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipe = await _context.Recipes
                .Include(r => r.User)
                .Include(r => r.RecipeSteps)
                .Include(r => r.RecipeIngredients)
                .FirstOrDefaultAsync(m => m.RecipeId == id);
            if (recipe == null)
            {
                return NotFound();
            }

            return View(recipe);
        }

        //GET: Recipes/Create
        public IActionResult Create()
        {
//           ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Email");
            ViewData["UserId"] = _context.Users.FirstOrDefault().UserId; 
            ViewData["Recipe"] = new Recipe();
            return View();
        }

        // POST: Recipes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        /*        [HttpPost]
                [ValidateAntiForgeryToken]
                public async Task<IActionResult> Create([Bind("RecipeId,Title,Description,UserId,DateCreated,DurationInMins,Calories,ServingSize,IsPublished,MainMediaUrl")] Recipe recipe)
                {
                    if (ModelState.IsValid)
                    {   //uses Service class to add Recipe
                        var successful = await _recipesService.AddRecipe(recipe);
                        if (successful)
                        {
                            return RedirectToAction(nameof(Index));
                        }
                    }
                    ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Email", recipe.UserId);
                    return View(recipe);
                }*/

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
                // Not redirecting as expected
                return RedirectToAction(nameof(Index));
            }

            ViewData["UserId"] = recipe.UserId;
            return View();
        }


        // GET: Recipes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipe = await _context.Recipes.FindAsync(id);
            if (recipe == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Email", recipe.UserId);
            return View(recipe);
        }

        // POST: Recipes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RecipeId,Title,Description,UserId,DateCreated,DurationInMins,Calories,ServingSize,IsPublished,MainMediaUrl")] Recipe recipe)
        {
            if (id != recipe.RecipeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(recipe);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RecipeExists(recipe.RecipeId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Email", recipe.UserId);
            return View(recipe);
        }

        // GET: Recipes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipe = await _context.Recipes
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.RecipeId == id);
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
            var recipe = await _context.Recipes.FindAsync(id);
            _context.Recipes.Remove(recipe);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RecipeExists(int id)
        {
            return _context.Recipes.Any(e => e.RecipeId == id);
        }
    }
}
