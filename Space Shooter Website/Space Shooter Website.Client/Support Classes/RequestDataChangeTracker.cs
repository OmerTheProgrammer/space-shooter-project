using Model.Entitys;

namespace Space_Shooter_Website.Client.Support_Classes
{
    public class RequestDataChangeTracker
    {
        public RequestData Data { get; set; } = new();
        public bool IsApproved { get; set; } = true;
    }
}
