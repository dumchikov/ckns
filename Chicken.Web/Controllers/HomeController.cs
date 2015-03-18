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
            _service.AddNewPosts("ddfb4386c9f7f511f299ff67b2e5990819f175896894670d67aae204dd495f2ec96a8ac589dadb3790043");
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetChickens(int skip = 0, int take = 10)
        {
            var model = _service
                .GetExisingChickens(skip, take)
                .ToList()
                .Select(ChickenListViewModel.Map);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetChickenDetails(int id)
        {
            var chicken = _service.GetChicken(id);
            var model = ChickenViewModel.Map(chicken);
            return Json(model, JsonRequestBehavior.AllowGet);
        }
    }
}
