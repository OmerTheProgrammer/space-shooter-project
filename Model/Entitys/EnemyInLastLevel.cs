using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entitys
{
    public enum Enemy
    {
        space_ship = 0,//the basic green
        space_destroyer = 1, // red - not implemnted yet in game
        mini_boss = 2, // not implemnted yet in game
        boss = 3, // not implemnted yet in game
    }

    public class EnemyInLastLevel : BaseEntity
    {
        private RunInfo runInfo;
        private Enemy name;
        private int Amount;

        public RunInfo RunInfo { get => runInfo; set => runInfo = value; }
        public Enemy Name { get => name; set => name = value; }
        public int Amount1 { get => Amount; set => Amount = value; }

        public override string ToString()
        {
            string output = $"{base.ToString()} In run: {this.RunInfo}.\n";
            if (this.Amount1 == 1)
            {
                output += $"there is 1 Enemy {this.Name} ";
            }
            else
            {
                output += $"there are {this.Amount1} Enemy {this.Name}s ";
            }
            return output;
        }
    }
}
