using System.ComponentModel.DataAnnotations;

namespace LmsApi.DTO
{
    public class BookDTO
    {
        [Required]
        public string BName { get; set; }
        [Required]
        public string BAuthor { get; set; }
        [Required]
        public int BQuantity { get; set; }
    }
}
