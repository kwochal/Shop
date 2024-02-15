using System.ComponentModel.DataAnnotations;

namespace Lab10.ViewModels
{
    public class OrderViewModel
    {

        public string FirstName { get; set; }
        public string LastName { get; set; }
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        [RegularExpression(@"^\d{2}-\d{3}$")]
        public string ZipCode { get; set; }
        public string Payment { get; set; }
        
    }
}
