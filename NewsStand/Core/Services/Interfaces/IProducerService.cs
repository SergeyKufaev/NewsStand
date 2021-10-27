using System.Collections.Generic;
using System.Threading.Tasks;
using NewsStand.Core.ViewModels;

namespace NewsStand.Core.Services.Interfaces
{
    public interface IProducerService
    {
        Task<IEnumerable<ProducerViewModel>> GetProducersAsync();
        Task<ProducerViewModel> GetProducerByIdAsync(int id);
        Task<ProducerViewModel> CreateProducerAsync(ProducerViewModel producerViewModel);
        Task<bool> UpdateProducerAsync(int id, ProducerViewModel producerViewModel);
        Task<bool> DeleteProducerAsync(int id);
    }
}
