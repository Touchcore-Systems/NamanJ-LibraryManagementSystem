using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LmsApi.Models
{
    public class Users
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UId { get; set; }
        public string UName { get; set; }  
        public string URole { get; set; }
        public string? UPass { get; set; }
    }
}
