using System.Web.Mvc;
using Chicken.Services;
using System.Linq;
using Chicken.Web.Models;

namespace Chicken.Web.Controllers
{
    public class AdminController : Controller
    {
        private readonly ChickenService _chickenService;

        public AdminController(ChickenService chickenService)
        {
            _chickenService = chickenService;
        }

        public ActionResult Index(int page = 1)
        {
            const int take = 10;
            var skip = page == 1 ? 0 : page*take;
            var posts = _chickenService.GetPosts(skip, take).ToList();
            var model = posts.Select(DetailsViewModel.Map);
            return View(model);
        }

        public ActionResult RemoveText()
        {
            const string text = @"**Информацию присылают участники, администрация группы ответственности не несет! Мнение автора и администрации может не совпадать! Истории вымышленные, любое сходство чисто случайно, в случае совпадения писать администратору сообщества 

ЗА ССЫЛКИ И ОСКОРБЛЕНИЯ БАН!!!!ОБЩАЙТЕСЬ ВЕЖЛИВО СУДАРИ

";

            _chickenService.RemoveTextFromAllPosts(text);
            return Content("ok");
        }
    }
}
