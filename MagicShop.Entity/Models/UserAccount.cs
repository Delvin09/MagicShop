namespace MagicShop.Entity.Models
{
    public class UserAccount : BaseEntity
    {
        public string Login { get; set; }

        public byte[] PasswordHash { get; set; }

        public byte[] PasswordSalt { get; set; }

        public UserRole Role { get; set; }
    }
}
