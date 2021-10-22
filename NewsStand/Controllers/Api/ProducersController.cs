using System.Collections.Generic;
using System.Linq;
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
        public ActionResult<IEnumerable<ProducerViewModel>> GetProducers()
        {
            var producers = _unitOfWork.Producers.GetAll();

            return Ok(_mapper.Map<IEnumerable<ProducerViewModel>>(producers));
        }

        [HttpGet("{id:int}")]
        public ActionResult<ProducerViewModel> GetProducer(int id)
        {
            var producer = _unitOfWork.Producers.GetById(id);

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
        public ActionResult CreateProducer([FromBody] ProducerViewModel producerViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var producer = _mapper.Map<Producer>(producerViewModel);
            _unitOfWork.Producers.Add(producer);

            _unitOfWork.Complete();

            return Created($"/api/producers/{producer.Id}", _mapper.Map<ProducerViewModel>(producer));
        }

        [HttpPut("{id:int}")]
        public ActionResult UpdateProducer(int id, [FromBody] ProducerViewModel producerViewModel)
        {
            if (id != producerViewModel.Id || !ModelState.IsValid)
                return BadRequest();

            var producer = _unitOfWork.Producers.GetById(id);

            if (producer == null)
                return NotFound();

            //_mapper.Map(producerViewModel, producer);
            producer.Name = producerViewModel.Name;
            _unitOfWork.Producers.Update(producer);

            _unitOfWork.Complete();

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public ActionResult DeleteProducer(int id)
        {
            var producer = _unitOfWork.Producers.GetById(id);

            if (producer == null)
                return NotFound();

            _unitOfWork.Producers.Delete(producer);

            _unitOfWork.Complete();

            return NoContent();
        }
    }
}
