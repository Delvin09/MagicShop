using MagicShop.Entity.Models;

namespace MagicShop.API.Interfaces
{
    public interface IOrderRepository
    {
        Task<Order> Add(Order order, CancellationToken cancellationToken);

        ValueTask<Order?> GetById(int id, CancellationToken cancellationToken);

        Task Remove(int id, CancellationToken cancellationToken);

        Task Update(Order order, CancellationToken cancellationToken);
    }
}
