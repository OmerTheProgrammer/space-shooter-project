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
        // Idx is always mandatory for finding the record
        public int Idx { get; set; }

        // Value type MUST be explicitly nullable for partial updates
        public int? GroupScore { get; set; } = null;

        /// <summary>
        /// Default constructor required for JSON deserialization.
        /// </summary>
        public GroupDTO() { }

        /// <summary>
        /// Factory method: Creates a DTO from a single Group entity for individual UPDATE operations.
        /// Only copies the mandatory Idx. All other fields are null by default.
        /// </summary>
        public static GroupDTO FromEntity(Group group)
        {
            return new GroupDTO { Idx = group.Idx };
        }
    }
}
