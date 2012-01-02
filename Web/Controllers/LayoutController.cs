using System.Configuration;
using System.Web.Mvc;

namespace Web.Controllers
{
    public class LayoutController : Controller
    {
        [ChildActionOnly]
        [OutputCache(Duration = 3600)]
        public ActionResult Footer()
        {
            var commitId = ConfigurationManager.AppSettings["appharbor.commit_id"];
            if (commitId.Length > 12)
                commitId = commitId.Substring(0, 12);
            ViewBag.CommitId = commitId;

            return View();
        }
    }
}
