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
        private readonly IGroupService _groupService;

        public UserProfileController(IUserService userService, IRecipeService recipeService, IGroupService groupService)
        {
            _userService = userService;
            _recipeService = recipeService;
            _groupService = groupService;
        }

        [Authorize]
        public async Task<IActionResult> Index(int? pageNumber, string search)
        {
            var user = await _userService.GetUserByUsername(User.Identity.Name);
            ViewData["User"] = user;

            int pageSize = 9;
            var recipeList = await _recipeService.GetAllRecipesByUserIdQueryable(user.Id);
            if (!String.IsNullOrEmpty(search))
            {
                recipeList = await _recipeService.SearchMyRecipeQueryable(search, user.Id);
            }

            PaginatedList<Recipe> paginatedList = await PaginatedList<Recipe>.CreateAsync(recipeList, pageNumber ?? 1, pageSize);
            ViewData["paginatedList"] = paginatedList;

            return View();
        }

        [Authorize]
        public async Task<IActionResult> MyGroups(int? pageNumber, string search)
        {
            var user = await _userService.GetUserByUsername(User.Identity.Name);
            ViewData["User"] = user;

            int pageSize = 9;
            var groupList = await _groupService.GetMyGroups(user.Id);
            if (!String.IsNullOrEmpty(search))
            {
                groupList = await _groupService.GetMyGroupsSearch(user.Id, search);
            }

            PaginatedList<UsersGroup> paginatedList = await PaginatedList<UsersGroup>.CreateAsync(groupList, pageNumber ?? 1, pageSize);
            ViewData["paginatedList"] = paginatedList;

            return View();
        }
    }
}
