using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicShop.Models
{
    [Flags]
    public enum RoleType
    {
        None = 0,
        Admin = 1,
        Vendor = 2,
        Customer = 4
    }

    public interface IUserModel
    {
        int Id { get; set; }

        string Login { get; set; }

        string Email { get; set; }
    }

    public interface ICustomerModel : IUserModel
    {
        string Address { get; set; }

        string PhoneNumber { get; set; }

        string LastName { get; set; }

        string Name { get; set; }
    }

    public class AccountModel : IUserModel
    {
        public int Id { get; set; }
        public string Login { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public RoleType Role { get; set; }
        public string Password { get; set; } = string.Empty;
    }

    public class CustomerAccountModel : AccountModel, ICustomerModel
    {
        public string Address { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;
    }
}
