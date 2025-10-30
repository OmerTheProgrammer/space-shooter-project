using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entitys
{
    public class Group : BaseEntity
    {
        private int groupScore = 0;

        public int GroupScore { get => groupScore; set => groupScore = value; }

        public override string ToString()
        {
            return $"{base.ToString()}, Groups score: {this.GroupScore}";
        }
    }
}
