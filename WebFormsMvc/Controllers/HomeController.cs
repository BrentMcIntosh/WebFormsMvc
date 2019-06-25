using System.IO;

using System.Web;

using System.Web.Mvc;

namespace WebFormsMvc.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(HttpPostedFileBase fileInfo)
        {
            if (fileInfo != null && fileInfo.ContentLength > 0)
            {
                var fileName = Path.GetFileName(fileInfo.FileName);

                var path = Path.Combine(Server.MapPath("~/Files"), fileName);

                fileInfo.SaveAs(path);
            }

            return RedirectToAction("Index");
        }
    }
}