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
                $" {this.Username}, ID: {this.Id}, " +
                $"With password Hased: {this.Password}, " +
                $"With Birthday: {this.Birthday}, " +
                $"Is {this.isLoggedIn}ly logged in, " +
                $"email: {this.Email}.\n";
        }
    }
}
