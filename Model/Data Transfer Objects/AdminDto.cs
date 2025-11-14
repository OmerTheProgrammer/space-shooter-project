using Model.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Data_Transfer_Objects
{
    // Data Transfer Object (DTO) for Admin entity supporting partial updates
    // includ fields that normally exist in Admin entity only as values 
    //becouse they not null in DB, but here they are
    //nullable to support partial updates
    public class AdminDTO
    {
        // Idx is always mandatory for finding the record
        public int Idx { get; set; }

        // Reference types (strings) are nullable by default (null if omitted from JSON)
        public string? Id { get; set; }
        public string? Password { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }

        // Value types MUST be explicitly nullable to support partial updates
        public bool? IsLoggedIn { get; set; } = null;
        public DateTime? Birthday { get; set; } = null;
        public DateTime? StartDate { get; set; } = null;
    }
}
