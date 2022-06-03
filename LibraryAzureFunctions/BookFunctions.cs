using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;
using LmsApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace LibraryAzureFunctions
{
    public static class BookFunctions
    {
        [FunctionName("BookFunctions")]
        public static async Task<IActionResult> GetBooks(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "getbooks")] HttpRequest req,
            ILogger log)
        {
            DataTable table = new();
            SqlDataReader myReader;
            string sqlDataSource = string.Empty;

            using (SqlConnection myConn = new(sqlDataSource))
            {
                using (SqlCommand cmd = new("GetBookDetails", myConn))
                {
                    try
                    {
                        sqlDataSource = Environment.GetEnvironmentVariable("MyConnectionString");

                        await myConn.OpenAsync();

                        cmd.CommandType = CommandType.StoredProcedure;
                        myReader = await cmd.ExecuteReaderAsync();
                        table.Load(myReader);

                        myReader.Close();

                        log.LogInformation("Book details fetched");
                        return new OkObjectResult(table);
                    }
                    catch (Exception ex)
                    {
                        log.LogInformation(ex.ToString());
                        return new BadRequestObjectResult(ex.Message);
                    }
                    finally
                    {
                        myConn.Close();
                    }
                }
            }
        }

        [FunctionName("AddBook")]
        public static async Task<IActionResult> AddBook(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "addbook")] HttpRequest req, ILogger log)
        {
            string sqlDataSource = string.Empty;
            string reqBody = string.Empty;
            BookDetails value;

            using (SqlConnection myConn = new(sqlDataSource))
            {
                using (SqlCommand cmd = new("AddBook", myConn))
                {
                    try
                    {
                        sqlDataSource = Environment.GetEnvironmentVariable("MyConnectionString");
                        reqBody = await new StreamReader(req.Body).ReadToEndAsync();
                        value = JsonConvert.DeserializeObject<BookDetails>(reqBody);

                        await myConn.OpenAsync();

                        cmd.Parameters.AddWithValue("Name", value.BName);
                        cmd.Parameters.AddWithValue("Author", value.BAuthor);
                        cmd.Parameters.AddWithValue("Quantity", value.BQuantity);

                        cmd.CommandType = CommandType.StoredProcedure;

                        await cmd.ExecuteNonQueryAsync();

                        log.LogInformation("Book added");
                        return new OkObjectResult("Book added");
                    }
                    catch (Exception ex)
                    {
                        log.LogError(ex.ToString());
                        return new BadRequestObjectResult(ex.Message);
                    }
                    finally
                    {
                        myConn.Close();
                    }
                }
            }
        }

        [FunctionName("UpdateBook")]
        public static async Task<IActionResult> UpdateBook(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = "updatebook")] HttpRequest req, ILogger log)
        {
            string sqlDataSource = string.Empty;
            string reqBody = string.Empty;
            BookDetails value;

            using (SqlConnection myConn = new(sqlDataSource))
            {
                using (SqlCommand cmd = new("UpdateBook", myConn))
                {
                    try
                    {
                        sqlDataSource = Environment.GetEnvironmentVariable("MyConnectionString");
                        reqBody = await new StreamReader(req.Body).ReadToEndAsync();
                        value = JsonConvert.DeserializeObject<BookDetails>(reqBody);

                        await myConn.OpenAsync();

                        cmd.Parameters.AddWithValue("Id", value.BId);
                        cmd.Parameters.AddWithValue("Name", value.BName);
                        cmd.Parameters.AddWithValue("Author", value.BAuthor);
                        cmd.Parameters.AddWithValue("Quantity", value.BQuantity);

                        cmd.CommandType = CommandType.StoredProcedure;

                        await cmd.ExecuteNonQueryAsync();

                        log.LogInformation("Book details update");
                        return new OkObjectResult("Book details updated");
                    }
                    catch (Exception ex)
                    {
                        log.LogError(ex.ToString());
                        return new BadRequestObjectResult(ex.Message);
                    }
                    finally
                    {
                        myConn.Close();
                    }
                }
            }
        }

        [FunctionName("DeleteBook")]
        public static async Task<IActionResult> DeleteBook(
            [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "deletebook")] HttpRequest req, ILogger log)
        {
            string sqlDataSource = string.Empty;
            string reqBody = string.Empty;
            BookDetails value;

            using (SqlConnection myConn = new(sqlDataSource))
            {
                using (SqlCommand cmd = new("DeleteBook", myConn))
                {
                    try
                    {
                        sqlDataSource = Environment.GetEnvironmentVariable("MyConnectionString");
                        reqBody = await new StreamReader(req.Body).ReadToEndAsync();
                        value = JsonConvert.DeserializeObject<BookDetails>(reqBody);

                        await myConn.OpenAsync();

                        cmd.Parameters.AddWithValue("Id", value.BId);

                        cmd.CommandType = CommandType.StoredProcedure;

                        await cmd.ExecuteNonQueryAsync();

                        log.LogInformation("Book deleted");
                        return new OkObjectResult("Book deleted");
                    }
                    catch (Exception ex)
                    {
                        log.LogError(ex.ToString());
                        return new BadRequestObjectResult(ex.Message);
                    }
                    finally
                    {
                        myConn.Close();
                    }
                }
            }
        }
    }
}

