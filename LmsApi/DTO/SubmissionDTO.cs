namespace LmsApi.DTO
{
    public class SubmissionDTO
    {
        public string Status { get; set; } = "returned";
        public DateTime DateOfReturn { get; set; } = DateTime.Now;
        public int Fine { get; set; }
    }
}