namespace MagicShop.Entity.Models
{
    public class Customer : BaseEntity
    {
        public UserAccount UserAccount { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string? Email { get; set; }

        public string? Phone { get; set; }
    }
}
