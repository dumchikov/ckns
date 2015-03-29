using System.Linq;
using System.Web.Mvc;
using Chicken.Services;
using Chicken.Web.Models;

namespace Chicken.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ChickenService _service;

        public HomeController(ChickenService service)
        {
            _service = service;
        }

        public JsonResult Update()
        {
            _service.AddNewPosts("0517e6e8668f8d48caa5d0a1acd00fcd16558a64fab17aa9c0245c2b90a8a1eab0c91de2dc36cdf292d37");
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetChickens(int skip = 0, int take = 50)
        {
            var model = _service
                .GetExisingChickens(skip, take)
                .ToList()
                .Select(ListItemViewModel.Map);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetChickenDetails(int id)
        {
            var chicken = _service.GetChicken(id);
            var model = DetailsViewModel.Map(chicken);
            return Json(model, JsonRequestBehavior.AllowGet);
        }
    }
}
