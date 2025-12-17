using Model.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
        new() where TEntity : BaseEntity, new()
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
            //the ? runs: if (configure != null) { configure.Invoke(dto); }
            configure?.Invoke(dto);

            return dto;
        }

        /// <summary>
        /// Converts the DTO to an Entity. 
        /// Works for:
        /// 1. Regular fields (Strings, Ints).
        /// 2. Inheritance (e.g., Admin : User).
        /// 3. Composition (Nested DTOs).
        /// </summary>
        public virtual TEntity ToEntity()
        {
            // Create the specific entity instance (e.g., an Admin)
            TEntity entity = new TEntity { Idx = this.Idx };

            // Get all properties including inherited ones (e.g., properties from UserDTO inside AdminDTO)
            PropertyInfo[] dtoProps = this.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            PropertyInfo[] entityProps = typeof(TEntity).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo dtoProp in dtoProps)
            {
                if (dtoProp.Name == "Idx") continue;

                object? value = dtoProp.GetValue(this);
                if (value == null) continue; // Skip fields not provided by the client

                // Find the matching property in the target Entity (or its base classes)
                PropertyInfo? entityProp = entityProps.FirstOrDefault(p => p.Name == dtoProp.Name);
                if (entityProp == null || !entityProp.CanWrite) continue;

                Type dtoType = dtoProp.PropertyType;

                // Case A: Nullable Value Types (int?, bool?, DateTime?)
                if (dtoType.IsGenericType && dtoType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    var hasValue = dtoType.GetProperty("HasValue")?.GetValue(value);
                    if (hasValue is bool b && b)
                    {
                        object? innerValue = dtoType.GetProperty("Value")?.GetValue(value);
                        entityProp.SetValue(entity, innerValue);
                    }
                }
                // Case B: Composition (Nested DTOs) - e.g., RunInfoDTO inside EnemyDTO
                else if (value is IBaseDTO nestedDTO)
                {
                    // Call ToEntity recursively on the nested DTO
                    MethodInfo? toEntityMethod = nestedDTO.GetType().GetMethod("ToEntity");
                    if (toEntityMethod != null)
                    {
                        object? nestedEntity = toEntityMethod.Invoke(nestedDTO, null);
                        entityProp.SetValue(entity, nestedEntity);
                    }
                }
                // Case C: Standard Fields and Inheritance
                // (Matches string -> string, or any types that are directly assignable)
                else if (entityProp.PropertyType.IsAssignableFrom(dtoType))
                {
                    entityProp.SetValue(entity, value);
                }
            }

            return entity;
        }

        public override string ToString()
        {
            return $"idx : {this.Idx}";
        }
    }
}
