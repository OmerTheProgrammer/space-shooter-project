using Model.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Data_Transfer_Objects
{
    // Data Transfer Object (DTO) for Admin entity supporting partial updates
    // includ fields that normally exist in Admin entity only as values 
    //becouse they not null in DB, but here they are
    //nullable to support partial updates
    public class PlayerDTO : UserDTO<Player, PlayerDTO>{
        public int? MaxLevel { get; set; } = null;
        public int? TotalScore { get; set; } = null;
        public bool? IsSoundOn { get; set; } = null;
        public bool? IsMusicOn { get; set; } = null;

        public override Player ToEntity() => (Player)base.ToEntity();

        public override string ToString()
        {
            return $"{base.ToString()}, " +
                $"Max level: {this.MaxLevel}, Total Score: {this.TotalScore}, " +
                $"Sound is {this.IsSoundOn}ly on, " +
                $"Music is {this.IsMusicOn}ly on";
        }
    }
}
