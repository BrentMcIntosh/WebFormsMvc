using System.Web.Mvc;
using WebFormsMvc.Models;

namespace WebFormsMvc.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View(new Test());
        }
    }
}