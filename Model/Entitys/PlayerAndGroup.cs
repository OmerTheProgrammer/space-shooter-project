using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entitys
{
    public class PlayerAndGroup : BaseEntity
    {
        private PLayer player;
        private Group group;

        public override string ToString()
        {
            return $"{base.ToString()} Group: {this.group}.\n" +
                $"Player: {this.player}";
        }
    }
}
