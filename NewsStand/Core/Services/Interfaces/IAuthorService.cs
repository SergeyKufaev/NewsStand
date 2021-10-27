using System.Collections.Generic;
using System.Threading.Tasks;
using NewsStand.Core.ViewModels;

namespace NewsStand.Core.Services.Interfaces
{
    public interface IAuthorService
    {
        Task<IEnumerable<AuthorViewModel>> GetAuthorsAsync();
        Task<AuthorViewModel> GetAuthorByIdAsync(int id);
        Task<AuthorViewModel> CreateAuthorAsync(AuthorViewModel authorViewModel);
        Task<bool> UpdateAuthorAsync(int id, AuthorViewModel authorViewModel);
        Task<bool> DeleteAuthorAsync(int id);
    }
}
