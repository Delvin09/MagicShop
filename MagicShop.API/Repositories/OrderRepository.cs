using MagicShop.API.Interfaces;
using MagicShop.Entity;
using MagicShop.Entity.Models;

namespace MagicShop.API.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly MagicShopContext _magicShopContext;

        public OrderRepository(MagicShopContext magicShopContext)
        {
            this._magicShopContext = magicShopContext;
        }

        public async Task<Order> Add(Order order, CancellationToken cancellationToken)
        {
            await _magicShopContext.Orders.AddAsync(order, cancellationToken);
            await _magicShopContext.SaveChangesAsync(cancellationToken);
            return order;
        }

        public ValueTask<Order?> GetById(int id, CancellationToken cancellationToken)
        {
            return _magicShopContext.Orders.FindAsync(id, cancellationToken);
        }

        public async Task Remove(int id, CancellationToken cancellationToken)
        {
            var order = await _magicShopContext.Orders.FindAsync(id, cancellationToken);
            if (order != null)
            {
                _magicShopContext.Orders.Remove(order);
                await _magicShopContext.SaveChangesAsync(cancellationToken);
            }
        }

        public Task Update(Order order, CancellationToken cancellationToken)
        {
            _magicShopContext.Orders.Attach(order).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

            return _magicShopContext.SaveChangesAsync(cancellationToken);
        }
    }
}
