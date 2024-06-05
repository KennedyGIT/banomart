using System.ComponentModel.DataAnnotations;

namespace bano_mart_mvc.Models
{
    public class RegistrationDto
    {

        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Required]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Username")]
        public string UserName { get; set; }
        public string Role { get; set; }
    }
}

