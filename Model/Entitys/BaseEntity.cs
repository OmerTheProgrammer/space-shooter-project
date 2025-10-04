using System.Text;

namespace Model.Entitys
{
    public class BaseEntity
    {
        private int idx;
        public int Idx { get => idx; set => idx = value; }

        public override string ToString()
        {
            return $"idx: {this.Idx}";
        }
    }
}
