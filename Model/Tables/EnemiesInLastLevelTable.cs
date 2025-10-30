using Model.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Tables
{
    public class EnemiesInLastLevelTable : List<EnemyInLastLevel>
    {
        public EnemiesInLastLevelTable() { }
        public EnemiesInLastLevelTable(IEnumerable<EnemyInLastLevel> list) : base(list) { }
        public EnemiesInLastLevelTable(IEnumerable<BaseEntity> list) : base(list.Cast<EnemyInLastLevel>().ToList()) { }
    }
}
