using System;
using System.IO;
using System.Web.Mvc;
using IOFile = System.IO.File;

namespace Web.Controllers
{
    public class ContentController : Controller
    {
        public ActionResult Render(string path)
        {
            var appPath = Request.PhysicalApplicationPath;
            if (appPath == null)
                throw new InvalidOperationException("The app is being hosted outside of an ASP.NET context.");

            var contentPath = Path.Combine(appPath, path ?? string.Empty);

            var filePath = contentPath + ".md";
            if (IOFile.Exists(filePath))
                return Content(filePath);

            if (Directory.Exists(contentPath))
            {
                filePath = Path.Combine(contentPath, "Default.md");
                if (IOFile.Exists(filePath))
                    return Content(filePath);
            }

            return HttpNotFound();
        }
    }
}
