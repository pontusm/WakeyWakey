using Nancy;

namespace WakeyWakey.Web.Modules
{
    public class HomeModule : NancyModule
    {
        public HomeModule()
        {
            Get["/"] = x => Response.AsRedirect("/machines");
        }

    }
}