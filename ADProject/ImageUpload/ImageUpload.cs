using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ADProject.ImageUpload
{
    public abstract class ImageUpload
    {
        public static async Task<string> UploadImage(IFormFile formFile)
        {
            string imgbbUrl = "https://api.imgbb.com/1/upload?key=5f7be5eee86ef4cadd3599c60d5c5306";
            var bytes = await GetBytes(formFile);
            string base64 = Convert.ToBase64String(bytes);
            ImageUploadedResponse imageUploadedResponse = new ImageUploadedResponse();

            using(var httpClient = new HttpClient())
            {
                var toSend = new MultipartFormDataContent();
                toSend.Add(new StringContent(base64), "image");
                using (var response = await httpClient.PostAsync(imgbbUrl, toSend))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    imageUploadedResponse = JsonConvert.DeserializeObject<ImageUploadedResponse>(apiResponse);
                }
            }

            if(imageUploadedResponse.success)
            {
                return imageUploadedResponse.data.url;
            }

            return "";
        }

        public static async Task<byte[]> GetBytes(IFormFile formFile)
        {
            using (var memoryStream = new MemoryStream())
            {
                await formFile.CopyToAsync(memoryStream);
                return memoryStream.ToArray();
            }
        }
    }
}
