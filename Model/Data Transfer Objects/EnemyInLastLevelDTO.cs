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
    {
        // Idx is always mandatory for finding the record
        public int Idx { get; set; }

        // Reference types (objects/strings) are naturally nullable
        public RunInfo? RunInfo { get; set; }

        // Value types MUST be explicitly nullable for partial updates
        public int? Amount { get; set; } = null;

        // Assuming Enemy is an enum, it must be nullable (Enemy?)
        public Enemy? Name { get; set; } = null;

        /// <summary>
        /// Parameterless constructor for deserialization and static factory use.
        /// </summary>
        public EnemyInLastLevelDTO() { }

        /// <summary>
        /// Factory method to easily create a DTO from a full EnemyInLastLevel entity,
        /// marking all fields as NOT to be updated initially (they are all null).
        /// </summary>
        public static EnemyInLastLevelDTO FromEntity(EnemyInLastLevel enemy)
        {
            // We only copy the Index (Idx) which is mandatory for the update operation.
            // All other properties remain null to signal to the server they shouldn't be touched.
            return new EnemyInLastLevelDTO
            {
                Idx = enemy.Idx,
            };
        }
    }
}
