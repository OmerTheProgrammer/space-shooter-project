using Model.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Tables
{
    public class RunsInfoTable : List<RunInfo>
    {
        public RunsInfoTable() { }
        public RunsInfoTable(IEnumerable<RunInfo> list) : base(list) { }
        public RunsInfoTable(IEnumerable<BaseEntity> list) : base(list.Cast<RunInfo>().ToList()) { }
    }
}
