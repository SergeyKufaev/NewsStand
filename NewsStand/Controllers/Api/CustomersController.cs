using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsStand.Core.Services.Interfaces;
using NewsStand.Core.ViewModels;

namespace NewsStand.Controllers.Api
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerViewModel>>> GetCustomers()
        {
            var customerViewModels = await _customerService.GetCustomersAsync();

            return Ok(customerViewModels);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<CustomerViewModel>> GetCustomer(int id)
        {
            var customerViewModel = await _customerService.GetCustomerByIdAsync(id);

            return customerViewModel != null ? Ok(customerViewModel) : NotFound();
        }

        [HttpPost]
        public async Task<ActionResult> CreateCustomer([FromBody] CustomerViewModel customerViewModel)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            customerViewModel = await _customerService.CreateCustomerAsync(customerViewModel);

            return Created($"/api/customers/{customerViewModel.Id}", customerViewModel);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateCustomer(int id, [FromBody] CustomerViewModel customerViewModel)
        {
            if (id != customerViewModel.Id || !ModelState.IsValid)
                return BadRequest();

            var result = await _customerService.UpdateCustomerAsync(id, customerViewModel);

            return result ? NoContent() : NotFound();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteCustomer(int id)
        {
            var result = await _customerService.DeleteCustomerAsync(id);

            return result ? NoContent() : NotFound();
        }
    }
}
