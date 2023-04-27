using CSC2037_SportsPro_Ch15.Models;
using Microsoft.AspNetCore.Mvc;

namespace CSC2037_SportsPro_Ch15.Controllers
{
    public class IncidentController : Controller
    {
        private Repository<Incident> incidentData { get; set; }
        private Repository<Customer> customerData { get; set; }
        private Repository<Product> productData { get; set; }
        private Repository<Technician> techData { get; set; }
        public IncidentController(SportsProContext ctx)
        {
            incidentData = new Repository<Incident>(ctx);
            customerData = new Repository<Customer>(ctx);
            productData = new Repository<Product>(ctx);
            techData = new Repository<Technician>(ctx);
        }

        public IActionResult Index() => RedirectToAction("List");

        [Route("[controller]s/{filter?}")]
        public IActionResult List(string filter = "all")
        {
            var options = new QueryOptions<Incident>
            {
                Includes = "Customer, Product",
                OrderBy = i => i.DateOpened
            };
            if (filter == "unassigned")
            {
                options.Where = i => i.TechnicianID == -1;
            }
            if (filter == "open")
            {
                options.Where = i => i.DateClosed == null;
            }

            var model = new IncidentListViewModel
            {
                Filter = filter,
                Incidents = incidentData.List(options)
            };

            return View(model);
        }

        public IActionResult Filter(string id)
        {
            return RedirectToAction("List", new { Filter = id });
        }

        private IncidentViewModel GetViewModel(string action = "")
        {
            IncidentViewModel model = new IncidentViewModel
            {
                Customers = customerData.List(new QueryOptions<Customer>
                {
                    OrderBy = c => c.FirstName
                }),
                Products = productData.List(new QueryOptions<Product>
                {
                    OrderBy = p => p.Name
                }),
                Technicians = techData.List(new QueryOptions<Technician>
                {
                    OrderBy = t => t.Name
                }),
            };
            if (!String.IsNullOrEmpty(action))
                model.Action = action;

            return model;
        }

        [HttpGet]
        public IActionResult Add()
        {
            IncidentViewModel model = GetViewModel("Add");
            model.Incident = new Incident();

            return View("AddEdit", model);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            IncidentViewModel model = GetViewModel("Edit");
            model.Incident = incidentData.Get(id)!;

            return View("AddEdit", model);
        }

        [HttpPost]
        public IActionResult Save(Incident incident)
        {
            if (ModelState.IsValid)
            {
                if (incident.IncidentID == 0)
                {
                    incidentData.Insert(incident);
                }
                else
                {
                    incidentData.Update(incident);
                }
                incidentData.Save();
                return RedirectToAction("List");
            }
            else
            {
                var model = GetViewModel();
                model.Incident = incident;

                if (incident.IncidentID == 0)
                {
                    model.Action = "Add";
                }
                else
                {
                    model.Action = "Edit";
                }
                return View("AddEdit", model);
            }
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var product = incidentData.Get(id);
            return View(product);
        }

        [HttpPost]
        public IActionResult Delete(Incident incident)
        {
            incidentData.Delete(incident);
            incidentData.Save();
            return RedirectToAction("List");
        }
    }
}