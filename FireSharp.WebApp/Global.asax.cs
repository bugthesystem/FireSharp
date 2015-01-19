using System.Web;

namespace FireSharp.WebApp
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            Bootstrapper.Start();
        }
    }
}