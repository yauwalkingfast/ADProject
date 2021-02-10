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
            group.UsersGroups = await this.CheckUsernameExist(group.UsersGroups.ToList());
            group.GroupTags = await this.CheckTagsDatabase(group.GroupTags.ToList());
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

                if(group.GroupPhoto != "")

                {
                    dbGroup.GroupPhoto = group.GroupPhoto;
                }

                dbGroup.Description = group.Description;
                dbGroup.IsPublished = group.IsPublished;
                dbGroup.RecipeGroups = group.RecipeGroups;

                dbGroup.GroupTags = await this.CheckTagsDatabase(group.GroupTags);
                dbGroup.UsersGroups = await this.CheckUsernameExist(group.UsersGroups);

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
                ApplicationUser n = new ApplicationUser
                {
                    UserName = r.Recipe.User.UserName
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
        // Check if the username exist in database
        private async Task<List<UsersGroup>> CheckUsernameExist(List<UsersGroup> usersGroup)
        {
            List<UsersGroup> foundUsers = new List<UsersGroup>();

            for(int i = 0; i < usersGroup.Count; i++)
            {
                if(usersGroup[i].User.UserName != null)
                {
                    var existUser = await _context.Users.FirstOrDefaultAsync(u => u.UserName.ToLower() == usersGroup[i].User.UserName.ToLower().Trim());
                    if (existUser != null)
                    {
                        foundUsers.Add(new UsersGroup
                        {
                            User = existUser,
                            UserId = existUser.UserId,
                            IsMod = false
                        });
                    }
                }
            }

            return foundUsers;
        }

        // Check if the tag exist in database
        private async Task<List<GroupTag>> CheckTagsDatabase(List<GroupTag> groupTags)
        {
            for (int i = 0; i < groupTags.Count(); i++)
            {
                if (groupTags[i].Tag.TagName != null)
                {
                    var existTag = await _context.Tags.FirstOrDefaultAsync(t => t.TagName.ToLower() == groupTags[i].Tag.TagName.ToLower().Trim());
                    if (existTag != null)
                    {
                        groupTags[i].TagId = existTag.TagId;
                        groupTags[i].Tag = existTag;
                    }
                    else
                    {
                        groupTags[i].Tag.Warning = groupTags[i].Tag.TagName;
                    }
                }
            }

            return groupTags.FindAll(gt => gt.Tag.TagName != null);
        }

        public async Task<bool> IsGroupAdmin(int? groupId, string username)
        {
            if(groupId == null)
            {
                return false;
            }

            var group = await _context.Groups
                .Include(g => g.UsersGroups)
                .ThenInclude(ug => ug.User)
                .FirstOrDefaultAsync(g => g.GroupId == groupId);

            if(group == null)
            {
                return false;
            }

            foreach(var ug in group.UsersGroups)
            {
                if (ug.User.UserName.Equals(username) && ug.User.IsAdmin == true)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
