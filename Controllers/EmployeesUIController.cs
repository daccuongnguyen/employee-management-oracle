using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Controllers
{
    public class EmployeesUIController : Controller
    {
        public IActionResult Index()
        {
            return View(); // Sẽ render Views/Employees/Index.cshtml
        }
    }
}
