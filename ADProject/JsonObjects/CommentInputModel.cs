using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ADProject.JsonObjects
{
    public class CommentInputModel
    {
        public int recipeId { get; set; }
        public string gobackurl { get; set; }
        public string comment { get; set; }
    }
}
