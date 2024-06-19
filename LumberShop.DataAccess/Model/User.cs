using LumberStoreSystem.DataAccess.Model.Enummerations;
using System.Numerics;

namespace LumberStoreSystem.DataAccess.Model
{

    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public UserRole Role { get; set; }
        public Address Address { get; set; }
        public int AddressId { get; set; }
    }
}
