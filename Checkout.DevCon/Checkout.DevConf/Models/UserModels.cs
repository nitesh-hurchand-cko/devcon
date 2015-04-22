
namespace Checkout.DevCon.Models
{
    public class AddressModel
    {
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string CountryCode { get; set; } 
    }

    public class PhoneModel
    {
        public string CountryCode { get; set; }
        public string Number { get; set; }
    }

    public class CreateUserModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public PhoneModel HomePhone { get; set; }
        public PhoneModel MobilePhone { get; set; }
        public AddressModel ResidentialAddress { get; set; }
        public AddressModel WorkAddress { get; set; }
    }
}