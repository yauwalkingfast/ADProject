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

        // POST: Groups/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GroupId,GroupName,GroupPicture,Description,DateCreated,IsPublished")] Group group)
        {
            if (ModelState.IsValid)
            {
                var groupPicture = group.GroupPicture;
                var groupPhoto = UploadPicture(groupPicture);
                if(groupPhoto.Equals("error"))
                {
                    return View(group);
                }
                group.GroupPhoto = groupPhoto;
                await _groupService.AddGroup(group);
                return RedirectToAction(nameof(Index));
            }
            return View(group);
        }

        // It might be better to put this into a service
        private string UploadPicture(IFormFile file)
        {
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
        public async Task<IActionResult> Edit(int id, [Bind("GroupId,GroupName,GroupPicture,Description,DateCreated,IsPublished")] Group group)
        {
            if (id != group.GroupId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var groupPicture = group.GroupPicture;
                var groupPhoto = UploadPicture(groupPicture);
                if (groupPhoto.Equals("error"))
                {
                    return View(group);
                }
                group.GroupPhoto = groupPhoto;
                if (await _groupService.EditGroup(id, group))
                {
                    return RedirectToAction(nameof(Index));
                }
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
