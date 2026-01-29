using Model.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Data_Transfer_Objects
{
    /// <summary>
    ///  version for when you want to use UserDTO directly (non-inherited).
    /// </summary>
    public class UserDTO : UserDTO<User, UserDTO>{
        //copies all from UserDTO<TEntity, TDTO> (below)
    }
    /// <summary>
    /// version for when you want to use UserDTO for inherite.
    /// </summary>
    public class UserDTO<TEntity, TDTO> : BaseDTO<TEntity, TDTO>
        where TEntity : User, new()
        where TDTO : UserDTO<TEntity, TDTO>, new()
    {
        public string? Id { get; set; }
        public string? Password { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }

        public DateTime? Birthday { get; set; } = null;

        public override string ToString()
        {
            return $"{base.ToString()}, " +
                $" {this.Username}, ID: {this.Id}, " +
                $"With password Hased: {this.Password},\n " +
                $"With Birthday: {this.Birthday}, " +
                $"email: {this.Email}";
        }
    }
}
