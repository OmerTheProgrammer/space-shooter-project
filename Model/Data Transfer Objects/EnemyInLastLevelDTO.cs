using Model.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Data_Transfer_Objects
{
    // NOTE: Assuming EnemyInLastLevel, RunInfo, and Enemy are defined elsewhere.

    // Data Transfer Object (DTO) for EnemyInLastLevel entity supporting partial updates.
    public class EnemyInLastLevelDTO
        : BaseDTO<EnemyInLastLevel, EnemyInLastLevelDTO>
    {

        // Reference types (objects/strings) are naturally nullable
        public RunInfoDTO? RunInfo { get; set; }

        // Value types MUST be explicitly nullable for partial updates
        public int? Amount { get; set; } = null;

        // Assuming Enemy is an enum, it must be nullable (Enemy?)
        public Enemy? Name { get; set; } = null;

        public override string ToString()
        {
            string output = $"{base.ToString()} In run: {this.RunInfo}\n";
            if (this.Amount == 1)
            {
                output += $"there is 1 Enemy {this.Name} ";
            }
            else
            {
                output += $"there are {this.Amount} Enemy {this.Name}s ";
            }
            return output;
        }
    }
}
