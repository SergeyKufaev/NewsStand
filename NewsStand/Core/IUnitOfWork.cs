using NewsStand.Core.Repositories;

namespace NewsStand.Core
{
    public interface IUnitOfWork
    {
        IAuthorRepository Authors { get; }
        IAuthorProductRepository AuthorProducts { get; }
        ICustomerRepository Customers { get; }
        IProducerRepository Producers { get; }
        IProductRepository Products { get; }
        IPurchaseRepository Purchases { get; }
        IPurchaseProductRepository PurchaseProducts { get; }
        void Complete();
    }
}
