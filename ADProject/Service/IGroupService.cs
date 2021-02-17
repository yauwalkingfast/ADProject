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

        Task<IQueryable<Group>> GetAllGroupsQueryable();

        Task<IQueryable<UsersGroup>> GetMyGroups(int id);

        Task<IQueryable<UsersGroup>> GetMyGroupsSearch(int id, string search);

        Task<Group> GetGroupById(int? id);

        Task<bool> EditGroup(int id, Group group);

        Task<bool> DeleteGroup(int id);

        Task<Group> ADGetGroupById(int? id);

        Task<List<Group>> GetAllGroupsSearch(string search);

        Task<IQueryable<Group>> GetAllGroupsSearchQueryable(string search);

        Task<Group> AddGroupAD(Group group);

        List<Group> UserInGroups(int userId);

        List<Group> RecipeInGroups(int recipeId);

        Task<bool> PostRecipes(List<Group> groups, int recipeId);

        Task<bool> IsGroupAdmin(int? groupId, string username);

        Task<bool> IsGroupMember(int? groupId, string username);

        Task<bool> JoinGroupWebVer(int? groupId, string username);

        Task<bool> LeaveGroupWebVer(int? groupId, string username);
        Task<IQueryable<RecipeGroup>> getRecipesGroupByGroupId(int? groupId);
        Task<IQueryable<RecipeGroup>> getRecipesGroupSearchByGroupId(int? groupId, string search);

        Task<bool> IsGroupMemberAD(int groupId, int userId);

        Task<List<Group>> GetAllGroups1();

    }
}
