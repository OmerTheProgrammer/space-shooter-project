using Model.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Model.Data_Transfer_Objects
{
    // Data Transfer Object (DTO) for Admin entity supporting partial updates
    // includ fields that normally exist in Admin entity only as values 
    //becouse they not null in DB, but here they are
    //nullable to support partial updates
    public class PlayerAndGroupDTO : BaseDTO<PlayerAndGroup, PlayerAndGroupDTO>
    {
        // Reference types (objects/strings) are naturally nullable
        public PlayerDTO? Player { get; set; }
        public GroupDTO? Group { get; set; }
    }
}
