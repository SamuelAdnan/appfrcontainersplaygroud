using container.frontend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Diagnostics;

namespace container.frontend.Controllers
{
    public class HomeController : Controller
    {
        List<FilesSelectedModel> files = new List<FilesSelectedModel>();
        private readonly ILogger<HomeController> _logger;
        private readonly IMemoryCache _memoryCache;

        public HomeController(ILogger<HomeController> logger,
            IMemoryCache memoryCache)
        {
            _logger = logger;
            _memoryCache = memoryCache;

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
            foreach (var file in myfiles)
            {
                model = new FilesSelectedModel();
                model.filename = file.FileName;
                model.mimetype = file.ContentType;
                files.Add(model);

                _memoryCache.Set("allfiles", files);
            }

            //return View( files );
            return PartialView("SelectedFilesView", files);


        }
    }
}
