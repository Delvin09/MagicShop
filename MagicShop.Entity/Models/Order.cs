namespace MagicShop.Entity.Models
{
    public class Order : BaseEntity
    {
        public Customer Customer { get; set; }

        public List<Product> Products { get; set; }
    }
}
