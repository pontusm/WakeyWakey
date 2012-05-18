using System;
using System.Globalization;
using System.Net.NetworkInformation;
using Nancy;
using WakeyWakey.Web.Models;
using WakeyWakey.Web.Services;
using WakeyWakey.Web.ViewModels;
using System.Linq;
using Nancy.ModelBinding;

namespace WakeyWakey.Web.Modules
{
    public class MachineModule : NancyModule
    {
        readonly WakeyCatalog _catalog;
        readonly NetworkService _networkService;

        public MachineModule(WakeyCatalog catalog, NetworkService networkService) : base("/machines")
        {
            _catalog = catalog;
            _networkService = networkService;

            Get["/"] = ListMachines;

            Get["/{id}"] = GetMachine;
            Get["/new"] = NewMachine;
            Post["/new"] = CreateNewMachine;
            //Get["/{id}/edit"] = EditMachine;
            //Post["/{id}/edit"] = DoEditMachine;
            Delete["/{id}"] = DeleteMachine;
            Post["/{id}/wake"] = DoWakeMachine;
            Post["/{id}/ping"] = DoPingMachine;
        }

        private Response ListMachines(dynamic parameters)
        {
            var machines = _catalog.Machines;

            return View["list", machines];
        }

        private Response GetMachine(dynamic parameters)
        {
            var id = (int) parameters.id;
            var machine = FindMachineById(id);
            if (machine == null)
                return HttpStatusCode.NotFound;

            return View["machines", new BaseViewModel()];
        }

        private Response NewMachine(dynamic parameters)
        {
            return View["new"];
        }

        private Response CreateNewMachine(dynamic parameters)
        {
            var machine = this.Bind<Machine>("Id");

            _catalog.Machines.Add(machine);
            _catalog.SaveChanges();
            
            return Response.AsRedirect("/");
        }

        private Response DeleteMachine(dynamic parameters)
        {
            var id = (int) parameters.id;

            var machine = FindMachineById(id);
            if (machine == null)
                return HttpStatusCode.NotFound;

            _catalog.Machines.Remove(machine);
            _catalog.SaveChanges();

            return Response.AsJson(JsonResult.OK());
        }

        private Response DoWakeMachine(dynamic parameters)
        {
            var id = (int) parameters.id;
            var machine = FindMachineById(id);
            if (machine == null)
                return HttpStatusCode.NotFound;

            if (string.IsNullOrEmpty(machine.MacAddress))
                return Response.AsJson(JsonResult.Error("No valid MAC address registered."));

            byte[] macAddress = machine.MacAddress.Split(':')
                .Select(x => byte.Parse(x, NumberStyles.HexNumber))
                .ToArray();
            if (macAddress.Length != 6)
                return Response.AsJson(JsonResult.Error("MAC address length is incorrect."));

            _networkService.SendMagicPacket(macAddress);

            return Response.AsJson(JsonResult.OK());
        }

        private Response DoPingMachine(dynamic parameters)
        {
            var id = (int) parameters.id;
            var machine = FindMachineById(id);
            if (machine == null)
                return HttpStatusCode.NotFound;

            if (string.IsNullOrEmpty(machine.HostName))
                return Response.AsJson(JsonResult.Error("No valid host name registered."));

            var reply = _networkService.Ping(machine.HostName, TimeSpan.FromSeconds(5));
            if (reply.Status != IPStatus.Success)
                return Response.AsJson(JsonResult.Error(string.Format("Ping error: {0}", reply.Status)));

            return Response.AsJson(JsonResult.OK());
        }

        private Machine FindMachineById(int id)
        {
            return _catalog.Machines.FirstOrDefault(x => x.Id == id);
        }
    }
}