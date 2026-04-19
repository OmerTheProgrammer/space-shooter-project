using Client_Manager___API;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Model.Data_Transfer_Objects;
using Model.Entitys;
using System.Threading.Tasks;

namespace Space_Shooter_Website.Client.Support_Classes
{
    public class TableConfig
    {
        public Type EntityType { get; set; } = typeof(BaseEntity);
        public Type DtoType { get; set; } = null!;
        public Func<Task<IEnumerable<object>>> GetAll { get; set; } = null!;
        public Func<int, Task<BaseEntity?>> GetById { get; set; } = null!;
        public Func<int, Task<(int rows, string? error)>> Delete { get; set; } = null!;
        public Func<BaseEntity, Task<(int rows, string? error)>> Insert { get; set; } = null!;
        public Func<IBaseDTO, Task<(int rows, string? error)>> Update { get; set; } = null!;
    }
}
