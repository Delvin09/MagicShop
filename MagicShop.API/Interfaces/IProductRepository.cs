using MagicShop.Entity.Models;

namespace MagicShop.API.Interfaces
{
    public interface IProductRepository
    {
        Task<Product> Add(Product product, CancellationToken cancellationToken);

        ValueTask<Product?> GetById(int id, CancellationToken cancellationToken);

        Task Remove(int id, CancellationToken cancellationToken);

        Task Update(Product product, CancellationToken cancellationToken);
    }
}
