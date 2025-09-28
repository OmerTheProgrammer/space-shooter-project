using System.Text;

namespace Model
{
    public class BaseEntity
    {
        private int idx = 3;// smallest defult value in all tables

        public int Idx { get => idx; set => idx = value; }

        public override string ToString()
        {
            return $"idx: {this.Idx}";
        }
    }
}
