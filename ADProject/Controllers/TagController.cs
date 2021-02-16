using ADProject.JsonObjects;
using ADProject.Models;
using ADProject.Service;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ADProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagController : ControllerBase
    {

        private readonly IRecipeService _recipesService;

        public TagController(IRecipeService recipeService)
        {
            _recipesService = recipeService;
        }
        
        // POST api/<TagController>
        [HttpGet("{id}")]
        public string Get(int id)
        {
            string uirWebAPI = "http://localhost:5000/api/allergentags";
            string webResponse = string.Empty;

            Debug.WriteLine(id);

            List<RecipeIngredient> recipeIngredients = _recipesService.FindRecipeStepsByRecipeId(id);
            Debug.WriteLine(recipeIngredients[0]);

            List<string> ingredients = new List<string>();
            foreach (RecipeIngredient ri in recipeIngredients)
            {
                ingredients.Add(ri.Ingredient);
            }

            recipeIngredientsJson toSend = new recipeIngredientsJson();
            toSend.ingredients = ingredients;

            try
            {
                Uri uri = new Uri(uirWebAPI);
                WebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(uri);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";
                using (StreamWriter streamWriter =
                    new StreamWriter(httpWebRequest.GetRequestStream()))
                {

                    streamWriter.Write(JsonConvert.SerializeObject(toSend));
                }
                HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream()))
                {
                    webResponse = streamReader.ReadToEnd();
                    
                }

            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            Debug.WriteLine(webResponse);
            
            tempAllergenTags tempAlTags = JsonConvert.DeserializeObject<tempAllergenTags>(webResponse);
            if (tempAlTags.allergens != null)
            {
                Debug.WriteLine(tempAlTags.allergens[0]);
            }

            return webResponse;
            
        }

        // PUT api/<TagController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<TagController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
