using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using NewsStand.Core.Entities;
using NewsStand.Core.Services.Interfaces;
using NewsStand.Core.ViewModels;

namespace NewsStand.Core.Services
{
    public class ProducerService : IProducerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProducerService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProducerViewModel>> GetProducersAsync()
        {
            var producers = await _unitOfWork.Producers.GetAllAsync();

            return _mapper.Map<IEnumerable<ProducerViewModel>>(producers);
        }

        public async Task<ProducerViewModel> GetProducerByIdAsync(int id)
        {
            var producer = await _unitOfWork.Producers.GetByIdAsync(id);

            if (producer == null)
                return null;

            var producerViewModel = _mapper.Map<ProducerViewModel>(producer);

            for (int i = 0; i < producer.Products.Count; i++)
            {
                var authors = producer.Products.ElementAt(i).AuthorProducts.Select(ap => ap.Author).ToList();
                producerViewModel.Products.ElementAt(i).Authors = _mapper.Map<List<AuthorViewModel>>(authors);
            }

            return producerViewModel;
        }

        public async Task<ProducerViewModel> CreateProducerAsync(ProducerViewModel producerViewModel)
        {
            var producer = _mapper.Map<Producer>(producerViewModel);
            await _unitOfWork.Producers.AddAsync(producer);

            await _unitOfWork.CompleteAsync();

            return _mapper.Map<ProducerViewModel>(producer);
        }

        public async Task<bool> UpdateProducerAsync(int id, ProducerViewModel producerViewModel)
        {
            var producer = await _unitOfWork.Producers.GetByIdAsync(id);

            if (producer == null)
                return false;

            //_mapper.Map(producerViewModel, producer);
            producer.Name = producerViewModel.Name;
            await _unitOfWork.Producers.UpdateAsync(producer);

            await _unitOfWork.CompleteAsync();

            return true;
        }

        public async Task<bool> DeleteProducerAsync(int id)
        {
            var producer = await _unitOfWork.Producers.GetByIdAsync(id);

            if (producer == null)
                return false;

            await _unitOfWork.Producers.DeleteAsync(producer);

            await _unitOfWork.CompleteAsync();

            return true;
        }
    }
}
