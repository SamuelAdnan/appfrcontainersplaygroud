using Azure.Core;
using container.backendwebapi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using System.Net;
using System.Reflection.Metadata;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace container.backendwebapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {

        // POST api/<HomeController>
        [HttpPost]
        public async Task<IActionResult> Post(BlobDataModel file)
        {
            var containerClient = HelpersBlob.GetBlobContainerClient();

            var blobClient = containerClient.GetBlobClient(file.filename);
            try
            {


                var OrgimgBytes = Convert.FromBase64String(file.filecontents);

                //convert bytes to image and back to bytes for blob
                Image image = null;

                using (var ms = new MemoryStream(OrgimgBytes))
                {
                    image = Image.FromStream(ms);
                }

                var imgbytes = HelpersBlob.GetImageBytes(image, System.Drawing.Imaging.ImageFormat.Png);
                var imgStream = new MemoryStream(imgbytes);
                var blobresponse = await blobClient.UploadAsync(imgStream);
                //var contentdata = await blobClient.DownloadContentAsync();


                //blobContent = await blobClient.OpenReadAsync();


            }
            catch (Exception)
            {

            }

            return Ok(blobClient.Uri.AbsoluteUri);
        }
    }

}
