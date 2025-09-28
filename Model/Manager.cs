using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Manager : User
    {
        private DateTime startDate;

        public DateTime StartDate { get => startDate; set => startDate = value; }

        public override string ToString()
        {
            return $"{base.ToString()} Start Date: {this.StartDate}.\n";
        }
    }
}
