using System.Data;
using System.Data.SqlClient;
using Lms_Api.LogRecord;
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
            LogRecord record = new LogRecord();
            DataTable table = new DataTable();
            SqlDataReader myReader;

            string u_name = User.Identity.Name;
            string res = string.Empty;
            string sqlDataSource = _configuration.GetConnectionString("LmsAuthCon");

            SqlConnection myConn = new SqlConnection(sqlDataSource);
            SqlCommand cmd = new SqlCommand("GetSubmissionDetails", myConn);

            try
            {
                cmd.Parameters.AddWithValue("@Username", u_name);

                cmd.CommandType = CommandType.StoredProcedure;

                myReader = cmd.ExecuteReader();
                table.Load(myReader);

                myReader.Close();

                res = "Books issued by user " + u_name + " fetched";
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
            return new JsonResult(table);
        }

        [HttpPut]
        public JsonResult Put(SubmissionModel sub)
        {
            LogRecord record = new LogRecord();
            DataTable table = new DataTable();
            SqlDataReader myReader;

            string sqlDataSource = _configuration.GetConnectionString("LmsAuthCon");
            string res = string.Empty;

            SqlConnection myConn = new SqlConnection(sqlDataSource);
            SqlCommand cmd = new SqlCommand("UpdateSubmissionDetails", myConn);

            try
            {
                myConn.Open();

                cmd.Parameters.AddWithValue("@Status", sub.status);
                cmd.Parameters.AddWithValue("@Date_Of_Return", TimeZoneInfo.ConvertTimeFromUtc(sub.date_of_return, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time")));
                cmd.Parameters.AddWithValue("@Fine", sub.fine);
                cmd.Parameters.AddWithValue("@Id", sub.t_id);

                cmd.CommandType = CommandType.StoredProcedure;

                myReader = cmd.ExecuteReader();
                table.Load(myReader);

                myReader.Close();

                res = "Book with T. Id: " + sub.t_id + " submitted";
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