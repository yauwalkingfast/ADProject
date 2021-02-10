using ADProject.Models;
using ADProject.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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

        public mGroupController(ADProjContext context, IGroupService groupService)
        {
            _context = context;
            _groupService = groupService;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<Group>> GetGroupById(int id)
        {
            var group = await _groupService.ADGetGroupById(id);

            if (group == null)
            {
                return NotFound();
            }

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
    }
}
