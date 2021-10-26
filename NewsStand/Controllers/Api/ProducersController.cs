using System.Collections.Generic;
using System.Linq;
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
    public class ProducersController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProducersController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProducerViewModel>>> GetProducers()
        {
            var producers = await _unitOfWork.Producers.GetAllAsync();

            return Ok(_mapper.Map<IEnumerable<ProducerViewModel>>(producers));
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProducerViewModel>> GetProducer(int id)
        {
            var producer = await _unitOfWork.Producers.GetByIdAsync(id);

            if (producer == null)
                return NotFound();

            var producerViewModel = _mapper.Map<ProducerViewModel>(producer);

            for (int i = 0; i < producer.Products.Count; i++)
            {
                var authors = producer.Products.ElementAt(i).AuthorProducts.Select(ap => ap.Author).ToList();
                producerViewModel.Products.ElementAt(i).Authors = _mapper.Map<List<AuthorViewModel>>(authors);
            }

            return Ok(producerViewModel);
        }

        [HttpPost]
        public async Task<ActionResult> CreateProducer([FromBody] ProducerViewModel producerViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var producer = _mapper.Map<Producer>(producerViewModel);
            await _unitOfWork.Producers.AddAsync(producer);

            await _unitOfWork.CompleteAsync();

            return Created($"/api/producers/{producer.Id}", _mapper.Map<ProducerViewModel>(producer));
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateProducer(int id, [FromBody] ProducerViewModel producerViewModel)
        {
            if (id != producerViewModel.Id || !ModelState.IsValid)
                return BadRequest();

            var producer = await _unitOfWork.Producers.GetByIdAsync(id);

            if (producer == null)
                return NotFound();

            //_mapper.Map(producerViewModel, producer);
            producer.Name = producerViewModel.Name;
            await _unitOfWork.Producers.UpdateAsync(producer);

            await _unitOfWork.CompleteAsync();

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteProducer(int id)
        {
            var producer = await _unitOfWork.Producers.GetByIdAsync(id);

            if (producer == null)
                return NotFound();

            await _unitOfWork.Producers.DeleteAsync(producer);

            await _unitOfWork.CompleteAsync();

            return NoContent();
        }
    }
}
