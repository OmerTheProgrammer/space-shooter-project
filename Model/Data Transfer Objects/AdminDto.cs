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

        public AdminDTO()
        {

        }

        public AdminDTO(Admin admin, bool IsLoggedIn)
        {
            this.Idx = admin.Idx;

            DateTime defaultDate = new DateTime(1753, 1, 1, 12, 0, 0);

            // String (Reference) Types Check (Default is "")
            if (!string.IsNullOrEmpty(admin.Id)) { this.Id = admin.Id; }
            if (!string.IsNullOrEmpty(admin.Password)) { this.Password = admin.Password; }
            if (!string.IsNullOrEmpty(admin.Username)) { this.Username = admin.Username; }
            if (!string.IsNullOrEmpty(admin.Email)) { this.Email = admin.Email; }

            //user set value, defult is vaild value
            this.IsLoggedIn = IsLoggedIn;

            // Date (Value Types Check - Default is 1753-01-01)
            if (admin.Birthday.HasValue && admin.Birthday.Value != defaultDate)
            {
                this.Birthday = admin.Birthday;
            }
            if (admin.StartDate.HasValue && admin.StartDate.Value != defaultDate)
            {
                this.StartDate = admin.StartDate;
            }
        }
    }
}
