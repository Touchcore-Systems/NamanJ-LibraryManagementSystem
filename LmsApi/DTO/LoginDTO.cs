using System.ComponentModel.DataAnnotations;

namespace LmsApi.DTO
{
    public class LoginDTO
    {
        [Required]
        public string UName { get; set; }
        [Required]
        public string UPass { get; set; }
        [Required]
        public string URole { get; set; }
    }
}
