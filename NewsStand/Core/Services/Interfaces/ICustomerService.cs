using System.Collections.Generic;
using System.Threading.Tasks;
using NewsStand.Core.ViewModels;

namespace NewsStand.Core.Services.Interfaces
{
    public interface ICustomerService
    {
        Task<IEnumerable<CustomerViewModel>> GetCustomersAsync();
        Task<CustomerViewModel> GetCustomerByIdAsync(int id);
        Task<CustomerViewModel> CreateCustomerAsync(CustomerViewModel customerViewModel);
        Task<bool> UpdateCustomerAsync(int id, CustomerViewModel customerViewModel);
        Task<bool> DeleteCustomerAsync(int id);
    }
}
