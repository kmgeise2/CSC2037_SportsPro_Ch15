using CSC2037_SportsPro_Ch15.Models;
using Microsoft.AspNetCore.Mvc;

namespace CSC2037_SportsPro_Ch15.Controllers
{
    public class ValidationController : Controller
    {
        private Repository<Customer> data { get; set; }
        public ValidationController(SportsProContext ctx) => data = new Repository<Customer>(ctx);

        public JsonResult CheckEmail(string email, int customerID)
        {
            if (customerID == 0)  // only check for new customers - don't check on edit
            {
                string msg = Check.EmailExists(data, email);
                if (!string.IsNullOrEmpty(msg))
                {
                    return Json(msg);
                }
            }

            TempData["okEmail"] = true;
            return Json(true);
        }
    }
}