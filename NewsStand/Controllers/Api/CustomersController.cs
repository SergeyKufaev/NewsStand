using System.Collections.Generic;
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
        public ActionResult<IEnumerable<CustomerViewModel>> GetCustomers()
        {
            var customers = _unitOfWork.Customers.GetAll();

            return Ok(_mapper.Map<IEnumerable<CustomerViewModel>>(customers));
        }

        [HttpGet("{id:int}")]
        public ActionResult<CustomerViewModel> GetCustomer(int id)
        {
            var customer = _unitOfWork.Customers.GetById(id);

            if (customer == null)
                return NotFound();

            return Ok(_mapper.Map<CustomerViewModel>(customer));
        }

        [HttpPost]
        public ActionResult CreateCustomer([FromBody] CustomerViewModel customerViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var customer = _mapper.Map<Customer>(customerViewModel);
            _unitOfWork.Customers.Add(customer);

            _unitOfWork.Complete();

            return Created($"/api/customers/{customer.Id}", _mapper.Map<CustomerViewModel>(customer));
        }

        [HttpPut("{id:int}")]
        public ActionResult UpdateCustomer(int id, [FromBody] CustomerViewModel customerViewModel)
        {
            if (id != customerViewModel.Id || !ModelState.IsValid)
                return BadRequest();

            var customer = _unitOfWork.Customers.GetById(id);

            if (customer == null)
                return NotFound();

            _mapper.Map(customerViewModel, customer);
            _unitOfWork.Customers.Update(customer);

            _unitOfWork.Complete();

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public ActionResult DeleteCustomer(int id)
        {
            var customer = _unitOfWork.Customers.GetById(id);

            if (customer == null)
                return NotFound();

            _unitOfWork.Customers.Delete(customer);

            _unitOfWork.Complete();

            return NoContent();
        }
    }
}
