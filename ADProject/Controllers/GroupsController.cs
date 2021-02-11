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
using Microsoft.AspNetCore.Authorization;

namespace ADProject.Controllers
{
    public class GroupsController : Controller
    {
        private readonly ADProjContext _context;
        private readonly IGroupService _groupService;
        private readonly IUserService _userService;

        public GroupsController(ADProjContext context, IGroupService groupService, IUserService userService)
        {
            _context = context;
            _groupService = groupService;
            _userService = userService;
        }

        // GET: Groups
        public async Task<IActionResult> Index(bool? joingroupfailed)
        {
            if (joingroupfailed == true)
            {
                ViewData["joingroupfailed"] = true;
            }
            else
            {
                ViewData["joingroupfailed"] = false;
            }

            return View(await _groupService.GetAllGroups());
        }

        [Authorize]
        public async Task<IActionResult> Join(int? id)
        {
            if(id == null)
            {
                return View("Error");
            }

            if(await _groupService.JoinGroupWebVer(id, User.Identity.Name))
            {
                return RedirectToAction("Details", new { id = id });
            }

            return RedirectToAction("Index", new { joingroupfailed = true });
        }

        [Authorize]
        public async Task<IActionResult> Leave(int? id)
        {
            if(id == null)
            {
                return View("Error");
            }

            if(await _groupService.LeaveGroupWebVer(id, User.Identity.Name))
            {
                return RedirectToAction("Details", new { id = id });
            }

            return RedirectToAction("Index");
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
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddGroupTag([Bind("GroupTags")] Group group)
        {
            group.GroupTags.Add(new GroupTag());
            return PartialView("AddGroupTags", group);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveGroupTag(Group group)
        {
            group.GroupTags.RemoveAt(group.GroupTags.Count - 1);
            return PartialView("AddGroupTags", group);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddUser([Bind("UsersGroups")] Group group)
        {
            group.UsersGroups.Add(new UsersGroup());
            return PartialView("AddUsers", group);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveUser(Group group)
        {
            group.UsersGroups.RemoveAt(group.UsersGroups.Count - 1);
            return PartialView("AddUsers", group);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UserAutocomplete()
        {
            var names = await _context.Users.Where(u => u.UserName != User.Identity.Name).Select(u => u.UserName).ToListAsync();
            return Json(names);
        }

        // POST: Groups/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
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
            var groupPhoto = await UploadPicture(groupPicture);
            //group.GroupPhoto = groupPhoto;
            if (groupPhoto.Equals("error"))
            {
                return View(group);
            } 
            else if (!groupPhoto.Equals("notset"))
            {
                group.GroupPhoto = groupPhoto;
            }

            ApplicationUser superUser = await _userService.GetUserByUsername(User.Identity.Name);
            group.UsersGroups.RemoveAll(u => u.User.UserName == User.Identity.Name);
            group.UsersGroups.Add(new UsersGroup
            {
                UserId = superUser.Id,
                IsMod = true,
                User = superUser
            });
            
            await _groupService.AddGroup(group);
            return RedirectToAction("Details", new { id = group.GroupId });
        }

        // It might be better to put this into a service
        /*        [Authorize]
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
                }*/

        [Authorize]
        private async Task<string> UploadPicture(IFormFile file)
        {
            if (file == null)
            {
                return "notset";
            }

            string imageUrl = await ImageUpload.ImageUpload.UploadImage(file);
            if(imageUrl == "")
            {
                return "error";
            }

            return imageUrl;
        }

        // GET: Groups/Edit/5
        [Authorize]
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

            if(!await _groupService.IsGroupAdmin(id, User.Identity.Name))
            {
                return RedirectToAction("Details", new { id = id });
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
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("GroupId,GroupName,GroupPicture,Description,DateCreated,IsPublished, GroupTags, UsersGroups")] Group group)
        {
            if (id != group.GroupId)
            {
                return NotFound();
            }

            if (!await _groupService.IsGroupAdmin(id, User.Identity.Name))
            {
                return RedirectToAction("Details", new { id = id });
            }

            if (group.GroupName.Trim() == "" && !ModelState.IsValid)
            {
                return View(group);
            }

            var groupPicture = group.GroupPicture;
            var groupPhoto = await UploadPicture(groupPicture);
            if (groupPhoto.Equals("error"))
            {
                return View(group);
            }
            else if (groupPhoto.Equals("notset")) 
            {
                group.GroupPhoto = "";
            } 

            else
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
        [Authorize]
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

            if (!await _groupService.IsGroupAdmin(id, User.Identity.Name))
            {
                return RedirectToAction("Details", new { id = id });
            }

            return View(@group);
        }

        // POST: Groups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!await _groupService.IsGroupAdmin(id, User.Identity.Name))
            {
                return RedirectToAction("Details", new { id = id });
            }

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
