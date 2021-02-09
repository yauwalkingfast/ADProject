using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ADProject.Models;
using ADProject.Service;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace ADProject.Controllers
{
    public class GroupsController : Controller
    {
        private readonly ADProjContext _context;
        private readonly IGroupService _groupService;

        public GroupsController(ADProjContext context, IGroupService groupService)
        {
            _context = context;
            _groupService = groupService;
        }

        // GET: Groups
        public async Task<IActionResult> Index()
        {
            return View(await _groupService.GetAllGroups());
        }

        // GET: Groups/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var group = await _groupService.GetGroupById(id);

            if (group == null)
            {
                return NotFound();
            }

            return View(group);
        }

        // GET: Groups/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddGroupTag([Bind("GroupTags")] Group group)
        {
            group.GroupTags.Add(new GroupTag());
            return PartialView("AddGroupTags", group);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveGroupTag(Group group)
        {
            group.GroupTags.RemoveAt(group.GroupTags.Count - 1);
            return PartialView("AddGroupTags", group);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddUser([Bind("UsersGroups")] Group group)
        {
            group.UsersGroups.Add(new UsersGroup());
            return PartialView("AddUsers", group);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveUser(Group group)
        {
            group.UsersGroups.RemoveAt(group.UsersGroups.Count - 1);
            return PartialView("AddUsers", group);
        }

        [HttpPost]
        public async Task<IActionResult> UserAutocomplete()
        {
            var names = await _context.Users.Select(u => u.Username).ToListAsync();
            return Json(names);
        }

        // POST: Groups/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GroupId,GroupName,GroupPicture,Description,DateCreated,IsPublished,GroupTags,UsersGroups")] Group group)
        {
            if (group.GroupName.Trim() == "" && !ModelState.IsValid)
            {
                return View(group);
            }

            DateTime now = DateTime.Now;
            group.DateCreated = now;

            var groupPicture = group.GroupPicture;
            var groupPhoto = UploadPicture(groupPicture);
            if (groupPhoto.Equals("error"))
            {
                return View(group);
            } 
            else if (groupPhoto.Equals("notset"))
            {
                group.GroupPhoto = groupPhoto;
            }

            await _groupService.AddGroup(group);
            return RedirectToAction(nameof(Index));
        }

        // It might be better to put this into a service
        private string UploadPicture(IFormFile file)
        {
            if (file == null)
            {
                return "notset";
            }

            try
            {
                string fileName = Guid.NewGuid().ToString() + ".jpg";
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileName);
                using (Stream stream = new FileStream(path, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                string imageUrl = "images/" + fileName;
                return imageUrl;
            } 
            catch
            {
                return "error";
            }
        }

        // GET: Groups/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var group = await _groupService.GetGroupById(id);
            if (group == null)
            {
                return NotFound();
            }
            return View(group);
        }

        // POST: Groups/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        /*        [HttpPost]
                [ValidateAntiForgeryToken]
                public async Task<IActionResult> Edit(int id, [Bind("GroupId,GroupName,GroupPicture,Description,DateCreated,IsPublished")] Group group)
                {
                    if (id != group.GroupId)
                    {
                        return NotFound();
                    }

                    if (ModelState.IsValid)
                    {
                        try
                        {
                            _context.Update(group);
                            await _context.SaveChangesAsync();
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                            if (!GroupExists(group.GroupId))
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
                    return View(group);
                }*/

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("GroupId,GroupName,GroupPicture,Description,DateCreated,IsPublished, GroupTags, UsersGroups")] Group group)
        {
            if (id != group.GroupId)
            {
                return NotFound();
            }

            if(group.GroupName.Trim() == "" && !ModelState.IsValid)
            {
                return View(group);
            }

            var groupPicture = group.GroupPicture;
            var groupPhoto = UploadPicture(groupPicture);
            if (groupPhoto.Equals("error"))
            {
                return View(group);
            }
            else if (groupPhoto.Equals("notset")) 
            {
                group.GroupPhoto = groupPhoto;
            }

            if (await _groupService.EditGroup(id, group))
            {
                return RedirectToAction(nameof(Index));
            }

            return View(group);
        }

        // GET: Groups/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var group = await _groupService.GetGroupById(id);
            if (group == null)
            {
                return NotFound();
            }

            return View(@group);
        }

        // POST: Groups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var successful = await _groupService.DeleteGroup(id);
            if (successful)
            {
                return RedirectToAction(nameof(Index));
            }
            return View("Error");
        }

        private bool GroupExists(int id)
        {
            return _context.Groups.Any(e => e.GroupId == id);
        }
    }
}
