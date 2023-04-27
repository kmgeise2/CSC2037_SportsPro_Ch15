using CSC2037_SportsPro_Ch15.Models;
using Microsoft.AspNetCore.Mvc;

namespace CSC2037_SportsPro_Ch15.Controllers
{
    public class TechIncidentController : Controller
    {
        private const string TECH_KEY = "techID";

        private Repository<Technician> techData { get; set; }
        private Repository<Incident> incidentData { get; set; }
        public TechIncidentController(SportsProContext ctx)
        {
            techData = new Repository<Technician>(ctx);
            incidentData = new Repository<Incident>(ctx);
        }

        [HttpGet]
        public IActionResult Index()
        {
            var options = new QueryOptions<Technician>
            {
                Where = t => t.TechnicianID > -1,  // skip default unassigned value
                OrderBy = c => c.Name
            };
            ViewBag.Technicians = techData.List(options);

            var technician = new Technician();

            int? techID = HttpContext.Session.GetInt32(TECH_KEY);
            if (techID.HasValue)
            {
                technician = techData.Get(techID.Value);
            }

            return View(technician);
        }

        [HttpPost]
        public IActionResult List(Technician technician)
        {
            if (technician.TechnicianID == 0)
            {
                TempData["message"] = "You must select a technician.";
                return RedirectToAction("Index");
            }
            else
            {
                HttpContext.Session.SetInt32(TECH_KEY, technician.TechnicianID);
                return RedirectToAction("List", new { id = technician.TechnicianID });
            }
        }

        [HttpGet]
        public IActionResult List(int id)
        {
            var technician = techData.Get(id);
            if (technician == null)
            {
                TempData["message"] = "Technician not found. Please select a technician.";
                return RedirectToAction("Index");
            }
            else
            {
                var options = new QueryOptions<Incident>
                {
                    Includes = "Customer, Product",
                    OrderBy = i => i.DateOpened,
                    Where = i => i.TechnicianID == id && i.DateClosed == null
                };
                var model = new TechIncidentViewModel
                {
                    Technician = technician,
                    Incidents = incidentData.List(options)
                };
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            int? techID = HttpContext.Session.GetInt32(TECH_KEY);
            if (!techID.HasValue)
            {
                TempData["message"] = "Technician not found. Please select a technician.";
                return RedirectToAction("Index");
            }

            var technician = techData.Get(techID.Value);
            if (technician == null)
            {
                TempData["message"] = "Technician not found. Please select a technician.";
                return RedirectToAction("Index");
            }
            else
            {
                var options = new QueryOptions<Incident>
                {
                    Includes = "Customer, Product",
                    Where = i => i.IncidentID == id
                };
                var model = new TechIncidentViewModel
                {
                    Technician = technician,
                    Incident = incidentData.Get(options)!
                };
                return View(model);
            }
        }

        [HttpPost]
        public IActionResult Edit(IncidentViewModel model)
        {
            Incident i = incidentData.Get(model.Incident.IncidentID)!;
            i.Description = model.Incident.Description;
            i.DateClosed = model.Incident.DateClosed;

            incidentData.Update(i);
            incidentData.Save();

            int? techID = HttpContext.Session.GetInt32(TECH_KEY);
            return RedirectToAction("List", new { id = techID });
        }
    }
}