using Model.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Tables
{
    public class RequestsDataTable : List<RequestingData>
    {
        public RequestsDataTable() { }
        public RequestsDataTable(IEnumerable<RequestingData> list) : base(list) { }
        public RequestsDataTable(IEnumerable<BaseEntity> list) : base(list.Cast<RequestingData>().ToList()) { }
    }
}
