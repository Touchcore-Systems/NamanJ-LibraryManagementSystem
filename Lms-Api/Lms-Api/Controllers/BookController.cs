using System.Data;
using System.Data.SqlClient;
using LibraryManagementSystemAPI.Models;
using Lms_Api.DTO;
using Lms_Api;
using Lms_Api.Repositories;
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
            LogRecord record = new LogRecord();
            BookRepository bookRepository = new BookRepository(_configuration);
            DataTable result = null;

            string res = string.Empty;

            try
            {
                result = bookRepository.GetBooks();
                res = "Data recieved at controller and result returned";
            }
            catch (Exception ex)
            {
                res = ex.Message;
            }
            finally
            {
                record.LogWriter(res);
            }
            return new JsonResult(result);
        }

        [HttpPost, Authorize(Roles = "admin")]
        public JsonResult Post(BookDTO book)
        {
            LogRecord record = new LogRecord();
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

                res = "Book " + book.b_name + " is added to collection";
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

            LogRecord record = new LogRecord();
            DataTable table = new DataTable();
            SqlDataReader myReader;

            string sqlDataSource = _configuration.GetConnectionString("LmsAuthCon");
            string res = string.Empty;

            SqlConnection myConn = new SqlConnection(sqlDataSource);
            SqlCommand cmd = new SqlCommand(proc, myConn);

            try
            {
                myConn.Open();

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
                res = "Book " + book.b_id + " updated";
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

        [HttpDelete("{id}"), Authorize(Roles = "admin")]
        public JsonResult Delete(int id)
        {
            DataTable table = new DataTable();
            SqlDataReader myReader;

            string sqlDataSource = _configuration.GetConnectionString("LmsAuthCon");
            string res = string.Empty;

            SqlConnection myConn = new SqlConnection(sqlDataSource);
            SqlCommand cmd = new SqlCommand("DeleteBook", myConn);

            try
            {
                myConn.Open();

                cmd.Parameters.AddWithValue("@Id", id);
                cmd.CommandType = CommandType.StoredProcedure;

                myReader = cmd.ExecuteReader();
                table.Load(myReader);
                myReader.Close();

                res = "Book with B.Id: " + id + " deleted";
            }
            catch (Exception ex)
            {
                res = ex.Message;
            }
            finally
            {
                myConn.Close();
            }
            return new JsonResult(res);
        }
    }
}
