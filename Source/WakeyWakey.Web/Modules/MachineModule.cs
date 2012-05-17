using Nancy;
using WakeyWakey.Web.Models;
using WakeyWakey.Web.ViewModels;
using System.Linq;
using Nancy.ModelBinding;

namespace WakeyWakey.Web.Modules
{
    public class MachineModule : NancyModule
    {
        readonly WakeyCatalog _catalog;

        public MachineModule(WakeyCatalog catalog) : base("/machines")
        {
            _catalog = catalog;

            Get["/"] = ListMachines;

            Get["/{id}"] = GetMachine;
            Get["/new"] = NewMachine;
            Post["/new"] = CreateNewMachine;
            //Get["/{id}/edit"] = EditMachine;
            //Post["/{id}/edit"] = DoEditMachine;
            Delete["/{id}"] = DeleteMachine;
            Post["/{id}/wake"] = DoWakeMachine;
        }

        private Response ListMachines(dynamic parameters)
        {
            var machines = _catalog.Machines;

            return View["list", machines];
        }

        private Response GetMachine(dynamic parameters)
        {
            var id = (int) parameters.id;
            var machine = _catalog.Machines.FirstOrDefault(x => x.Id == id);
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

            var machine = _catalog.Machines.FirstOrDefault(x => x.Id == id);
            
            _catalog.Machines.Remove(machine);
            _catalog.SaveChanges();

            return Response.AsJson(JsonResult.OK());
        }

        private Response DoWakeMachine(dynamic parameters)
        {
            return Response.AsJson(JsonResult.OK());
        }
    }
}