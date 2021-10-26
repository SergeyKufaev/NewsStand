using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsStand.Core;
using NewsStand.Core.Entities;
using NewsStand.Core.ViewModels;

namespace NewsStand.Controllers.Api
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CustomersController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerViewModel>>> GetCustomers()
        {
            var customers = await _unitOfWork.Customers.GetAllAsync();

            return Ok(_mapper.Map<IEnumerable<CustomerViewModel>>(customers));
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<CustomerViewModel>> GetCustomer(int id)
        {
            var customer = await _unitOfWork.Customers.GetByIdAsync(id);

            if (customer == null)
                return NotFound();

            return Ok(_mapper.Map<CustomerViewModel>(customer));
        }

        [HttpPost]
        public async Task<ActionResult> CreateCustomer([FromBody] CustomerViewModel customerViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var customer = _mapper.Map<Customer>(customerViewModel);
            await _unitOfWork.Customers.AddAsync(customer);

            await _unitOfWork.CompleteAsync();

            return Created($"/api/customers/{customer.Id}", _mapper.Map<CustomerViewModel>(customer));
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateCustomer(int id, [FromBody] CustomerViewModel customerViewModel)
        {
            if (id != customerViewModel.Id || !ModelState.IsValid)
                return BadRequest();

            var customer = await _unitOfWork.Customers.GetByIdAsync(id);

            if (customer == null)
                return NotFound();

            _mapper.Map(customerViewModel, customer);
            await _unitOfWork.Customers.UpdateAsync(customer);

            await _unitOfWork.CompleteAsync();

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteCustomer(int id)
        {
            var customer = await _unitOfWork.Customers.GetByIdAsync(id);

            if (customer == null)
                return NotFound();

            await _unitOfWork.Customers.DeleteAsync(customer);

            await _unitOfWork.CompleteAsync();

            return NoContent();
        }
    }
}
