using System.Data;
using System.Data.SqlClient;
using LmsAuthentication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LmsAuthentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class IssueController : ControllerBase
    {
        //dependency injection
        private readonly IConfiguration _configuration;
        public IssueController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet, Authorize(Roles = "admin, student")]

        public JsonResult Get()
        {
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("LmsAuthCon");
            SqlDataReader myReader;

            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                myConn.Open();

                using (SqlCommand myCommand = new SqlCommand("GetIssueDetails", myConn))
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

        [HttpPost, Authorize(Roles = "admin, student")]

        public JsonResult Post(IssueModel issue)
        {
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("LmsAuthCon");
            SqlDataReader myReader;

            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                myConn.Open();

                using (SqlCommand cmd = new SqlCommand("InsertIssueDetails", myConn))
                {
                    cmd.Parameters.AddWithValue("@Username", issue.u_name);
                    cmd.Parameters.AddWithValue("@Status", issue.status);
                    cmd.Parameters.AddWithValue("@Id", issue.b_id);
                    cmd.CommandType = CommandType.StoredProcedure;

                    myReader = cmd.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myConn.Close();
                }
            }

            return new JsonResult("Requested Successfully");
        }
    }
}
