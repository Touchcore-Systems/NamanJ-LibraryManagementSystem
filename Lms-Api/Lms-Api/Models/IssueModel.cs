namespace LmsAuthentication.Models
{
    public class IssueModel
    {
        public int t_id { get; set; }
        public string u_name { get; set; }

        public int b_id { get; set; }
        public string b_name { get; set; }
        public string b_author { get; set; }

        public string status { get; set; }
        public int fine { get; set; }
    }
}
