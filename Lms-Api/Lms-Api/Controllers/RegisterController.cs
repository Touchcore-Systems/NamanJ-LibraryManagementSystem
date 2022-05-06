using System.Data;
using System.Data.SqlClient;
using Lms_Api.DTO;
using Lms_Api;
using Microsoft.AspNetCore.Mvc;

namespace LmsAuthentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        //dependency injection
        private readonly IConfiguration _configuration;

        public RegisterController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        public JsonResult Post(RegisterDTO reg)
        {
            LogRecord record = new LogRecord();

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("LmsAuthCon");
            SqlDataReader myReader;
            string res = "";

            SqlConnection myConn = new SqlConnection(sqlDataSource);

            try
            {
                myConn.Open();

                SqlCommand cmd = new SqlCommand("RegisterStudent", myConn);

                cmd.Parameters.AddWithValue("@Username", reg.u_name);
                cmd.Parameters.AddWithValue("@Password", LmsApi.HashPass.hashPass(reg.u_pass));
                cmd.Parameters.AddWithValue("@Role", reg.u_role);

                cmd.CommandType = CommandType.StoredProcedure;

                myReader = cmd.ExecuteReader();
                table.Load(myReader);
                myReader.Close();

                res = "User "+reg.u_name+" registered successfully";
            }
            catch (Exception e)
            {
                res = e.Message;
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
