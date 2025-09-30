using Model.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Tables
{
    public class ManagerTable : List<Manager>
    {
        public ManagerTable() { }
        public ManagerTable(IEnumerable<Manager> list) : base(list) { }
        public ManagerTable(IEnumerable<BaseEntity> list) : base(list.Cast<Manager>().ToList()) { }
    }
}
