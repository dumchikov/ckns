using System.Collections.Generic;
using System.Dynamic;
using System.Web.Mvc;
using Chicken.Services;
using System.Linq;
using Chicken.Web.Models.Admin;

namespace Chicken.Web.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly ChickenService _chickenService;

        public AdminController(ChickenService chickenService)
        {
            _chickenService = chickenService;
        }

        public ActionResult Index()
        {
            dynamic model = new ExpandoObject();
            model.totalCount = _chickenService.GetPostsCount();
            model.spamCount = _chickenService.GetSpamCount();
            return View(model);
        }

        public JsonResult GetPosts(int skip = 0, int take = 100)
        {
            var posts = _chickenService.GetPosts(skip, take, true).ToList();
            var model = posts.Select(AdminPostViewModel.Map);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Update()
        {
            var posts = _chickenService.AddNewPosts();
            var model = posts.OrderByDescending(x => x.Date).Select(AdminPostViewModel.Map);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Save(IList<AdminPostViewModel> models)
        {
            if (models != null)
            {
                foreach (var model in models)
                {
                    var post = _chickenService.GetPost(model.Id);
                    post.IsSpam = model.IsSpam;
                    post.Text = model.Text;
                    _chickenService.EditPost(post);
                }
            }

            return Json(null, JsonRequestBehavior.AllowGet);
        }
    }
}
