using EmployeeManagement.Models;
using EmployeeManagement.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace EmployeeManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]  // 👈 chỉ cho phép người có token hợp lệ
    public class DepartmentsController : ControllerBase
    {
        private readonly DepartmentService _service;

        public DepartmentsController(DepartmentService service)
        {
            _service = service;
        }


        [HttpGet]
        [AllowAnonymous] // bỏ xác thực cho riêng endpoint này
        public IActionResult GetAll() => Ok(_service.GetAll());

        [HttpPost]
        public IActionResult Create(Department dept)
        {
            _service.Add(dept);
            return Ok();
        }

        [HttpPut]
        public IActionResult Update(Department dept)
        {
            _service.Update(dept);
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _service.Delete(id);
            return Ok();
        }
    }
}
