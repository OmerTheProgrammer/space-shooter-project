using Model.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Tables
{
    public class UsersTable : List<User>
    {
        public UsersTable() { }
        public UsersTable(IEnumerable<User> list) : base(list) { }
        public UsersTable(IEnumerable<BaseEntity> list) : base(list.Cast<User>().ToList()) { }
    }
}
