using Model.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Tables
{
    public class PlayersAndGroupsTable : List<PlayerAndGroup>
    {
        public PlayersAndGroupsTable() { }
        public PlayersAndGroupsTable(IEnumerable<PlayerAndGroup> list) : base(list) { }
        public PlayersAndGroupsTable(IEnumerable<BaseEntity> list) : base(list.Cast<PlayerAndGroup>().ToList()) { }
    }
}
