using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class ManagerList : List<Manager>
    {
        public ManagerList() { }
        public ManagerList(IEnumerable<Manager> list) : base(list) { }
        public ManagerList(IEnumerable<BaseEntity> list) : base(list.Cast<Manager>().ToList()) { }
    }
}
