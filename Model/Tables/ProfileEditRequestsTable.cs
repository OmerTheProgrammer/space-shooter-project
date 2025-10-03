using Model.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Tables
{
    public class ProfileEditRequestsTable : List<ProfileEditRequest>
    {
        public ProfileEditRequestsTable() { }
        public ProfileEditRequestsTable(IEnumerable<ProfileEditRequest> list) : base(list) { }
        public ProfileEditRequestsTable(IEnumerable<BaseEntity> list) : base(list.Cast<ProfileEditRequest>().ToList()) { }
    }
}
