using CSC2037_SportsPro_Ch15.Models;
using Microsoft.AspNetCore.Mvc;

namespace CSC2037_SportsPro_Ch15.Controllers
{
    public class CustomerController : Controller
    {
        private Repository<Customer> customerData { get; set; }
        private Repository<Country> countryData { get; set; }
        public CustomerController(SportsProContext ctx)
        {
            customerData = new Repository<Customer>(ctx);
            countryData = new Repository<Country>(ctx);
        }

        public IActionResult Index() => RedirectToAction("List");

        [Route("[controller]s")]
        public IActionResult List()
        {
            var customers = customerData.List(new QueryOptions<Customer> { OrderBy = c => c.LastName });
            return View(customers);
        }

        [HttpGet]
        public IActionResult Add()
        {
            ViewBag.Action = "Add";

            ViewBag.Countries = countryData.List(new QueryOptions<Country> { OrderBy = c => c.Name });

            return View("AddEdit", new Customer());
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            ViewBag.Action = "Edit";

            ViewBag.Countries = countryData.List(new QueryOptions<Country> { OrderBy = c => c.Name });

            var customer = customerData.Get(id);
            return View("AddEdit", customer);
        }

        [HttpPost]
        public IActionResult Save(Customer customer)
        {
            // do remote validation check on server if doesn't run on client
            if (TempData["okEmail"] == null)  // is null if remote validation doesn't run 
            {
                string msg = Check.EmailExists(customerData, customer.Email!);
                if (!String.IsNullOrEmpty(msg))
                {
                    ModelState.AddModelError(nameof(Customer.Email), msg);
                }
            }

            if (ModelState.IsValid)
            {
                if (customer.CustomerID == 0)
                {
                    customerData.Insert(customer);
                }
                else
                {
                    customerData.Update(customer);
                }
                customerData.Save();
                return RedirectToAction("List");
            }
            else
            {
                if (customer.CustomerID == 0)
                {
                    ViewBag.Action = "Add";
                }
                else
                {
                    ViewBag.Action = "Edit";
                }
                ViewBag.Countries = countryData.List(new QueryOptions<Country> { OrderBy = c => c.Name });
                return View("AddEdit", customer);
            }
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var customer = customerData.Get(id);
            return View(customer);
        }

        [HttpPost]
        public IActionResult Delete(Customer customer)
        {
            customerData.Delete(customer);
            customerData.Save();
            return RedirectToAction("List");
        }

    }
}