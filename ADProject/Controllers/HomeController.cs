using ADProject.Models;
using ADProject.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ADProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ADProjContext _context;
        private readonly IUserService _userService;
        private readonly IRecipeService _recipeService;
        public HomeController(ADProjContext context, IUserService userService, IRecipeService recipeService)
        {
            _context = context;
            _recipeService = recipeService;
            _userService = userService;
        }

        /*        public async Task<IActionResult> Index()
                {
                    ApplicationUser user = _context.Users.FirstOrDefault();
                    int id = user.Id;
                   // ApplicationUser user = await _userService.GetUserById(id);
                   var recipes= await _recipeService.GetAllRecipesByUserId(id);
                    ViewData ["FirstName"]= user.FirstName;
                    return View(recipes);
                }
                [HttpPost]
                public async Task<IActionResult> Index([FromForm]String search)
                {//i need current user who is logged in for now i have hardcoded the id
                    int id = 1;
                    if (!String.IsNullOrEmpty(search))
                    {
                        return View( await _recipeService.SearchMyRecipe(search, id));
                    }
                    else
                    //return View(await _userService.GetUserById(id));
                    return View(await _recipeService.GetAllRecipesByUserId(id));
                }
               */

        public async Task<IActionResult> Index(int? pageNumber, string search)
        {
            // Hard code user for now
            ApplicationUser user = _context.Users.FirstOrDefault();
            int id = user.Id;

            ViewData["search"] = search;
            int pageSize = 1;
            var recipeList = await _recipeService.GetAllRecipesByUserIdQueryable(id);

            if (!String.IsNullOrEmpty(search))
            {
                recipeList = await _recipeService.SearchMyRecipeQueryable(search, id);
            }

            PaginatedList<Recipe> paginatedList = await PaginatedList<Recipe>.CreateAsync(recipeList, pageNumber ?? 1, pageSize);
            ViewData["paginatedList"] = paginatedList;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
