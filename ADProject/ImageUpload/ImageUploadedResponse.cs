using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ADProject.ImageUpload
{
    public class ImageUploadedResponse
    {
        public Data data { get; set; }
        public bool success { get; set; }
        public string status { get; set; }
    }

    public class Data
    {
        public string url { get; set; }
        public string delete_url { get; set; }
    }
}
