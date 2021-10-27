using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using NewsStand.Core.Entities;
using NewsStand.Core.Services.Interfaces;
using NewsStand.Core.ViewModels;

namespace NewsStand.Core.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CustomerService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CustomerViewModel>> GetCustomersAsync()
        {
            var customers = await _unitOfWork.Customers.GetAllAsync();

            return _mapper.Map<IEnumerable<CustomerViewModel>>(customers);
        }

        public async Task<CustomerViewModel> GetCustomerByIdAsync(int id)
        {
            var customer = await _unitOfWork.Customers.GetByIdAsync(id);

            if (customer == null)
                return null;

            var customerViewModel = _mapper.Map<CustomerViewModel>(customer);

            return customerViewModel;
        }

        public async Task<CustomerViewModel> CreateCustomerAsync(CustomerViewModel customerViewModel)
        {
            var customer = _mapper.Map<Customer>(customerViewModel);
            await _unitOfWork.Customers.AddAsync(customer);

            await _unitOfWork.CompleteAsync();

            return _mapper.Map<CustomerViewModel>(customer);
        }

        public async Task<bool> UpdateCustomerAsync(int id, CustomerViewModel customerViewModel)
        {
            var customer = await _unitOfWork.Customers.GetByIdAsync(id);

            if (customer == null)
                return false;

            _mapper.Map(customerViewModel, customer);
            await _unitOfWork.Customers.UpdateAsync(customer);

            await _unitOfWork.CompleteAsync();

            return true;
        }

        public async Task<bool> DeleteCustomerAsync(int id)
        {
            var customer = await _unitOfWork.Customers.GetByIdAsync(id);

            if (customer == null)
                return false;

            await _unitOfWork.Customers.DeleteAsync(customer);

            await _unitOfWork.CompleteAsync();

            return true;
        }
    }
}
