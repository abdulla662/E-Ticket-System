using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace E_Ticket_System.Areas.ErrorPage.Controllers
{
    [Area("ErrorPage")]
    [AllowAnonymous]
    public class AccessDeniedController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public  IActionResult Errormodel()
        {
            return View();
        }
    }
}
