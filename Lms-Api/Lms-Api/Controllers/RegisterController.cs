using System.Data;
using System.Data.SqlClient;
using LmsAuthentication.Models;
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
        public JsonResult Post(RegisterModel reg)
        {
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("LmsAuthCon");
            SqlDataReader myReader;

            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                myConn.Open();

                using (SqlCommand cmd = new SqlCommand("RegisterStudent", myConn))
                {
                    try
                    {
                        cmd.Parameters.AddWithValue("@Username", reg.u_name);
                        cmd.Parameters.AddWithValue("@Password", LmsApi.HashPass.hashPass(reg.u_pass));
                        cmd.Parameters.AddWithValue("@Role", reg.u_role);

                        cmd.CommandType = CommandType.StoredProcedure;

                        myReader = cmd.ExecuteReader();
                        table.Load(myReader);
                        myReader.Close();
                        throw new Exception("hello");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Exception: ", ex);
                    }
                    finally
                    {
                        myConn.Close();
                    }
                }
            }
            return new JsonResult("Registered Successfully");
        }
    }
}
