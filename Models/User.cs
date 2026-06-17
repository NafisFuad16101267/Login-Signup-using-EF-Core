using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CoreAppPlayGround.Models
{
    public partial class User
    {
        public int Id { get; set; }

        [Required]
        [DisplayName("Name")]
        public string EmpName { get; set; } = null!;

        [Required]
        public string Gender { get; set; } = null!;

        [Required]
        public int? Age { get; set; }

        [Required(ErrorMessage = "Email is required. Please provide and email address.")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; } = null!;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;
    }
}
