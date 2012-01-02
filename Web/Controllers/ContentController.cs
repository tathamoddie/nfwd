using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using MarkdownSharp;
using IOFile = System.IO.File;

namespace Web.Controllers
{
    public class ContentController : Controller
    {
        static readonly Regex Heading1Regex = new Regex(@"(?m:(?<=^\#\s).+(?=$))");

        public ActionResult Page(string path)
        {
            var filePath = ResolveFilePath(path);

            if (filePath == null)
                return HttpNotFound();

            Response.AddHeader("X-File-Path", filePath);

            var fileContent = IOFile.ReadAllText(filePath);
            ViewBag.PageContent = new HtmlString(new Markdown().Transform(fileContent));

            var heading1 = Heading1Regex.Match(fileContent);
            ViewBag.Title = heading1.Success ? heading1.Value : "";

            return View();
        }

        string ResolveFilePath(string path)
        {
            var appPath = Request.PhysicalApplicationPath;
            if (appPath == null)
                throw new InvalidOperationException("The app is being hosted outside of an ASP.NET context.");

            var contentPath = Path.Combine(appPath, "Docs", path ?? string.Empty);
            Response.AddHeader("X-Content-Path", contentPath);

            var filePath = contentPath + ".md";
            if (IOFile.Exists(filePath))
                return filePath;

            if (Directory.Exists(contentPath))
            {
                filePath = Path.Combine(contentPath, "Default.md");
                if (IOFile.Exists(filePath))
                    return filePath;
            }

            return null;
        }
    }
}
