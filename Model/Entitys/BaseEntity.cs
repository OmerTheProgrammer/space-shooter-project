using System.Text;

namespace Model.Entitys
{
    public class BaseEntity
    {
        //3 becouse a record exists in every table with idx=3
        //required for insert operation in inherited classes
        private int idx = 3;
        public int Idx { get => idx; set => idx = value; }

        public override string ToString()
        {
            return $"idx: {this.Idx}";
        }
    }
}
