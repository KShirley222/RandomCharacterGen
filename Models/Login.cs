using System.ComponentModel.DataAnnotations;

namespace CharacterGenerator.Models
{
    public class Login
    {
        [Required]
        [EmailAddress]
        [Display(Name="Email")]
        public string LoginEmail { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name="Password")]
        public string LoginPassword { get; set; }
    }
}