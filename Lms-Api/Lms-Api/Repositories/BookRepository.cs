using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;

namespace Lms_Api.Repositories
{
    public class BookRepository
    {
        //dependency injection
        private readonly IConfiguration _configuration;

        public BookRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public DataTable GetBooks()
        {
            LogRecord record = new LogRecord();
            DataTable table = new DataTable();
            SqlDataReader myReader;

            string sqlDataSource = _configuration.GetConnectionString("LmsAuthCon");
            string res = string.Empty;

            SqlConnection myConn = new SqlConnection(sqlDataSource);
            SqlCommand cmd = new SqlCommand("GetBookDetails", myConn);

            try
            {
                myConn.Open();

                cmd.CommandType = CommandType.StoredProcedure;
                myReader = cmd.ExecuteReader();
                table.Load(myReader);

                myReader.Close();

                res = "Available books fetched and passed to controller";
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
            return table;
        }
    }
}
