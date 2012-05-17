using System;
using Nancy;
using WakeyWakey.Web.Services;
using WakeyWakey.Web.ViewModels;

namespace WakeyWakey.Web.Modules
{
    public class ApiModule : NancyModule
    {
        readonly NetworkService _networkService;

        public ApiModule(NetworkService networkService) : base("/api")
        {
            _networkService = networkService;

            Get["/getmac/{ip}"] = GetMacAddress;
        }

        private Response GetMacAddress(dynamic parameters)
        {
            try
            {
                var ip = (string) parameters.ip;
                if (string.IsNullOrEmpty(ip))
                    return Response.AsJson(JsonResult.Error("No IP address specified."));

                var addr = _networkService.GetMACAddress(ip);
                if (addr == null)
                    return Response.AsJson(JsonResult.Error("Failed to retrieve MAC address."));

                var macAddress = BitConverter.ToString(addr).Replace("-", ":");
                return Response.AsJson(new JsonResult { ok = true, message = macAddress });
            }
            catch (Exception e)
            {
                return Response.AsJson(JsonResult.Error("Error: " + e.Message));
            }
        }
    }
}