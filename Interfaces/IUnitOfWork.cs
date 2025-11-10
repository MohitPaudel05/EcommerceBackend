using System;
using System.Threading.Tasks;

namespace Ecommerce.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IProductRepository Products { get; }
        ICategoryRepository Categories { get; }
        Task SaveAsync();
    }
}
