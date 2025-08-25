using System.ComponentModel.DataAnnotations.Schema;

namespace Talabat.Core.Domain_Models.Identity
{
    public class Address
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        [ForeignKey(nameof(User))]
        public string userID { get; set; }
        public ApplicationUser User { get; set; }
    }
}