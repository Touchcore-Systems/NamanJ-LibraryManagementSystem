using System.Reflection;

namespace Lms_Api.LogRecord
{
    public class LogRecord
    {
        private string path = string.Empty;
        public void LogWriter(string logMessage)
        {
            LogWrite(logMessage);
        }
        public void LogWrite(string logMessage)
        {
            path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Console.WriteLine(path);
            try
            {
                using (StreamWriter w = File.AppendText(path + "\\" + "log.txt"))
                {
                    Log(logMessage, w);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public void Log(string logMessage, TextWriter txtWriter)
        {
            try
            {
                txtWriter.Write("\r\nLog Entry : ");
                txtWriter.WriteLine(DateTime.Now.ToLongTimeString(),
                    DateTime.Now.ToLongDateString(), ":");
                txtWriter.WriteLine(logMessage);
                txtWriter.WriteLine("-------------------------------");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
