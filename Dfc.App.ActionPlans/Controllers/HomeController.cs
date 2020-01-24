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

        [HttpGet]
        [Route("/head/")]
        public IActionResult Head()
        {
            var vm = new HomePageVm();
            return View(vm);
        }

        [HttpGet]
        [Route("/body/")]
        public IActionResult Body()
        {
            var vm = new HomePageVm();
            return View(vm);
        }

    }
}
