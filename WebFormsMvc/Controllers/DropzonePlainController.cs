
using System.IO;

using System.Web;

using System.Web.Mvc;

namespace WebFormsMvc.Controllers
{
    public class DropzonePlainController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
            {
                var fileName = Path.GetFileName(file.FileName);

                var path = Path.Combine(Server.MapPath("~/Files"), fileName);

                file.SaveAs(path);
            }

            return RedirectToAction("Index");
        }

        public class FileStuff
        {
            public string Name { get; set; }
        }

        [HttpPost]
        public string DeleteFile(FileStuff file)
        {


            return $"okay {file.Name} was removed";
        }
    }
}