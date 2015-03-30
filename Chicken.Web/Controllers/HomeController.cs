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
            _service.AddNewPosts();
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPosts(int skip = 0, int take = 50)
        {
            var posts = _service.GetPosts(skip, take).ToList();
            var model = posts.Select(ListItemViewModel.Map);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDetails(int id)
        {
            var post = _service.GetPost(id);
            var model = DetailsViewModel.Map(post);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetComments(int id)
        {
            var comments = _service.GetComments(id).ToList();
            var model = comments.Select(CommentViewModel.Map);
            return Json(model, JsonRequestBehavior.AllowGet);
        }
    }
}
