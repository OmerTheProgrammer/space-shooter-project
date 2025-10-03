using Model.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Tables
{
    public class RequestsDataTable : List<RequestData>
    {
        public RequestsDataTable() { }
        public RequestsDataTable(IEnumerable<RequestData> list) : base(list) { }
        public RequestsDataTable(IEnumerable<BaseEntity> list) : base(list.Cast<RequestData>().ToList()) { }
    }
}
