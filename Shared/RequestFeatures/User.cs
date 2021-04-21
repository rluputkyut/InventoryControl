using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryControl.Shared.RequestFeatures
{
    public partial class User
    {
        public int UserId { get; set; }
        public string EmailAddress { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Source { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public int PubId { get; set; }
        public DateTime? HireDate { get; set; }

        public Role Role { get; set; }
        public string ConfirmPassword { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }

    public class Role
    {
        public short RoleId { get; set; }
        public string RoleDesc { get; set; }
    }

    public class RefreshRequest
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
