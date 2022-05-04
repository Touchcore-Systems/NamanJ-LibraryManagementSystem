using System.Data;
using System.Data.SqlClient;
using LmsAuthentication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace LmsAuthentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubmissionController : ControllerBase
    {
        //dependency injection
        private readonly IConfiguration _configuration;
        public SubmissionController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet, Authorize(Roles = "admin, student")]
        public JsonResult Get()
        {
            string u_name = User.Identity.Name;

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("LmsAuthCon");
            SqlDataReader myReader;

            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                myConn.Open();

                using (SqlCommand cmd = new SqlCommand("GetSubmissionDetails", myConn))
                {
                    cmd.Parameters.AddWithValue("@Username", u_name);

                    cmd.CommandType = CommandType.StoredProcedure;

                    myReader = cmd.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myConn.Close();
                }
            }
            return new JsonResult(table);
        }

        [HttpPut]
        public JsonResult Put(SubmissionModel sub)
        {
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("LmsAuthCon");
            SqlDataReader myReader;

            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                myConn.Open();

                using (SqlCommand cmd = new SqlCommand("UpdateSubmissionDetails", myConn))
                {
                    cmd.Parameters.AddWithValue("@Status", sub.status);
                    cmd.Parameters.AddWithValue("@Date_Of_Return", TimeZoneInfo.ConvertTimeFromUtc(sub.date_of_return, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time")));
                    cmd.Parameters.AddWithValue("@Fine", sub.fine);
                    cmd.Parameters.AddWithValue("@Id", sub.t_id);

                    cmd.CommandType = CommandType.StoredProcedure;

                    myReader = cmd.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myConn.Close();
                }
            }

            return new JsonResult("Book submitted!");
        }
    }
}