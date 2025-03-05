using Microsoft.AspNetCore.Mvc;
using ECommerceApp.DTOs;
using ECommerceApp.DTOs.CustomerDTOs;
using ECommerceApp.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
namespace ECommerceApp.Controllers
{
    [ApiController]
    [Route("api/Customers")]
    public class CustomersController : ControllerBase
    {
        private readonly CustomerService _customerService;
        private readonly IConfiguration _config;
        // Injecting the services
        public CustomersController(CustomerService customerService, IConfiguration config)
        {
            _customerService = customerService;
            _config = config;
        }

        // Registers a new customer.
        [HttpPost("RegisterCustomer")]
        public async Task<ActionResult<ApiResponse<CustomerResponseDTO>>> RegisterCustomer([FromBody] CustomerRegistrationDTO customerDto)
        {
            var response = await _customerService.RegisterCustomerAsync(customerDto);
            if (response.StatusCode != 200)
            {
                return StatusCode((int)response.StatusCode, response);
            }
            return Ok(response);
        }

        // Logs in a customer.
        [HttpPost("Login")]
        public async Task<ActionResult<String>> Login([FromBody] LoginDTO loginDto)
        {
            var response = await _customerService.LoginAsync(loginDto);
            if (response.StatusCode != 200)
            {
                return StatusCode((int)response.StatusCode, response);
            }

            var jwtSettings = _config.GetSection("Jwt");
            var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]);

            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, loginDto.Password ),
            new Claim(JwtRegisteredClaimNames.Email, loginDto.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings["ExpireMinutes"])),
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);


            // return Ok(response);
        }

        // Retrieves customer details by ID.
        [HttpGet("GetCustomerById/{id}")]
        public async Task<ActionResult<ApiResponse<CustomerResponseDTO>>> GetCustomerById(int id)
        {
            var response = await _customerService.GetCustomerByIdAsync(id);
            if (response.StatusCode != 200)
            {
                return StatusCode((int)response.StatusCode, response);
            }
            return Ok(response);
        }
        // Retrieves customer details by ID.
        [HttpGet("test")]
        public IActionResult test()
        {

            return Ok("test");
        }
        // Updates customer details.
        [HttpPut("UpdateCustomer")]
        public async Task<ActionResult<ApiResponse<ConfirmationResponseDTO>>> UpdateCustomer([FromBody] CustomerUpdateDTO customerDto)
        {
            var response = await _customerService.UpdateCustomerAsync(customerDto);
            if (response.StatusCode != 200)
            {
                return StatusCode((int)response.StatusCode, response);
            }
            return Ok(response);
        }

        // Deletes a customer by ID.
        [HttpDelete("DeleteCustomer/{id}")]
        public async Task<ActionResult<ApiResponse<ConfirmationResponseDTO>>> DeleteCustomer(int id)
        {
            var response = await _customerService.DeleteCustomerAsync(id);
            if (response.StatusCode != 200)
            {
                return StatusCode((int)response.StatusCode, response);
            }
            return Ok(response);
        }

        // Changes the password for an existing customer.
        [HttpPost("ChangePassword")]
        public async Task<ActionResult<ApiResponse<ConfirmationResponseDTO>>> ChangePassword([FromBody] ChangePasswordDTO changePasswordDto)
        {
            var response = await _customerService.ChangePasswordAsync(changePasswordDto);
            if (response.StatusCode != 200)
            {
                return StatusCode((int)response.StatusCode, response);
            }
            return Ok(response);
        }
    }
}