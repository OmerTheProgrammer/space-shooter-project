using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entitys
{
    public enum Enemy
    {
        None = 0,
        space_ship = 1,//the basic green
        space_destroyer = 2, // red - not implemnted yet in game
        mini_boss = 3, // not implemnted yet in game
        boss = 4, // not implemnted yet in game
    }

    public class EnemyInLastLevel : BaseEntity
    {
        private RunInfo runInfo;
        private Enemy name = Enemy.None;//defult that must be changed
        private int amount = -1;//defult that must be changed

        public RunInfo RunInfo { get => runInfo; set => runInfo = value; }
        public Enemy Name { get => name; set => name = value; }
        public int Amount { get => amount; set => amount = value; }

        public override string ToString()
        {
            string output = $"{base.ToString()} In run: {this.RunInfo}\n";
            if (this.Amount == 1)
            {
                output += $"there is 1 Enemy {this.Name} ";
            }
            else
            {
                output += $"there are {this.Amount} Enemy {this.Name}s ";
            }
            return output;
        }
    }
}
