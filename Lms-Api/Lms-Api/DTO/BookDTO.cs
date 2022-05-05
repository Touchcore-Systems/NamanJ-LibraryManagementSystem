using System.ComponentModel.DataAnnotations;

namespace Lms_Api.DTO
{
    public class BookDTO
    {
        public int b_id { get; set; }
        [Required (ErrorMessage ="Book name is required!")]
        public string b_name { get; set; }
        [Required(ErrorMessage = "Author name is required!")]
        public string b_author { get; set; }
        [Required(ErrorMessage = "Quantity is required!")]
        public int b_quantity { get; set; }
    }
}
