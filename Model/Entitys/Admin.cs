using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entitys
{
    public class Admin : User
    {
        private DateTime? startDate = new DateTime(1753, 1, 1, 12, 0, 0);

        public DateTime? StartDate { get => startDate; set => startDate = value; }

        public override string ToString()
        {
            return $"{base.ToString()} Start Date: {this.StartDate}";
        }
    }
}
