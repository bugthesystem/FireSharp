using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using FireSharp.Interfaces;

namespace FireSharp.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IFirebaseClient _client;

        public HomeController(IFirebaseClient client)
        {
            _client = client;
        }

        public ActionResult Index()
        {
            return View();
        }

        public async Task<RedirectToRouteResult> CallFirebase()
        {
            await _client.PushAsync("chat/", new
            {
                name = "someone",
                text = "Hello from backend :" + DateTime.Now.ToString("f")
            });

            return RedirectToAction("Index");
        }

        public ActionResult CallFirebaseSync()
        {
            _client.Push("chat/", new
            {
                name = "someone",
                text = "Hello from backend :" + DateTime.Now.ToString("f")
            });

            return Content("hello");
        }

        public ActionResult CallFirebaseSync2()
        {
            _client.PushAsync("chat/", new
            {
                name = "someone",
                text = "Hello from backend :" + DateTime.Now.ToString("f")
            }).Wait();

            return Content("hello");
        }
    }
}