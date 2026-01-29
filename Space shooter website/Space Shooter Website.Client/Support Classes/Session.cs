using Model.Entitys;

namespace Space_Shooter_Website.Client.Support_Classes
{
    public class Session
    {
        public User? CurrentUser { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsPlayer { get; set; }
        public bool IsLoggedIn => CurrentUser != null;

        public void Logout()
        {
            CurrentUser = null;
            IsAdmin = false;
            IsPlayer = false;
        }
    }
}