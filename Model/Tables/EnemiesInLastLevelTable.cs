using Model.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Tables
{
    public class EnemiesInLastLevelTableTable : List<EnemyInLastLevel>
    {
        public EnemiesInLastLevelTableTable() { }
        public EnemiesInLastLevelTableTable(IEnumerable<EnemyInLastLevel> list) : base(list) { }
        public EnemiesInLastLevelTableTable(IEnumerable<BaseEntity> list) : base(list.Cast<EnemyInLastLevel>().ToList()) { }
    }
}
