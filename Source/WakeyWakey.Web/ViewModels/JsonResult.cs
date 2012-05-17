namespace WakeyWakey.Web.ViewModels
{
    public class JsonResult
    {
        public bool ok { get; set; }

        public string message { get; set; }

        public static JsonResult OK()
        {
            return new JsonResult { ok = true, message = string.Empty };
        }

        public static JsonResult Error(string message)
        {
            return new JsonResult { ok = false, message = message };
        }
    }
}