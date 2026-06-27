using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SaaSify.Application.DTOs;
using SaaSify.Application.Interface.Services;

namespace SaaSify.Api.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var customers = await _customerService.GetAllAsync();
            return Ok(customers);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var customer = await _customerService.GetByIdAsync(id);
            if (customer == null)
                return NotFound(new { message = "Customer not found." });

            return Ok(customer);
        }

        [HttpPost("save")]
        public async Task<IActionResult> Save([FromBody] CustomerRequest request)
        {
            var customer = await _customerService.SaveAsync(request);
            return Ok(customer);
        }




        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var success = await _customerService.DeleteAsync(id);
            if (!success)
                return NotFound(new { message = "Customer not found." });

            return NoContent();
        }
    }
}
