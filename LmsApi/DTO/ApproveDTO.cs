namespace LmsApi.DTO
{
    public class ApproveDTO
    {
        public string Status { get; set; } = "approved";
        public DateTime DateOfIssue { get; set; } = DateTime.Now;
        public DateTime DateOfSubmission { get; set; } = DateTime.Now.AddDays(7);
    }
}
