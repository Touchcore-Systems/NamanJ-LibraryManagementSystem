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
            LogRecord record = new LogRecord();
            DataTable table = new DataTable();
            SqlDataReader myReader;

            string sqlDataSource = _configuration.GetConnectionString("LmsAuthCon");
            string res = "";

            SqlConnection myConn = new SqlConnection(sqlDataSource);
            SqlCommand myCommand = new SqlCommand("GetPendingBooks", myConn);

            try
            {
                myConn.Open();
                myCommand.CommandType = CommandType.StoredProcedure;

                myReader = myCommand.ExecuteReader();
                table.Load(myReader);

                myReader.Close();
                res = "Books to approve fetched!";
            }
            catch (Exception e)
            {
                res = e.Message;
            }
            finally
            {
                myConn.Close();
                record.LogWriter(res);
            }

            return new JsonResult(table);
        }

        [HttpPut, Authorize(Roles = "admin")]
        public JsonResult Put(ApproveModel approve)
        {
            LogRecord record = new LogRecord();
            DataTable table = new DataTable();
            SqlDataReader myReader;

            string sqlDataSource = _configuration.GetConnectionString("LmsAuthCon");
            string res = string.Empty;

            SqlConnection myConn = new SqlConnection(sqlDataSource);
            SqlCommand cmd = new SqlCommand("UpdateApproveStatus", myConn);

            try
            {
                myConn.Open();

                cmd.Parameters.AddWithValue("@Status", approve.status);
                cmd.Parameters.AddWithValue("@Date_Of_Issue", approve.date_of_issue);
                cmd.Parameters.AddWithValue("@Date_Of_Submission", approve.date_of_submission);
                cmd.Parameters.AddWithValue("@Id", approve.t_id);

                cmd.CommandType = CommandType.StoredProcedure;

                myReader = cmd.ExecuteReader();
                table.Load(myReader);

                myReader.Close();
                res = "Book Status Updated Successfully!";
            }
            catch (Exception ex)
            {
                res = ex.Message;
            }
            finally
            {
                myConn.Close();
                record.LogWrite(res);
            }

            return new JsonResult(res);
        }
    }
}
