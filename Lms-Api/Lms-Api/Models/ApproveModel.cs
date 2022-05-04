namespace LmsAuthentication.Models
{
    public class ApproveModel
    {
        public int t_id { get; set; }
        public DateTime date_of_issue { get; set; } = DateTime.Now;
        public DateTime date_of_submission { get; set; } = DateTime.Now.AddDays(7);
        public string status { get; set; } = "approved";

    }
}
