using ADProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ADProject.JsonObjects
{
    public class groupUserMultipart
    {
        public groupUserMultipart() { }
        
        public Group group { get; set; }
        public ApplicationUser user { get; set; }

        public string tags { get; set; }
    }
}
