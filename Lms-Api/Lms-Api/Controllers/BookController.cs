using System.Data;
using System.Data.SqlClient;
using LibraryManagementSystemAPI.Models;
using Lms_Api.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        //dependency injection
        private readonly IConfiguration _configuration;

        public BookController(IConfiguration configuration)
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

                using (SqlCommand cmd = new SqlCommand("GetBookDetails", myConn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    myReader = cmd.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myConn.Close();
                }
            }

            return new JsonResult(table);
        }

        [HttpPost, Authorize(Roles = "admin")]
        public JsonResult Post(BookDTO book)
        {
            DataTable table = new DataTable();
            SqlDataReader myReader;
            string sqlDataSource = _configuration.GetConnectionString("LmsAuthCon");
            string res = "";

            SqlConnection myConn = new SqlConnection(sqlDataSource);
            SqlCommand cmd = new SqlCommand("AddBook", myConn);
            try
            {
                myConn.Open();

                cmd.Parameters.AddWithValue("@Name", book.b_name);
                cmd.Parameters.AddWithValue("@Author", book.b_author);
                cmd.Parameters.AddWithValue("@Quantity", book.b_quantity);

                cmd.CommandType = CommandType.StoredProcedure;

                myReader = cmd.ExecuteReader();
                table.Load(myReader);
                myReader.Close();

                res = "Added Successfully!";
            }
            catch (Exception e)
            {
                res = e.Message;
            }
            finally
            {
                myConn.Close();
            }

            return new JsonResult(res);
        }

        [HttpPut, Authorize(Roles = "admin, student")]
        public JsonResult Put(BookModel book)
        {
            string proc = string.Empty;
            if (User.IsInRole("admin"))
            {
                proc = "UpdateBook";
            }
            else
            {
                proc = "UpdateBookQuantity";
            }

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("LmsAuthCon");
            SqlDataReader myReader;

            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                myConn.Open();

                using (SqlCommand cmd = new SqlCommand(proc, myConn))
                {
                    if (User.IsInRole("admin"))
                    {
                        cmd.Parameters.AddWithValue("@Name", book.b_name);
                        cmd.Parameters.AddWithValue("@Author", book.b_author);
                        cmd.Parameters.AddWithValue("@Quantity", book.b_quantity);
                        cmd.Parameters.AddWithValue("@Id", book.b_id);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@Quantity", book.b_quantity);
                        cmd.Parameters.AddWithValue("@Id", book.b_id);
                    }

                    cmd.CommandType = CommandType.StoredProcedure;

                    myReader = cmd.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myConn.Close();
                }
            }

            return new JsonResult("Updated Successfully");
        }

        [HttpDelete("{id}"), Authorize(Roles = "admin")]
        public JsonResult Delete(int id)
        {
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("LmsAuthCon");
            SqlDataReader myReader;

            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                myConn.Open();

                using (SqlCommand cmd = new SqlCommand("DeleteBook", myConn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);

                    cmd.CommandType = CommandType.StoredProcedure;

                    myReader = cmd.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myConn.Close();
                }
            }

            return new JsonResult("Deleted Successfully");
        }

    }
}
