using EmployeeManagement.Models;
using EmployeeManagement.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EmployeeManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // 👈 tất cả API trong controller này yêu cầu JWT
    public class EmployeesController : ControllerBase
    {
        private readonly EmployeeService _service;
        private readonly IConfiguration _config;

        public EmployeesController(EmployeeService service, IConfiguration config)
        {
            _service = service;
            _config = config;
        }

        [HttpPost("login")]
        [AllowAnonymous] // login không cần JWT
        public IActionResult Login([FromBody] LoginModel model)
        {
            // Demo check user
            if (model.Username == "admin" && model.Password == "123")
            {
                var token = GenerateJwtToken(model.Username);
                return Ok(new { token, username = model.Username });
            }
            return Unauthorized();
        }

        [HttpGet]
        public IActionResult GetAll() => Ok(_service.GetAll());

        [HttpPost]
        public IActionResult Create(Employee emp)
        {
            _service.Add(emp);
            return Ok("Success");
        }

        [HttpPut]
        public IActionResult Update(Employee emp)
        {
            _service.Update(emp);
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _service.Delete(id);
            return Ok();
        }

        private string GenerateJwtToken(string username)
        {
            var jwtSettings = _config.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, "Admin") // 👈 có thể đổi role
            };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

    public class LoginModel
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
