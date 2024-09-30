namespace MagicShop.Entity.Models
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }

        public DateTimeOffset CreateDate { get; set; }

        public DateTimeOffset UpdateDate { get; set; }

        public bool IsDeleted { get; set; }
    }
}
