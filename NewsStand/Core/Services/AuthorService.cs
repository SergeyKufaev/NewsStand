using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using NewsStand.Core.Entities;
using NewsStand.Core.Services.Interfaces;
using NewsStand.Core.ViewModels;

namespace NewsStand.Core.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AuthorService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<AuthorViewModel>> GetAuthorsAsync()
        {
            var authors = await _unitOfWork.Authors.GetAllAsync();

            return _mapper.Map<IEnumerable<AuthorViewModel>>(authors);
        }

        public async Task<AuthorViewModel> GetAuthorByIdAsync(int id)
        {
            var author = await _unitOfWork.Authors.GetByIdAsync(id);

            if (author == null)
                return null;

            var authorViewModel = _mapper.Map<AuthorViewModel>(author);

            var products = author.AuthorProducts.Select(ap => ap.Product).ToList();
            authorViewModel.Products = _mapper.Map<List<ProductViewModel>>(products);

            return authorViewModel;
        }

        public async Task<AuthorViewModel> CreateAuthorAsync(AuthorViewModel authorViewModel)
        {
            var author = _mapper.Map<Author>(authorViewModel);
            await _unitOfWork.Authors.AddAsync(author);

            await _unitOfWork.CompleteAsync();

            return _mapper.Map<AuthorViewModel>(author);
        }

        public async Task<bool> UpdateAuthorAsync(int id, AuthorViewModel authorViewModel)
        {
            var author = await _unitOfWork.Authors.GetByIdAsync(id);

            if (author == null)
                return false;

            _mapper.Map(authorViewModel, author);
            await _unitOfWork.Authors.UpdateAsync(author);

            await _unitOfWork.CompleteAsync();

            return true;
        }

        public async Task<bool> DeleteAuthorAsync(int id)
        {
            var author = await _unitOfWork.Authors.GetByIdAsync(id);

            if (author == null)
                return false;

            await _unitOfWork.Authors.DeleteAsync(author);

            await _unitOfWork.CompleteAsync();

            return true;
        }
    }
}
