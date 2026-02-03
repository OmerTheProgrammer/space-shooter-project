using Model.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Data_Transfer_Objects
{
    public class AdminDTO : UserDTO<Admin, AdminDTO>
    {
        // Inherits Id, Password, Username, Email, Birthday from UserDTO

        public DateTime? StartDate { get; set; } = null;

        /// <summary>
        /// Strongly typed override to ensure the correct Entity type is returned.
        /// </summary>
        public override Admin ToEntity() => (Admin)base.ToEntity();

        public override string ToString()
        {
            return $"idx : {this.Idx} Start Date: {this.StartDate}";
        }
    }
}
