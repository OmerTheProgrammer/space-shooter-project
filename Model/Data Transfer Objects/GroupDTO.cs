using Model.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Data_Transfer_Objects
{
    public class GroupDTO
    {
        // Idx is always mandatory (inherited from BaseEntity)
        public int Idx { get; set; }

        // Value type MUST be explicitly nullable for partial updates
        public int? GroupScore { get; set; } = null;

        public GroupDTO(Group group)
        {
            this.Idx = group.Idx;

            // Define default value used in the Group entity
            const int defaultGroupScore = -1;

            // GroupScore (Value Type Check - Default is 0)
            // Only assign if the score is NOT the default value.
            if (group.GroupScore != defaultGroupScore)
            {
                this.GroupScore = group.GroupScore;
            }
        }
    }
}
