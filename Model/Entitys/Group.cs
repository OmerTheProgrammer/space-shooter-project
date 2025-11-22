using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entitys
{
    public class Group : BaseEntity
    {
        private int score = -1;//defult that must be changed
        private string name = "";//defult that must be changed

        public int Score { get => score; set => score = value; }
        public string Name { get => name; set => name = value; }

        public override string ToString()
        {
            return $"{base.ToString()},The Group {this.Name}'s score is {this.Score}";
        }
    }
}
