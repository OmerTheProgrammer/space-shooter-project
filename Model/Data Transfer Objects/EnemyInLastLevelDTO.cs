using Model.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Data_Transfer_Objects
{
    public class EnemyInLastLevelDTO
    {
        // Idx is always mandatory for finding the record
        public int Idx { get; set; }

        // Reference types (objects/strings) are naturally nullable
        public RunInfo? RunInfo { get; set; }

        // Value types MUST be explicitly nullable for partial updates
        public int? Amount { get; set; } = null;
        public Enemy? Name { get; set; } = null;

        public EnemyInLastLevelDTO(EnemyInLastLevel enemy)
        {
            this.Idx = enemy.Idx;

            // --- NEW DEFAULT VALUES ---
            const Enemy defaultEnemyName = Enemy.None;
            const int defaultAmount = -1;
            // --------------------------

            // RunInfo (Reference Type Check - Default is null/uninitialized)
            if (enemy.RunInfo != null)
            {
                this.RunInfo = enemy.RunInfo;
            }

            // Name (Value Type Check - Default is Enemy.None)
            if (enemy.Name != defaultEnemyName)
            {
                this.Name = enemy.Name;
            }

            // Amount (Value Type Check - Default is -1)
            if (enemy.Amount != defaultAmount)
            {
                this.Amount = enemy.Amount;
            }
        }
    }
}
