using System;
using System.Configuration;
using System.Globalization;
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
            var docsFolderPath = ResolveDocsFolderPath();
            var filePath = ResolveFilePath(docsFolderPath, path);

            if (filePath == null)
                return HttpNotFound();

            Response.AddHeader("X-File-Path", filePath);

            var fileContent = IOFile.ReadAllText(filePath);
            ViewBag.PageContent = new HtmlString(new Markdown().Transform(fileContent));

            var heading1 = Heading1Regex.Match(fileContent);
            ViewBag.Title = heading1.Success ? heading1.Value : "";

            var commitId = ConfigurationManager.AppSettings["appharbor.commit_id"];
            ViewBag.GitHubEditLink = ResolveGitHubEditLink(docsFolderPath, filePath, commitId);

            return View();
        }

        string ResolveFilePath(string docsFolderPath, string path)
        {
            path = path ?? string.Empty;
            path = path.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);

            var contentPath = Path.Combine(docsFolderPath, path);
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

        string ResolveDocsFolderPath()
        {
            var appPath = Request.PhysicalApplicationPath;
            if (appPath == null)
                throw new InvalidOperationException("The app is being hosted outside of an ASP.NET context.");
            var docsFolderPath = Path.Combine(appPath, @"Docs\");
            return docsFolderPath;
        }

        static string ResolveGitHubEditLink(string docsFolderPath, string filePath, string commitId)
        {
            var docs = new Uri(docsFolderPath);
            var file = new Uri(filePath);
            var relativeFileUri = docs.MakeRelativeUri(file);

            return string.Format("https://github.com/tathamoddie/nfwd/edit/{0}/Web/Docs/{1}", commitId, relativeFileUri.OriginalString);
        }
    }
}
