using Model.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Tables
{
    public class PlayersTable : List<PLayer>
    {
        public PlayersTable() { }
        public PlayersTable(IEnumerable<PLayer> list) : base(list) { }
        public PlayersTable(IEnumerable<BaseEntity> list) : base(list.Cast<PLayer>().ToList()) { }
    }
}
