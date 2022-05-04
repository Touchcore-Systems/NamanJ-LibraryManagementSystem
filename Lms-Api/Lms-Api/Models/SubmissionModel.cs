using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LmsAuthentication.Models
{
    public class SubmissionModel
    {
        public int t_id { get; set; }
        public string status { get; set; } = "returned";
        public DateTime date_of_return { get; set; }

        public int fine { get; set; }

    }
}
