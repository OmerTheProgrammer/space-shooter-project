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
        public int? GroupScore { get; set; } = null;
        public string? Name;

    }
}
