using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ADProject.Models;
using ADProject.Service;
using Microsoft.AspNetCore.Authorization;
using ADProject.JsonObjects;

namespace ADProject.Controllers
{
    public class CommentsController : Controller
    {
        private readonly ADProjContext _context;
        private readonly ICommentService _commentService;
        private readonly IUserService _userService;

        public CommentsController(ADProjContext context, ICommentService commentService, IUserService userService)
        {
            _context = context;
            _commentService = commentService;
            _userService = userService;
        }

        [Authorize]
        public async Task<IActionResult> AddCommentToRecipe([FromBody] CommentInputModel commentInputModel)
        {
            ApplicationUser user = await _userService.GetUserByUsername(User.Identity.Name);
            if(user == null)
            {
                return BadRequest();
            }

            Comment comment = new Comment{
                User = user,
                UserId = user.Id,
                RecipeId = commentInputModel.recipeId,
                Comment1 = commentInputModel.comment,
                Dateposted = DateTime.Now
            };

            if(await _commentService.AddComment(commentInputModel.recipeId, comment))
            {
                return Ok();
            }

            return BadRequest();
        }
    }
}
