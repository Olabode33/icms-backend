using Abp.Auditing;
using Microsoft.AspNetCore.Mvc;

namespace ICMSDemo.Web.Controllers
{
    public class HomeController : ICMSDemoControllerBase
    {
        [DisableAuditing]
        public IActionResult Index()
        {
            return RedirectToAction("Index", "Ui");
        }
    }
}
