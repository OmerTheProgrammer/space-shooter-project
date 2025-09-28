using System;

namespace Model.Entitys
{
    public class User : BaseEntity
    {
        private string id = "";
        private string password = "";
        private string username = "";
        private DateTime birthday;
        private string email = "";
        private bool isLoggedIn = false;

        public string Id { get => id; set => id = value; }
        public string Password { get => password; set => password = value; }
        public string Username { get => username; set => username = value; }
        public DateTime Birthday { get => birthday; set => birthday = value; }
        public string Email { get => email; set => email = value; }
        public bool IsLoggedIn { get => isLoggedIn; set => isLoggedIn = value; }

        public override string ToString()
        {
            return $"{base.ToString()}, " +
                $" {Username}, ID: {Id}, " +
                $"With password Hased: {Password}, " +
                $"With Birthday: {Birthday}, " +
                $"Is {isLoggedIn}ly logged in, " +
                $"email: {Email}.\n";
        }
    }
}
