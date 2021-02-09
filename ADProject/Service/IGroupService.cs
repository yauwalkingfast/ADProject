using ADProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ADProject.Service
{
    public interface IGroupService
    {
        Task<bool> AddGroup(Group group);

        Task<List<Group>> GetAllGroups();

        Task<Group> GetGroupById(int? id);

        Task<bool> EditGroup(int id, Group group);

        Task<bool> DeleteGroup(int id);

        Task<Group> ADGetGroupById(int? id);

        Task<List<Group>> GetAllGroupsSearch(string search);
    }
}
