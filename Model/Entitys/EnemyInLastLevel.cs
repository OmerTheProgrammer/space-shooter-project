using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entitys
{
    public enum Enemy
    {
        space_ship=0,//the basic green
        space_destroyer=1, // red - not implemnted yet
        mini_boss=2, // not implemnted yet
        boss=3, // not implemnted yet
    }

    public class EnemyInLastLevel : BaseEntity
    {
        private RunInfo runInfo;
        private Enemy name;
        private int Amount;

        public override string ToString()
        {
            string output = $"{base.ToString()} In run: {this.runInfo}.\n";
            if (this.Amount == 1){
                output += $"there is 1 Enemy {this.name} ";
            }
            else
            {
                output += $"there are {this.Amount} Enemy {this.name}s ";
            }
            return output;
        }
    }
}
