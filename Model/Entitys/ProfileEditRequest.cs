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
        private DateTime? requestDate = new DateTime(1753, 1, 1, 12, 0, 0);
        private Status status = 0;
        private DateTime? reviewDate = new DateTime(1753, 1, 1, 12, 0, 0);
        private Admin adressingAdmin;

        public Player RequestingPlayer { get => requestingPlayer; set => requestingPlayer = value; }
        public DateTime? RequestDate { get => requestDate; set => requestDate = value; }
        public DateTime? ReviewDate { get => reviewDate; set => reviewDate = value; }
        public Admin AdressingAdmin { get => adressingAdmin; set => adressingAdmin = value; }
        public Status Status { get => status; set => status = value; }

        public override string ToString()
        {
            return $"{base.ToString()}, " +
                $"Requesting Player: {this.RequestingPlayer},\n" +
                $"Request Date: {this.RequestDate}, " +
                $"Review Date: {this.ReviewDate}, " +
                $"Adressing Admin: {this.AdressingAdmin},\n " +
                $"Status: {this.Status}\n";
        }
    }
}
