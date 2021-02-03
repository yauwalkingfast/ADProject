using ADProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ADProject.JsonObjects
{
    public class recieveIngredientsJson
    {
        public recieveIngredientsJson() { }
        public List<RecipeIngredient> ingredients { get; set; }
    }
}
