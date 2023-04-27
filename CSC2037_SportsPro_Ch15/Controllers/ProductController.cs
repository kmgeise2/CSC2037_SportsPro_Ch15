using CSC2037_SportsPro_Ch15.Models;
using Microsoft.AspNetCore.Mvc;

namespace CSC2037_SportsPro_Ch15.Controllers
{
    public class ProductController : Controller
    {
        private Repository<Product> data { get; set; }
        public ProductController(SportsProContext ctx) => data = new Repository<Product>(ctx);

        public RedirectToActionResult Index() => RedirectToAction("List");

        [Route("[controller]s")]
        public ViewResult List()
        {
            var products = data.List(new QueryOptions<Product> { OrderBy = p => p.ReleaseDate });
            return View(products);
        }

        [HttpGet]
        public ViewResult Add()
        {
            ViewBag.Action = "Add";
            return View("AddEdit", new Product());
        }

        [HttpGet]
        public ViewResult Edit(int id)
        {
            ViewBag.Action = "Edit";
            var product = data.Get(id);
            return View("AddEdit", product);
        }

        [HttpPost]
        public IActionResult Save(Product product)
        {
            string message = string.Empty;
            if (ModelState.IsValid)
            {
                if (product.ProductID == 0)
                {
                    data.Insert(product);
                    message = product.Name + " was added.";
                }
                else
                {
                    data.Update(product);
                    message = product.Name + " was updated.";
                }
                data.Save();
                TempData["message"] = message;
                return RedirectToAction("List");
            }
            else
            {
                if (product.ProductID == 0)
                {
                    ViewBag.Action = "Add";
                }
                else
                {
                    ViewBag.Action = "Edit";
                }
                return View(product);
            }
        }

        [HttpGet]
        public ViewResult Delete(int id)
        {
            var product = data.Get(id);
            return View(product);
        }

        [HttpPost]
        public RedirectToActionResult Delete(Product product)
        {
            data.Delete(product);
            data.Save();
            TempData["message"] = product.Name + " was deleted.";
            return RedirectToAction("List");
        }
    }
}