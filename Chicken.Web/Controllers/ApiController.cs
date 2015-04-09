using System.Linq;
using System.Web.Mvc;
using Chicken.Services;
using Chicken.Web.Models;

namespace Chicken.Web.Controllers
{
    public class ApiController : Controller
    {
        private readonly NotificationService _notificationService;

        private readonly ChickenService _chickenService;

        public ApiController(ChickenService service, NotificationService notificationService)
        {
            _chickenService = service;
            _notificationService = notificationService;
        }

        public JsonResult GetPosts(int skip = 0, int take = 50)
        {
            var posts = _chickenService.GetPosts(skip, take).ToList();
            var model = posts.Select(ListItemViewModel.Map);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDetails(int id)
        {
            var post = _chickenService.GetPost(id);
            var model = DetailsViewModel.Map(post);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetComments(int id)
        {
            var comments = _chickenService.GetComments(id).ToList();
            var model = comments.Select(CommentViewModel.Map);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public void RegisterDevice(string id)
        {
            _notificationService.AddDevice(id);
        }
    }
}
