using MagicShop.API.Interfaces;
using MagicShop.Entity;
using MagicShop.Entity.Models;

namespace MagicShop.API.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly MagicShopContext _magicShopContext;

        public ProductRepository(MagicShopContext magicShopContext)
        {
            this._magicShopContext = magicShopContext;
        }

        public async Task<Product> Add(Product product, CancellationToken cancellationToken)
        {
            await _magicShopContext.Products.AddAsync(product, cancellationToken);
            await _magicShopContext.SaveChangesAsync(cancellationToken);
            return product;
        }

        public ValueTask<Product?> GetById(int id, CancellationToken cancellationToken)
        {
            return _magicShopContext.Products.FindAsync(id, cancellationToken);
        }

        public async Task Remove(int id, CancellationToken cancellationToken)
        {
            var product = await _magicShopContext.Products.FindAsync(id, cancellationToken);
            if (product != null)
            {
                _magicShopContext.Products.Remove(product);
                await _magicShopContext.SaveChangesAsync(cancellationToken);
            }
        }

        public Task Update(Product product, CancellationToken cancellationToken)
        {
            _magicShopContext.Products.Attach(product).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

            return _magicShopContext.SaveChangesAsync(cancellationToken);
        }
    }
}
