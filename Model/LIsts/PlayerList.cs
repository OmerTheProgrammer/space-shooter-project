using Model.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.LIsts
{
    public class PLayerList : List<PLayer>
    {
        public PLayerList() { }
        public PLayerList(IEnumerable<PLayer> list) : base(list) { }
        public PLayerList(IEnumerable<BaseEntity> list) : base(list.Cast<PLayer>().ToList()) { }
    }
}
