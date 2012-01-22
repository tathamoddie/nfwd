using System;
using System.IO;
using System.Linq;
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
            ViewBag.PageContent = ProcessContent(fileContent);

            var heading1 = Heading1Regex.Match(fileContent);
            ViewBag.Title = heading1.Success ? heading1.Value : "";

            ViewBag.GitHubEditLink = ResolveGitHubEditLink(docsFolderPath, filePath);

            return View();
        }

        static readonly Regex CodeHighlightRegex = new Regex(@"(?s:(?<=<code>)@@highlight\s(?<highlightOptions>.*?)\n(?<code>.*?)(?=</code>))");
        static readonly Regex SequenceDiagramRegex = new Regex(@"(?s:<pre><code>@@sequence(?<sequenceContent>.*)</code></pre>)");
        static HtmlString ProcessContent(string fileContent)
        {
            var content = new Markdown().Transform(fileContent);

            content = CodeHighlightRegex.Replace(content, match =>
            {
                var highlightOptions = match.Groups["highlightOptions"].Value;
                var linesToHighlight = highlightOptions.Split(',').Select(int.Parse);
                var codeLines = match.Groups["code"].Value.Split('\n');
                foreach (var lineNumber in linesToHighlight)
                {
                    var lineIndex = lineNumber - 1;
                    if (lineIndex < 0) throw new ArgumentException(string.Format("Tried to highlight a line with index less than 0. The full code block was:\r\n\r\n{0}", match.Value));
                    if (lineIndex > codeLines.Count() - 1) throw new ArgumentException(string.Format("Tried to highlight a line with index {0}, which doesn't exist. The full code block was:\r\n\r\n{1}", codeLines.Count(), match.Value));
                    codeLines[lineIndex] = string.Format("<strong>{0}</strong>", codeLines[lineIndex]);
                }
                return string.Join(Environment.NewLine, codeLines);
            });

            var contentIncludesASequenceDiagram = false;
            content = SequenceDiagramRegex.Replace(content, match =>
            {
                contentIncludesASequenceDiagram = true;
                var sequenceContent = match.Groups["sequenceContent"].Value;
                return string.Format("<div class=\"wsd\">{0}</div>", sequenceContent);
            });
            if (contentIncludesASequenceDiagram)
                content += "\r\n<script type=\"text/javascript\" src=\"http://www.websequencediagrams.com/service.js\" async></script>";

            return new HtmlString(content);
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

        static string ResolveGitHubEditLink(string docsFolderPath, string filePath)
        {
            var docs = new Uri(docsFolderPath);
            var file = new Uri(filePath);
            var relativeFileUri = docs.MakeRelativeUri(file);

            return string.Format("https://github.com/tathamoddie/nfwd/edit/master/Web/Docs/{0}", relativeFileUri.OriginalString);
        }
    }
}
