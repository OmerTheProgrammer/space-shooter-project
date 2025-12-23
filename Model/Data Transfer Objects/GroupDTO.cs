using Model.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Data_Transfer_Objects
{
    public class GroupDTO : BaseDTO<Group, GroupDTO>
    {
        // Value type MUST be explicitly nullable for partial updates
        public int? Score { get; set; } = null;
        public string? Name { get; set; }

        public override string ToString()
        {
            return $"{base.ToString()}, " +
                $"The Group {this.Name}'s score is {this.Score}";
        }
    }
}
