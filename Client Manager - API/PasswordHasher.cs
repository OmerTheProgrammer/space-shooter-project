using System.Security.Cryptography;
using System.Text;

namespace Client_Manager___API
{
    public static class PasswordHasher
    {
        public static string Hash(string rawPassword)
        {
            if (string.IsNullOrEmpty(rawPassword)) return "";

            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawPassword));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}