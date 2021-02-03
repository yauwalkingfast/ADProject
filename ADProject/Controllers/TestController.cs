using ADProject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ADProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        public Recipe recipe1 = new Recipe();
        public Recipe recipe2 = new Recipe();
        public List<Recipe> list;

        public TestController()
        {
            recipe1.Title = "Cake";
            recipe1.Description = "Egg";
            recipe2.Title = "Mile Tea";
            recipe2.Description = "Milk";
            list = new List<Recipe>();
            list.Add(recipe1);
            list.Add(recipe2);
        }


        /*[HttpGet]
        //[Route("get")]
        public ActionResult<List<Recipe>> GetAllRecipe()
        {
            return list;
        }*/

        [HttpGet]
        [Route("{id}")]
        public ActionResult<Recipe> GetRecipe(int id)
        {
            return list[id];
        }

        [HttpPost]
        //[Route("post")]
        public ActionResult<Recipe> CreateRecipe([FromBody] Recipe recipe)
        {
            list.Add(recipe);

            return recipe;
        }

        [HttpDelete]
        [Route("deleterecipe/{id}")]
        public void DeleteRecipe(int id)
        {
            list.RemoveAt(id);
            return;
        }
    }
}
