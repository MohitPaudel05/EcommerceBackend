using Ecommerce.Datas;
using Ecommerce.Interfaces;
using Ecommerce.Repositories;
using System.Threading.Tasks;

namespace Ecommerce.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public IProductRepository Products { get; }
        public ICategoryRepository Categories { get; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Products = new ProductRepository(_context);
            Categories = new CategoryRepository(_context);
        }

        public async Task SaveAsync() => await _context.SaveChangesAsync();

        public void Dispose() => _context.Dispose();
    }
}
