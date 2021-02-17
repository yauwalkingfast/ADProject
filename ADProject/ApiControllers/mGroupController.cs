using ADProject.JsonObjects;
using ADProject.Models;
using ADProject.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ADProject.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class mGroupController : ControllerBase
    {
        private readonly ADProjContext _context;
        private readonly IGroupService _groupService;
        private readonly IUserService _usersService;
        private readonly IRecipeService _recipesService;

        public mGroupController(ADProjContext context, IGroupService groupService, IUserService userService, IRecipeService recipeService)
        {
            _context = context;
            _groupService = groupService;
            _usersService = userService;
            _recipesService = recipeService;
        }

        [HttpPost]
        [Route("getGroup")]
        public async Task<ActionResult<Group>> GetGroupById([FromBody] UsersGroup ug)
        {
            int id = ug.GroupId;
            var group = await _groupService.ADGetGroupById(id);

            if (group == null)
            {
                return NotFound();
            }

            int userId = ug.UserId;
            group.isJoined = await _groupService.IsGroupMemberAD(id, userId);

            return group;
        }

        [HttpGet]
        [Route("search/{search}")]
        public async Task<ActionResult<List<Group>>> SearchGroups(string search)
        {
            List<Group> groupList = await _groupService.GetAllGroupsSearch(search);
            if (groupList == null)
            {
                return NotFound();
            }

            return groupList;
        }

        [HttpGet]
        [Route("recipeGroups/{id}")]
        public async Task<ActionResult<List<Group>>> RecipeNotInGroup(int id)
        {
            Recipe recipe = await _recipesService.GetRecipeById(id);
            List<Group> myGroups = _groupService.UserInGroups(recipe.User.Id);
            List<Group> recipeInGroups = _groupService.RecipeInGroups(id);
            List<Group> toReturn = new List<Group>();

            foreach (Group mg in myGroups)
            {
                if (!recipeInGroups.Any(g => g.GroupId == mg.GroupId))
                {
                    toReturn.Add(mg);
                }
            }
            return toReturn;

        }

        [HttpPost]
        [Route("addRtoG/{id}")]
        public async Task<ActionResult> addRtoG([FromBody] List<Group> groups, int id)
        {
            Debug.Write("Reached");

            bool posted = await _groupService.PostRecipes(groups, id);
            
            if (posted)
            {
                return Ok();
            }

            return BadRequest();
        }


        [HttpPost]
        public async Task<ActionResult<Group>> createGroup([FromBody] groupUserMultipart gu)
        {
            Group group = gu.group;
            ApplicationUser user = gu.user;
            string tags = gu.tags;


            DateTime now = DateTime.Now;
            group.DateCreated = now;
            group.IsPublished = true;

            group = stringToTag(tags, group);

            Group rg = await _groupService.AddGroupAD(group);
            if (rg == null)
            {
                return NotFound();
            }

            //User auto joins group, is automatically a moderator
            UsersGroup n = new UsersGroup
            {
                GroupId = rg.GroupId,
                UserId = user.Id,
                IsMod = true
            };
            await _usersService.JoinGroup(n);

            return rg;
        }

        [HttpGet]
        [Route("getall")]
        public async Task<ActionResult<List<Group>>> getAllGroups()
        {
            List<Group> groupList = await _groupService.GetAllGroups1();
            if (groupList == null)
            {
                return NotFound();
            }
            return groupList;
        }

        public Group stringToTag(string s, Group g)
        {
            string[] toArray = s.Split('#');
            List<GroupTag> gt = new List<GroupTag>();

            for (int i = 0; i < toArray.Length; i++)
            {
                if (!String.IsNullOrEmpty(toArray[i].Trim()))
                {
                    Tag t = new Tag { TagName = toArray[i].Trim() };
                    GroupTag gtt = new GroupTag
                    {
                        Tag = t,
                        Group = g
                    };
                    gt.Add(gtt);

                }
            }

            g.GroupTags = gt;
            return g;
        }
    }
}
