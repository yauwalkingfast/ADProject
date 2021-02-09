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
    public class mUserGroupController : ControllerBase
    {

        private readonly ADProjContext _context;
        private readonly IUserService _usersService;

        public mUserGroupController(ADProjContext context, IUserService userService)
        {
            _context = context;
            _usersService = userService;
        }

        

        [HttpGet]
        [Route("UserId/{id}")]
        public async Task<ActionResult<List<UsersGroup>>> GetUserGroupById(int id)
        {
            var usergroup = await _usersService.GetUserGroupByUserId(id);


            if (usergroup == null)
            {
                return NotFound();
            }


            return usergroup;
        }

        [HttpPost]
        //[Route("post")]
        public async Task<ActionResult<UsersGroup>> JoinGroup([FromBody] UsersGroup ug)
        {
            bool joined = await _usersService.JoinGroup(ug);

            if (joined)
            {
                return ug;
            }
            else
            {
                return NotFound();
            }

        }


    }
}
