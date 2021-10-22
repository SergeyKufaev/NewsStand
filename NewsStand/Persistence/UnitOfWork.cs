using NewsStand.Core;
using NewsStand.Core.Repositories;
using NewsStand.Persistence.Repositories;

namespace NewsStand.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        readonly NewsStandDbContext _context;

        public UnitOfWork(NewsStandDbContext context)
        {
            _context = context;
            Authors = new AuthorRepository(context);
            AuthorProducts = new AuthorProductRepository(context);
            Customers = new CustomerRepository(context);
            Producers = new ProducerRepository(context);
            Products = new ProductRepository(context);
            Purchases = new PurchaseRepository(context);
            PurchaseProducts = new PurchaseProductRepository(context);
        }

        public IAuthorRepository Authors { get; private set; }
        public IAuthorProductRepository AuthorProducts { get; private set; }
        public ICustomerRepository Customers { get; private set; }
        public IProducerRepository Producers { get; private set; }
        public IProductRepository Products { get; private set; }
        public IPurchaseRepository Purchases { get; private set; }
        public IPurchaseProductRepository PurchaseProducts { get; private set; }

        public void Complete()
        {
            _context.SaveChanges();
        }
    }
}
