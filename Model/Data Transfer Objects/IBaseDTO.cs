using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Data_Transfer_Objects
{
    public interface IBaseDTO
    {
        /// <summary>
        /// The primary key ID of the record to be updated. This is mandatory.
        /// </summary>
        int Idx { get; set; }
    }
}
