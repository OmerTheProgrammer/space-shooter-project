using Model.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Data_Transfer_Objects
{
    /// <summary>
    /// Abstract base class for all Data Transfer Objects used for partial updates.
    /// It implements the IBaseDTO contract, centralizing the mandatory primary key (Idx) 
    /// and providing the parameterless constructor required for JSON deserialization.
    /// Derived classes (like AdminDTO or GroupDTO) will inherit these properties, 
    /// reducing boilerplate code.
    /// </summary>
    // requires generic entity and dto types to be specified
    public abstract class BaseDTO<TEntity, TDTO> : IBaseDTO
        //restrict TDTO to be a BaseDTO of TEntity and TDTO itself
        where TDTO : BaseDTO<TEntity, TDTO>,
        //restrict TEntity to be a BaseEntity or derived from it
        new() where TEntity : BaseEntity
        {
        /// <summary>
        /// The primary key ID of the record to be updated. This property satisfies the 
        /// IBaseDTO contract and is implemented once here.
        /// </summary>
        public int Idx { get; set; }

        /// <summary>
        /// Default constructor required for JSON deserialization by the client and server.
        /// </summary>
        public BaseDTO() { }

        /// <summary>
        /// Factory method: Creates and optionally configures a DTO from a single entity 
        /// for individual UPDATE operations.
        /// </summary>
        /// <param name="entity">The source entity containing the Idx.</param>
        /// <param name="configure">An optional action to set the properties intended for update (fluent configuration).</param>
        public static TDTO FromEntity(TEntity entity, Action<TDTO>? configure = null)
        {
            // Use the 'new TDTO()' constraint to create an instance of the derived class.
            var dto = new TDTO();
            dto.Idx = entity.Idx; // Inherited Idx is set from the entity

            // Execute the configuration lambda if provided
            configure?.Invoke(dto);

            return dto;
        }
    }
}
