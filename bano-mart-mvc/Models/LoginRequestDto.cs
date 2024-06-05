using System.ComponentModel.DataAnnotations;

namespace bano_mart_mvc.Models
{
    public class LoginRequestDto
    {
        [Required]
        [Display(Name = "Username")]
        public string UserName {  get; set; }

        [Required]
        public string Password { get; set; }
    }
}
