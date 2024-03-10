namespace container.frontend.Models
{
    public class BlobData
    {

        public string filename { get; set; }
        public string blobid { get; set; }
        public string mimetype { get; set; }
    

    }


    public class BlobDataModel
    {

        public string filename { get; set; }
        public string mimetype { get; set; }
        public string filecontents { get; set; }
    }
}
