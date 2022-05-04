using System.Data;
using System.Data.SqlClient;
using LmsAuthentication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LmsAuthentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApproveController : ControllerBase
    {
        //dependency injection
        private readonly IConfiguration _configuration;
        public ApproveController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet, Authorize(Roles = "admin")]

        public JsonResult Get()
        {
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("LmsAuthCon");
            SqlDataReader myReader;

            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                myConn.Open();

                using (SqlCommand myCommand = new SqlCommand("GetPendingBooks", myConn))
                {
                    myCommand.CommandType = CommandType.StoredProcedure;

                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myConn.Close();
                }
            }

            return new JsonResult(table);
        }

        [HttpPut, Authorize(Roles = "admin")]
        public JsonResult Put(ApproveModel approve)
        {
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("LmsAuthCon");
            SqlDataReader myReader;

            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                myConn.Open();

                using (SqlCommand cmd = new SqlCommand("UpdateApproveStatus", myConn))
                {
                    cmd.Parameters.AddWithValue("@Status", approve.status);
                    cmd.Parameters.AddWithValue("@Date_Of_Issue", approve.date_of_issue);
                    cmd.Parameters.AddWithValue("@Date_Of_Submission", approve.date_of_submission);
                    cmd.Parameters.AddWithValue("@Id", approve.t_id);

                    cmd.CommandType = CommandType.StoredProcedure;

                    myReader = cmd.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myConn.Close();
                }
            }

            return new JsonResult("Book approved!");
        }
    }
}
