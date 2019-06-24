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
        public ActionResult Index(HttpPostedFileBase one)
        {
            if (one != null && one.ContentLength > 0)
            {
                var fileName = Path.GetFileName(one.FileName);

                var path = Path.Combine(Server.MapPath("~/Files"), fileName);

                one.SaveAs(path);
            }

            return RedirectToAction("Index");
        }
    }
}