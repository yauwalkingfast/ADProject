using ADProject.Models;
using ADProject.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ADProject.Controllers
{
    public class UserProfileController : Controller
    {
        private readonly IUserService _userService;
        private readonly IRecipeService _recipeService;

        public UserProfileController(IUserService userService, IRecipeService recipeService)
        {
            _userService = userService;
            _recipeService = recipeService;
        }

        [Authorize]
        public async Task<IActionResult> Index(int? pageNumber)
        {
            var user = await _userService.GetUserByUsername(User.Identity.Name);
            ViewData["User"] = user;

            int pageSize = 9;
            var recipeList = await _recipeService.GetAllRecipesByUserIdQueryable(user.Id);
            PaginatedList<Recipe> paginatedList = await PaginatedList<Recipe>.CreateAsync(recipeList, pageNumber ?? 1, pageSize);
            ViewData["paginatedList"] = paginatedList;

            return View();
        }

        [Authorize]
        public async Task<IActionResult> MyGroups()
        {
            ViewData["User"] = await _userService.GetUserByUsername(User.Identity.Name);
            return View();
        }
    }
}
