using System.Data;
using System.Data.SqlClient;
using Lms_Api;
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
            LogRecord record = new LogRecord();
            DataTable table = new DataTable();
            SqlDataReader myReader;

            string sqlDataSource = _configuration.GetConnectionString("LmsAuthCon");
            string res = string.Empty;

            SqlConnection myConn = new SqlConnection(sqlDataSource);
            SqlCommand myCommand = new SqlCommand("GetIssueDetails", myConn);

            try
            {
                myConn.Open();

                myCommand.CommandType = CommandType.StoredProcedure;

                myReader = myCommand.ExecuteReader();
                table.Load(myReader);

                myReader.Close();

                res = "Issue details fetched";
            }
            catch (Exception ex)
            {
                res = ex.Message;
            }
            finally
            {
                myConn.Close();
                record.LogWriter(res);
            }
            return new JsonResult(table);
        }

        [HttpPost, Authorize(Roles = "admin, student")]

        public JsonResult Post(IssueModel issue)
        {
            LogRecord record = new LogRecord();
            DataTable table = new DataTable();
            SqlDataReader myReader;

            string sqlDataSource = _configuration.GetConnectionString("LmsAuthCon");
            string res = string.Empty;

            SqlConnection myConn = new SqlConnection(sqlDataSource);
            SqlCommand cmd = new SqlCommand("InsertIssueDetails", myConn);

            try
            {
                myConn.Open();

                cmd.Parameters.AddWithValue("@Username", issue.u_name);
                cmd.Parameters.AddWithValue("@Status", issue.status);
                cmd.Parameters.AddWithValue("@Id", issue.b_id);
                cmd.CommandType = CommandType.StoredProcedure;

                myReader = cmd.ExecuteReader();
                table.Load(myReader);

                myReader.Close();
                res = "Book with B.Id: " + issue.b_id + " requested";
            }
            catch (Exception ex)
            {
                res = ex.Message;
            }
            finally
            {
                myConn.Close();
                record.LogWriter(res);
            }
            return new JsonResult(res);
        }
    }
}
