using Model.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Tables
{
    public class PLayerTable : List<PLayer>
    {
        public PLayerTable() { }
        public PLayerTable(IEnumerable<PLayer> list) : base(list) { }
        public PLayerTable(IEnumerable<BaseEntity> list) : base(list.Cast<PLayer>().ToList()) { }
    }
}
