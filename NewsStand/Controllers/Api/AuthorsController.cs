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
        public ActionResult<IEnumerable<AuthorViewModel>> GetAuthors()
        {
            var authors = _unitOfWork.Authors.GetAll();

            return Ok(_mapper.Map<IEnumerable<AuthorViewModel>>(authors));
        }

        [HttpGet("{id:int}")]
        public ActionResult<AuthorViewModel> GetAuthor(int id)
        {
            var author = _unitOfWork.Authors.GetById(id);

            if (author == null)
                return NotFound();

            var authorViewModel = _mapper.Map<AuthorViewModel>(author);

            var products = author.AuthorProducts.Select(ap => ap.Product).ToList();
            authorViewModel.Products = _mapper.Map<List<ProductViewModel>>(products);

            return Ok(authorViewModel);
        }

        [HttpPost]
        public ActionResult CreateAuthor([FromBody] AuthorViewModel authorViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var author = _mapper.Map<Author>(authorViewModel);
            _unitOfWork.Authors.Add(author);

            _unitOfWork.Complete();

            return Created($"/api/authors/{author.Id}", _mapper.Map<AuthorViewModel>(author));
        }

        [HttpPut("{id:int}")]
        public ActionResult UpdateAuthor(int id, [FromBody] AuthorViewModel authorViewModel)
        {
            if (id != authorViewModel.Id || !ModelState.IsValid)
                return BadRequest();

            var author = _unitOfWork.Authors.GetById(id);

            if (author == null)
                return NotFound();

            _mapper.Map(authorViewModel, author);
            _unitOfWork.Authors.Update(author);

            _unitOfWork.Complete();

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public ActionResult DeleteAuthor(int id)
        {
            var author = _unitOfWork.Authors.GetById(id);

            if (author == null)
                return NotFound();

            _unitOfWork.Authors.Delete(author);

            _unitOfWork.Complete();

            return NoContent();
        }
    }
}
