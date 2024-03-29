using container.frontend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;

namespace container.frontend.Controllers
{
    public class HomeController : Controller
    {
        List<FilesSelectedModel> files = new List<FilesSelectedModel>();
        private readonly ILogger<HomeController> _logger;
        private readonly IMemoryCache _memoryCache;
        private readonly IConfiguration configuration;
        public HomeController(ILogger<HomeController> logger,
            IMemoryCache memoryCache, IConfiguration Configuration)
        {
            _logger = logger;
            _memoryCache = memoryCache;
            configuration = Configuration;

        }

        public IActionResult Index()
        {
            _memoryCache.TryGetValue("allfiles", out files);
            if (files != null &&
                files.Count > 0)
            {
                return View(files);
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost("UploadFiles")]
        public ActionResult UploadFiles(List<IFormFile> myfiles)
        {
            _memoryCache.Remove("allfiles");
            FilesSelectedModel model = new FilesSelectedModel();
            var ms = new MemoryStream();
            foreach (var file in Request.Form.Files)
            {
                model = new FilesSelectedModel();
                model.filename = file.FileName;
                model.mimetype = file.ContentType;
                file.CopyTo(ms);
                model.filecontents = Convert.ToBase64String(ms.ToArray());
                files.Add(model);
            }

            _memoryCache.Set("allfiles", files);

            var json = JsonConvert.SerializeObject(model);
            return Json(json);


        }

        [HttpGet("SentToblob")]
        public async Task<ActionResult<string>> SentToblob()
        {
            string url = string.Empty;
            if (configuration["ApiUrl"] != null &&
                ( !string.IsNullOrWhiteSpace(configuration["ApiUrl"])))
            {
                url = configuration["ApiUrl"];

                //string url = "https://localhost:44349/api/Home"; // sample url
                string result = string.Empty;
                _memoryCache.TryGetValue("allfiles", out files);

                BlobDataModel blobDataModel = new BlobDataModel();
                foreach (var file in files)
                {
                    blobDataModel = new BlobDataModel();
                    blobDataModel.filecontents = file.filecontents;
                    blobDataModel.filename = file.filename;
                    blobDataModel.mimetype = file.mimetype;
                }

                var payload = JsonConvert.SerializeObject(blobDataModel);
                var content = new StringContent(payload, Encoding.UTF8, "application/json");
                using (HttpClient client = new HttpClient())
                {
                    var response = await client.PostAsync(url, content);
                    result = await response.Content.ReadAsStringAsync();
                }

                return Ok(result);
            }
            else
            {
                return Ok("ApiUrl is empty.");
            }

           
        }

    }
}
