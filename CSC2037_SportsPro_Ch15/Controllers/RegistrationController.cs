using CSC2037_SportsPro_Ch15.Models;
using Microsoft.AspNetCore.Mvc;

namespace CSC2037_SportsPro_Ch15.Controllers
{
    public class RegistrationController : Controller
    {
        private Repository<Customer> customerData { get; set; }
        private Repository<Product> productData { get; set; }
        public RegistrationController(SportsProContext ctx)
        {
            customerData = new Repository<Customer>(ctx);
            productData = new Repository<Product>(ctx);
        }

        [HttpGet]
        public IActionResult Index()
        {
            var model = new RegistrationViewModel
            {
                Customer = new Customer(),
                Customers = customerData.List(new QueryOptions<Customer> { OrderBy = c => c.LastName })
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Index(RegistrationViewModel model)
        {
            if (model.HasCustomer)
            {
                return RedirectToAction("List", new { id = model.Customer.CustomerID });
            }
            else
            {
                TempData["message"] = "You must select a customer.";
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        [Route("[controller]s/{id?}")]
        public IActionResult List(int id)
        {
            // get selected customer and related products 
            var options = new QueryOptions<Customer>
            {
                Includes = "Products",
                Where = c => c.CustomerID == id
            };
            var model = new RegistrationViewModel
            {
                Customer = customerData.Get(options)!
            };

            if (model.HasCustomer)
            {
                // get list of products for drop-down and display view
                model.Products = productData.List(new QueryOptions<Product> { OrderBy = p => p.Name });
                return View(model);
            }
            else
            {
                TempData["message"] = "Customer not found. Please select a customer.";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult Register(RegistrationViewModel model)
        {
            if (model.HasProduct)
            {
                // get customer and product from database
                var options = new QueryOptions<Customer>
                {
                    Includes = "Products",
                    Where = c => c.CustomerID == model.Customer.CustomerID
                };
                model.Customer = customerData.Get(options)!;
                model.Product = productData.Get(model.Product.ProductID)!;

                if (model.HasCustomer && model.HasProduct)
                {
                    if (model.Customer.Products.Contains(model.Product))
                    {
                        TempData["message"] = $"{model.Product.Name} is already registered to {model.Customer.FullName}";

                        // re-display view
                        model.Products = productData.List(new QueryOptions<Product> { OrderBy = p => p.Name });
                        return View("List", model);
                    }
                    else
                    {
                        model.Customer.Products.Add(model.Product);
                        customerData.Save();
                        TempData["message"] = $"{model.Product.Name} has been registered to {model.Customer.FullName}";
                    }
                }
            }
            else  // no product selected
            {
                TempData["message"] = "You must select a product.";
            }

            return RedirectToAction("List", new { ID = model.Customer.CustomerID });
        }

        [HttpPost]
        public IActionResult Delete(RegistrationViewModel model)
        {
            // get customer and product from database
            var options = new QueryOptions<Customer>
            {
                Includes = "Products",
                Where = c => c.CustomerID == model.Customer.CustomerID
            };
            model.Customer = customerData.Get(options)!;
            model.Product = productData.Get(model.Product.ProductID)!;

            if (model.HasCustomer && model.HasProduct)
            {
                model.Customer.Products.Remove(model.Product);
                customerData.Save();
                TempData["message"] = $"{model.Product.Name} has been de-registered from {model.Customer.FullName}";
            }

            return RedirectToAction("List", new { ID = model.Customer.CustomerID });
        }
    }
}