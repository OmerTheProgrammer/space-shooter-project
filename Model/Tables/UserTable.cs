using Model.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Tables
{
    public class UserTable : List<User>
    {
        public UserTable() { }
        public UserTable(IEnumerable<User> list) : base(list) { }
        public UserTable(IEnumerable<BaseEntity> list) : base(list.Cast<User>().ToList()) { }
    }
}
