using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entitys
{
    public class PlayerAndGroup : BaseEntity
    {
        private Player player;
        private Group group;

        public Player Player { get => player; set => player = value; }
        public Group Group { get => group; set => group = value; }

        public override string ToString()
        {
            return $"{base.ToString()} Group: {this.Group}\n" +
                $"Player: {this.Player}";
        }
    }
}
