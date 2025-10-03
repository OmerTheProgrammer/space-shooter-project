using System;

namespace Model.Entitys
{
    enum Status
    {
        Pending=0,
        Approved=1,
        Rejected=2,
        Canceled=3
    }

    public class ProfileEditRequest : BaseEntity
    {
        private Player requestingPlayer;
        private DateTime requestDate;
        private Status status;
        private DateTime reviewDate;
        private Admin adressingAdmin;

        public Player RequestingPlayer { get => requestingPlayer; set => requestingPlayer = value; }
        public DateTime RequestDate { get => requestDate; set => requestDate = value; }
        public DateTime ReviewDate { get => reviewDate; set => reviewDate = value; }
        public Admin AdressingAdmin { get => adressingAdmin; set => adressingAdmin = value; }
        internal Status Status { get => status; set => status = value; }

        public override string ToString()
        {
            return $"{base.ToString()}, " +
                $"Requesting Player: {this.RequestingPlayer},\n" +
                $"Request Date: {this.RequestDate}, " +
                $"Review Date: {this.ReviewDate}, " +
                $"Adressing Admin: {this.AdressingAdmin}, " +
                $"Status: {this.Status}.\n";
        }
    }
}
