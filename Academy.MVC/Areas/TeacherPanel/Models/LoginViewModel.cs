using System.ComponentModel.DataAnnotations;

namespace Academy.MVC.Areas.TeacherPanel.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Username is required")]
        [Display(Name = "Username")]
        public string UserName { get; set; } = null!;

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; } = null!;

        public bool RememberMe { get; set; }
    }
}
