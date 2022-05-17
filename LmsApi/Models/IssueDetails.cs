using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LmsApi.Models
{
    public class IssueDetails
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TId { get; set; }
        public string? UName { get; set; }
        public int? BId { get; set; }
        public string? Status { get; set; }
        public DateTime? DateOfIssue { get; set; }
        public DateTime? DateofSubmission { get; set; }
        public DateTime? DateOfReturn { get; set; }
        public int? Fine { get; set; }
    }
}
