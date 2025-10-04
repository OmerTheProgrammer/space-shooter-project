using System.Text;

namespace Model.Entitys
{
    public class BaseEntity
    {
        private int idx = 3;//fixes itself in 1:1
        public int Idx { get => idx; set => idx = value; }

        public override string ToString()
        {
            return $"idx: {this.Idx}";
        }
    }
}
