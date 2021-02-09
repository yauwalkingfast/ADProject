using ADProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ADProject.Service
{
    public class UserService : IUserService
    {
        private readonly ADProjContext _context;

        public UserService(ADProjContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserById(int? id)
        {
            User user = await _context.Users
                .Include(r => r.Recipes)
                .Include(r => r.LikesDislikes)
                .Include(r => r.Comments)
                .FirstOrDefaultAsync(r => r.UserId == id);

            return user;
        }

        public async Task<List<UsersGroup>> GetUserGroupByUserId(int? id)
        {
            return await _context.UsersGroups
                .Include(r => r.User)
                .Include(r => r.Group)
                .Where(r => r.UserId == id)
                .ToListAsync();
        }

        public async Task<List<UsersGroup>> GetUserGroupByGroupId(int? id)
        {
            return await _context.UsersGroups
                .Include(r => r.User)
                .Include(r => r.Group)
                .Where(r => r.GroupId == id)
                .ToListAsync();
        }

        public async Task<List<UserAllergen>> getUserAllergens(int id)
        {
            return await _context.UserAllergens
                .Include(x => x.Tag)
                .Include(x => x.User)
                .Where(x => x.UserId == id)
                .ToListAsync();
        }
    }
}
