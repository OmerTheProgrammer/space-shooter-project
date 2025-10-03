using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entitys
{
    enum Enemy
    {
        space_ship,//the basic green
        space_destroyer, // red - not implemnted yet
        mini_boss, // not implemnted yet
        boss, // not implemnted yet
    }

    public class EnemyInLastLevel : BaseEntity
    {
        private RunInfo runInfo;
        private Enemy name;
        private int Amount;

        public override string ToString()
        {
            return $"{base.ToString()} In run: {this.runInfo}.\n" +
                $"there are {this.Amount} Enemy {this.name} ";
        }
    }
}
