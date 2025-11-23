using Model.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Data_Transfer_Objects
{
    // Data Transfer Object (DTO) for Admin entity supporting partial updates
    // includ fields that normally exist in Admin entity only as values 
    //becouse they not null in DB, but here they are
    //nullable to support partial updates
    public class RunInfoDto : BaseDTO<RunInfo, RunInfoDto>
    {
        // Core data properties (matching the original RunInfo entity)
        public int? CurrentScore { get; set; } = null;
        public int? CurrentLevel { get; set; } = null;
        public int? CurrentShieldLevel { get; set; } = null;
        public int? CurrentBlasterCount { get; set; } = null;
        public int? CurrentHp { get; set; } = null;
        public bool? IsRunOver { get; set; } = null;
        public DateTime? RunStopDate { get; set; } = null;
        public Player? Player { get; set; } = null;
    }
}
