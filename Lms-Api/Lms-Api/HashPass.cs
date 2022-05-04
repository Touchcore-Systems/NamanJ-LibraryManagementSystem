using System.Text;
using Tweetinvi.Security;

namespace LmsApi
{
    public class HashPass
    {
        public static string hashPass(string pass)
        {
            SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider();

            byte[] pass_bytes = Encoding.ASCII.GetBytes(pass);
            byte[] encrypted_pass = sha1.ComputeHash(pass_bytes);

            return Convert.ToBase64String(encrypted_pass);
        }
    }
}
