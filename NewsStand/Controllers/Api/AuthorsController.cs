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
    public class AuthorsController : ControllerBase
    {
        private readonly IAuthorService _authorService;

        public AuthorsController(IAuthorService authorService)
        {
            _authorService = authorService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuthorViewModel>>> GetAuthors()
        {
            var authorViewModels = await _authorService.GetAuthorsAsync();

            return Ok(authorViewModels);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<AuthorViewModel>> GetAuthor(int id)
        {
            var authorViewModel = await _authorService.GetAuthorByIdAsync(id);

            return authorViewModel != null ? Ok(authorViewModel) : NotFound();
        }

        [HttpPost]
        public async Task<ActionResult> CreateAuthor([FromBody] AuthorViewModel authorViewModel)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            authorViewModel = await _authorService.CreateAuthorAsync(authorViewModel);

            return Created($"/api/authors/{authorViewModel.Id}", authorViewModel);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateAuthor(int id, [FromBody] AuthorViewModel authorViewModel)
        {
            if (id != authorViewModel.Id || !ModelState.IsValid)
                return BadRequest();

            var result = await _authorService.UpdateAuthorAsync(id, authorViewModel);

            return result ? NoContent() : NotFound();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteAuthor(int id)
        {
            var result = await _authorService.DeleteAuthorAsync(id);

            return result ? NoContent() : NotFound();
        }
    }
}
