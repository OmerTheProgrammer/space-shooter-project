using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entitys
{
    public class Group : BaseEntity
    {
        private int groupsScore;
        private int leadboardPos;

        public int GroupsScore { get => groupsScore; set => groupsScore = value; }
        public int LeadboardPos { get => leadboardPos; set => leadboardPos = value; }

        public override string ToString()
        {
            return $"{base.ToString()} Groups score: {this.GroupsScore}.\n" +
                $"Leadboard position: {this.LeadboardPos}";
        }
    }
}
