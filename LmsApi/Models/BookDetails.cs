using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LmsApi.Models
{
    public class BookDetails
    {  
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BId { get; set; }
        public string BName { get; set; }
        public string BAuthor { get; set; }
        public int BQuantity { get; set; }
    }
}
