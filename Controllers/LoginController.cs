using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
