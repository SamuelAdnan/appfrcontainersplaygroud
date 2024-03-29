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
        private readonly IConfiguration configuration;

        public HomeController(IConfiguration Configuration)
        {
            configuration = Configuration;
        }
        // POST api/<HomeController>
        [HttpPost]
        public async Task<IActionResult> Post(BlobDataModel file)
        {

            string blobconnectionstring = configuration["blobconnectionstring"];
            string containerName = configuration["containerName"];
            var containerClient = HelpersBlob.GetBlobContainerClient(blobconnectionstring, containerName);

            var blobClient = containerClient.GetBlobClient(file.filename);
            try
            {


                var OrgimgBytes = Convert.FromBase64String(file.filecontents);

                //convert bytes to image and back to bytes for blob
                //Image image = null;

                MemoryStream ms = new MemoryStream(OrgimgBytes, 0, OrgimgBytes.Length);
                // Convert byte[] to Image
                ms.Write(OrgimgBytes, 0, OrgimgBytes.Length);

                ms.Flush();
                ms.Position = 0;
                System.Drawing.Image image = System.Drawing.Image.FromStream(ms, false);


                string ext = Path.GetExtension(file.filename);

                System.Drawing.Imaging.ImageFormat imageFormat = System.Drawing.Imaging.ImageFormat.Png;
                if (ext.Contains(".png"))
                {
                    imageFormat = System.Drawing.Imaging.ImageFormat.Png;
                }

                if (ext.Contains(".jpg"))
                {
                    imageFormat = System.Drawing.Imaging.ImageFormat.Jpeg;
                }

                if (ext.Contains(".bmp"))
                {
                    imageFormat = System.Drawing.Imaging.ImageFormat.Bmp;
                }


                if (ext.Contains(".gif"))
                {
                    imageFormat = System.Drawing.Imaging.ImageFormat.Gif;
                }


                if (ext.Contains(".tiff"))
                {
                    imageFormat = System.Drawing.Imaging.ImageFormat.Tiff;
                }

                //using (var ms = new MemoryStream(OrgimgBytes))
                //{
                //    image = Image.FromStream(ms);
                //}



                var imgbytes = HelpersBlob.GetImageBytes(image, imageFormat);
                var imgStream = new MemoryStream(imgbytes);
                var blobresponse = await blobClient.UploadAsync(imgStream);



            }
            catch (Exception)
            {

            }

            return Ok(blobClient.Uri.AbsoluteUri);
        }
    }

}
