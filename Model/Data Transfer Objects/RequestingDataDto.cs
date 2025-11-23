using Model.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Model.Data_Transfer_Objects
{
    // Data Transfer Object (DTO) for Admin entity supporting partial updates
    // includ fields that normally exist in Admin entity only as values 
    //becouse they not null in DB, but here they are
    //nullable to support partial updates
    public class RequestingDataDto : BaseDTO<RequestingData, RequestingDataDto>
    {
        public ProfileEditRequest? Request { get; set; } = null;
        public string? Field { get; set; } = null;
        public string? OldValue { get; set; } = null;
        public string? NewValue { get; set; } = null;
    }
}
