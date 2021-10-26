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
    public class AuthorsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AuthorsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuthorViewModel>>> GetAuthors()
        {
            var authors = await _unitOfWork.Authors.GetAllAsync();

            return Ok(_mapper.Map<IEnumerable<AuthorViewModel>>(authors));
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<AuthorViewModel>> GetAuthor(int id)
        {
            var author = await _unitOfWork.Authors.GetByIdAsync(id);

            if (author == null)
                return NotFound();

            var authorViewModel = _mapper.Map<AuthorViewModel>(author);

            var products = author.AuthorProducts.Select(ap => ap.Product).ToList();
            authorViewModel.Products = _mapper.Map<List<ProductViewModel>>(products);

            return Ok(authorViewModel);
        }

        [HttpPost]
        public async Task<ActionResult> CreateAuthor([FromBody] AuthorViewModel authorViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var author = _mapper.Map<Author>(authorViewModel);
            await _unitOfWork.Authors.AddAsync(author);

            await _unitOfWork.CompleteAsync();

            return Created($"/api/authors/{author.Id}", _mapper.Map<AuthorViewModel>(author));
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateAuthor(int id, [FromBody] AuthorViewModel authorViewModel)
        {
            if (id != authorViewModel.Id || !ModelState.IsValid)
                return BadRequest();

            var author = await _unitOfWork.Authors.GetByIdAsync(id);

            if (author == null)
                return NotFound();

            _mapper.Map(authorViewModel, author);
            await _unitOfWork.Authors.UpdateAsync(author);

            await _unitOfWork.CompleteAsync();

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteAuthor(int id)
        {
            var author = await _unitOfWork.Authors.GetByIdAsync(id);

            if (author == null)
                return NotFound();

            await _unitOfWork.Authors.DeleteAsync(author);

            await _unitOfWork.CompleteAsync();

            return NoContent();
        }
    }
}
