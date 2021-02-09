using ADProject.Models;
using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ADProject.Service
{
    public class GroupService : IGroupService
    {
        private readonly ADProjContext _context;

        public GroupService(ADProjContext context)
        {
            _context = context;
        }

        public async Task<bool> AddGroup(Group group)
        {
            _context.Add(group);
            var saveResult = await _context.SaveChangesAsync();
            return saveResult >= 1;
        }

        public async Task<bool> DeleteGroup(int id)
        {
            var group = await _context.Groups
                .FirstOrDefaultAsync(g => g.GroupId == id);
            _context.Groups.Remove(group);
            var saveResult = await _context.SaveChangesAsync();
            return saveResult >= 1;
        }

        public async Task<bool> EditGroup(int id, Group group)
        {
            try
            {
                var dbGroup = await _context.Groups
                    .Include(g => g.GroupTags)
                    .ThenInclude(gt => gt.Tag)
                    .Include(g => g.RecipeGroups)
                    .ThenInclude(rg => rg.Recipe)
                    .Include(g => g.UsersGroups)
                    .ThenInclude(ug => ug.User)
                    .FirstOrDefaultAsync(g => g.GroupId == id);

                _context.GroupTags.RemoveRange(dbGroup.GroupTags);
                _context.RecipeGroups.RemoveRange(dbGroup.RecipeGroups);
                _context.UsersGroups.RemoveRange(dbGroup.UsersGroups);

                dbGroup.GroupName = group.GroupName;
                dbGroup.GroupPhoto = group.GroupPhoto;
                dbGroup.Description = group.Description;
                dbGroup.DateCreated = group.DateCreated;
                dbGroup.IsPublished = group.IsPublished;
                dbGroup.GroupTags = group.GroupTags;
                dbGroup.RecipeGroups = group.RecipeGroups;
                dbGroup.UsersGroups = group.UsersGroups;

                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
        }

        public async Task<List<Group>> GetAllGroups()
        {
            return await _context.Groups
                .Include(g => g.GroupTags)
                .ThenInclude(gt => gt.Tag)
                .Include(g => g.RecipeGroups)
                .ThenInclude(rg => rg.Recipe)
                .Include(g => g.UsersGroups)
                .ThenInclude(ug => ug.User)
                .ToListAsync();
        }

        public async Task<Group> GetGroupById(int? id)
        {
            return await _context.Groups
                .Include(g => g.GroupTags)
                .ThenInclude(gt => gt.Tag)
                .Include(g => g.RecipeGroups)
                .ThenInclude(rg => rg.Recipe)
                .Include(g => g.UsersGroups)
                .ThenInclude(ug => ug.User)
                .FirstOrDefaultAsync(g => g.GroupId == id);
        }

        public async Task<Group> ADGetGroupById(int? id)
        {
            Group group =  await _context.Groups
                .Include(g => g.GroupTags)
                .ThenInclude(gt => gt.Tag)
                .Include(g => g.RecipeGroups)
                .ThenInclude(rg => rg.Recipe)
                .ThenInclude(r => r.User)
                .FirstOrDefaultAsync(g => g.GroupId == id);

            foreach (RecipeGroup r in group.RecipeGroups)
            {
                User n = new User
                {
                    UserId = r.Recipe.User.UserId,
                    Username = r.Recipe.User.Username
                };

                r.Recipe.User = n;

            }

            return group;
        }

        public async Task<List<Group>> GetAllGroupsSearch(string search)
        {
            List<Group> gList = await _context.Groups
                .Include(r => r.GroupTags)
                .ThenInclude(rtag => rtag.Tag)
                .Where(r => r.GroupName.Contains(search)
                            || r.Description.Contains(search)
                            || r.GroupTags.Any(y => y.Tag.TagName.Contains(search)))
                .ToListAsync();

            /*foreach (Recipe r in rList)
            {
                User n = new User
                {
                    UserId = r.User.UserId,
                    Username = r.User.Username
                };

                r.User = n;
            }*/

            return gList;
        }
    }
}
