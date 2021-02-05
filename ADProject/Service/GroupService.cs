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
            var group = await _context.Groups.FirstOrDefaultAsync(g => g.GroupId == id);
            _context.Groups.Remove(group);
            var saveResult = await _context.SaveChangesAsync();
            return saveResult >= 1;
        }

        public async Task<bool> EditGroup(int id, Group group)
        {
            try
            {
                // This may not work is we have nested foreign keys
                _context.Update(group);
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
            return await _context.Groups.ToListAsync();
        }

        public async Task<Group> GetGroupById(int? id)
        {
            return await _context.Groups
                .FirstOrDefaultAsync(g => g.GroupId == id);
        }
    }
}
