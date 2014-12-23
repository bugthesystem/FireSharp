using System;
using System.Web.Mvc;
using FireSharp.Interfaces;

namespace FireSharp.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IFirebaseClient _firebaseClient;

        public HomeController(IFirebaseClient firebaseClient)
        {
            _firebaseClient = firebaseClient;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CallFirebase()
        {
            _firebaseClient.Push("chat/", new
            {
                name = "someone",
                text = "Hello from backend :" + DateTime.Now.ToString("f")
            });

            return RedirectToAction("Index");
        }
    }
}