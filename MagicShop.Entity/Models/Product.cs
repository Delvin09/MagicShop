namespace MagicShop.Entity.Models
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }

        public string? Description { get; set; }

        public decimal Price { get; set; }

        public int Count { get; set; }
    }
}
