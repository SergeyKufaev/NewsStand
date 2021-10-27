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
    public class ProducersController : ControllerBase
    {
        private readonly IProducerService _producerService;

        public ProducersController(IProducerService producerService)
        {
            _producerService = producerService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProducerViewModel>>> GetProducers()
        {
            var producerViewModels = await _producerService.GetProducersAsync();

            return Ok(producerViewModels);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProducerViewModel>> GetProducer(int id)
        {
            var producerViewModel = await _producerService.GetProducerByIdAsync(id);

            return producerViewModel != null ? Ok(producerViewModel) : NotFound();
        }

        [HttpPost]
        public async Task<ActionResult> CreateProducer([FromBody] ProducerViewModel producerViewModel)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            producerViewModel = await _producerService.CreateProducerAsync(producerViewModel);

            return Created($"/api/producers/{producerViewModel.Id}", producerViewModel);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateProducer(int id, [FromBody] ProducerViewModel producerViewModel)
        {
            if (id != producerViewModel.Id || !ModelState.IsValid)
                return BadRequest();

            var result = await _producerService.UpdateProducerAsync(id, producerViewModel);

            return result ? NoContent() : NotFound();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteProducer(int id)
        {
            var result = await _producerService.DeleteProducerAsync(id);

            return result ? NoContent() : NotFound();
        }
    }
}
