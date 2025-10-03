using Model.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Tables
{
    public class ManagersTable : List<Manager>
    {
        public ManagersTable() { }
        public ManagersTable(IEnumerable<Manager> list) : base(list) { }
        public ManagersTable(IEnumerable<BaseEntity> list) : base(list.Cast<Manager>().ToList()) { }
    }
}
