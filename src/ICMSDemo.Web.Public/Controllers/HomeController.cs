using Microsoft.AspNetCore.Mvc;
using ICMSDemo.Web.Controllers;

namespace ICMSDemo.Web.Public.Controllers
{
    public class HomeController : ICMSDemoControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}