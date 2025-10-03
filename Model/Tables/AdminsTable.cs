using Model.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Tables
{
    public class AdminsTable : List<Admin>
    {
        public AdminsTable() { }
        public AdminsTable(IEnumerable<Admin> list) : base(list) { }
        public AdminsTable(IEnumerable<BaseEntity> list) : base(list.Cast<Admin>().ToList()) { }
    }
}
