using Dfc.App.ActionPlans.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Dfc.App.ActionPlans.Controllers
{
    public class HomeController : BaseController
    {
        public IActionResult Index()
        {
            var vm = new HomePageVm();
            return View(vm);
        }
    }
}
