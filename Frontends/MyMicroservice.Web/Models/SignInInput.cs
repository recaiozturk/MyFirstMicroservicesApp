using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace MyMicroservice.Web.Models
{
    public class SignInInput
    {
        [Display(Name ="Email adresiniz")]
        public string Email { get; set; }

        [Display(Name = "Şifreniz")]
        public string Password { get; set; }

        [Display(Name = "Beni hatırla")]
        public bool IsRememberMe { get; set; }
    }
}
