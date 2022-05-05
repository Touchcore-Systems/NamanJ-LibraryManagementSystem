using System.ComponentModel.DataAnnotations;

namespace Lms_Api.DTO
{
    public class RegisterDTO
    {
        [Required(ErrorMessage =("Username is required!"))]
        public string u_name { get; set; }
        [Required(ErrorMessage ="Password is required!")]
        public string u_pass { get; set; }
        [Required(ErrorMessage ="Role is required!")]
        public string u_role { get; set; }
    }
}
