using Azure.Storage.Blobs;
using System.Drawing;

namespace container.backendwebapi
{
    public static class HelpersBlob
    {

        public static BlobClient GetBlobClient(string filename)
        {
            BlobServiceClient blobServiceClient = null;
            string blobconnectionstring = "***";
            string containerName = "***";

            blobServiceClient = new BlobServiceClient(blobconnectionstring);
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);


            BlobClient blobClient = containerClient.GetBlobClient(filename);

            return blobClient;
        }


        public static BlobContainerClient GetBlobContainerClient(string blobconnectionstring, string containerName)
        {
            BlobServiceClient blobServiceClient = null;
          

            blobServiceClient = new BlobServiceClient(blobconnectionstring);
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);



            return containerClient;
        }

        public static byte[] GetImageBytes(System.Drawing.Image image,
        System.Drawing.Imaging.ImageFormat format)
        {
           


           
            using (var ms = new System.IO.MemoryStream())
            {
                image.Save(ms, format);
                return ms.ToArray();
            }
        }
    }
}
