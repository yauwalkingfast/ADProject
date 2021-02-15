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


/*        // GET: Comments
        public async Task<IActionResult> Index()
        {
            var aDProjContext = _context.Comments.Include(c => c.Recipe).Include(c => c.User);
            return View(await aDProjContext.ToListAsync());
        }

        // GET: Comments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _context.Comments
                .Include(c => c.Recipe)
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.CommentsId == id);
            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        // GET: Comments/Create
        public IActionResult Create()
        {
            ViewData["RecipeId"] = new SelectList(_context.Recipes, "RecipeId", "Title");
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Email");
            return View();
        }

        // POST: Comments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CommentsId,UserId,RecipeId,Dateposted,Comment1")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(comment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RecipeId"] = new SelectList(_context.Recipes, "RecipeId", "Title", comment.RecipeId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Email", comment.UserId);
            return View(comment);
        }

        // GET: Comments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _context.Comments.FindAsync(id);
            if (comment == null)
            {
                return NotFound();
            }
            ViewData["RecipeId"] = new SelectList(_context.Recipes, "RecipeId", "Title", comment.RecipeId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Email", comment.UserId);
            return View(comment);
        }

        // POST: Comments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CommentsId,UserId,RecipeId,Dateposted,Comment1")] Comment comment)
        {
            if (id != comment.CommentsId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(comment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommentExists(comment.CommentsId))
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
            ViewData["RecipeId"] = new SelectList(_context.Recipes, "RecipeId", "Title", comment.RecipeId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Email", comment.UserId);
            return View(comment);
        }

        // GET: Comments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _context.Comments
                .Include(c => c.Recipe)
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.CommentsId == id);
            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var comment = await _context.Comments.FindAsync(id);
            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }*/

        private bool CommentExists(int id)
        {
            return _context.Comments.Any(e => e.CommentsId == id);
        }
    }
}
