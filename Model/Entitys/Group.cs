using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entitys
{
    public class Group : BaseEntity
    {
        private int score = 0;
        private string name = "";

        public int Score { get => score; set => score = value; }
        public string Name { get => name; set => name = value; }

        public override string ToString()
        {
            return $"{base.ToString()},The Group {this.Name}'s score is {this.Score}";
        }
    }
}
