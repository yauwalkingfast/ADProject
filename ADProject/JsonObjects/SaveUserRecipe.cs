using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ADProject.JsonObjects
{
    public class SaveUserRecipe
    {
        public int userId { get; set; }
        public int recipeId { get; set; }

        public SaveUserRecipe() { }

        public SaveUserRecipe(int userId, int recipeId)
        {
            this.userId = userId;
            this.recipeId = recipeId;
        }
    }
}
