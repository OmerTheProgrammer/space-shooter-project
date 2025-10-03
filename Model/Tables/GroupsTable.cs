using Model.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Tables
{
    public class GroupsTable : List<Group>
    {
        public GroupsTable() { }
        public GroupsTable(IEnumerable<Group> list) : base(list) { }
        public GroupsTable(IEnumerable<BaseEntity> list) : base(list.Cast<Group>().ToList()) { }
    }
}
