using System.Security.Cryptography;
using System.Text;

namespace Client_Manager___API
{
    public static class TextHasher
    {
        public static string Hash(string rawText)
        {
            if (string.IsNullOrEmpty(rawText)) return "";

            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawText));// Convert string to a byte array

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