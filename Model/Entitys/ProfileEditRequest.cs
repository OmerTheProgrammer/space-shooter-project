using System;

namespace Model.Entitys
{
    public enum Status
    {
        Pending=0,
        Approved=1,
        Rejected=2,
        Canceled=3
    }

    public class ProfileEditRequest : BaseEntity
    {
        private Player requestingPlayer;
        private DateTime requestingDate = new DateTime(1753, 1, 1, 12, 0, 0);
        private Status status = 0;
        private DateTime? reviewingDate = null; //defult becouse is nullable
        private Admin? adressingAdmin;

        public Player RequestingPlayer { get => requestingPlayer; set => requestingPlayer = value; }
        public DateTime RequestingDate { get => requestingDate; set => requestingDate = value; }
        public DateTime? ReviewingDate { get => reviewingDate; set => reviewingDate = value; }
        public Admin? AdressingAdmin { get => adressingAdmin; set => adressingAdmin = value; }
        public Status Status { get => status; set => status = value; }

        public override string ToString()
        {
            return $"{base.ToString()}, " +
                $"Requesting Player: {this.RequestingPlayer},\n" +
                $"Request Date: {this.RequestingDate}, " +
                $"Review Date: {this.ReviewingDate}, " +
                $"Adressing Admin: {this.AdressingAdmin},\n" +
                $"Status: {this.Status}";
        }
    }
}
