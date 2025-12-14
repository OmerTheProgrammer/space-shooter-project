using Microsoft.AspNetCore.Mvc;
using Model.Data_Transfer_Objects;
using Model.Entitys;
using Model.Tables;
using Server_Manager___API.Controllers;
using System;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Text.RegularExpressions;
using ViewModel;
using ViewModel.DBs;

namespace Server_Manager___API.Controllers
{

    // The route template is "api/Update/[ActionName]"
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UpdateController : Controller
    {
        /// <summary>
        /// Helper method to check if any non-Idx field was provided
        /// in a DTO of a BaseEntity derivative 
        /// AND if that provided value is different from the original value.
        /// </summary>
        private static List<List<string>>? CheckForInnerFieldChanges<T>(T source, T original) where T : BaseEntity
        {
            PropertyInfo[] properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            List<List<string>> allChanges = new List<List<string>>();

            foreach (PropertyInfo prop in properties)
            {
                // Skip Idx
                if (prop.Name == "Idx")
                {
                    continue;
                }

                //get values of the property from source and original
                object? sourceValue = prop.GetValue(source);
                object? originalValue = prop.GetValue(original);
                bool isValueProvided = false;
                Type propType = prop.PropertyType;

                if (sourceValue != null)
                {
                    // 1. Check for Nullable Value Types
                    // (e.g., int?, bool?, DateTime?)
                    if (propType.IsGenericType &&
                        propType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        //gets the HasValue property of the nullable type
                        var hasValue =
                            propType.GetProperty("HasValue")?
                            .GetValue(sourceValue, null);
                        //if HasValue is true, then the inner field was provided
                        //the HasValue is always a bool, but we check to be safe
                        if (hasValue is bool b && b)
                        {
                            isValueProvided = true; // Inner field provided
                        }
                    }
                    // 2. Check for non-nullables (Value types like int, bool, structs, and Reference types like string/classes)
                    else
                    {
                        isValueProvided = true; // Inner field provided (non-null)
                    }
                }

                // If a value was explicitly provided by the client, check if it differs from the original.
                if (isValueProvided)
                {
                    // A: Nested BaseEntity Derivative (Recursive Check) ---
                    // If the property is a complex class that inherits
                    // from BaseEntity (a nested DTO/Entity)
                    if (typeof(BaseEntity).IsAssignableFrom(propType)
                        && propType.IsClass && propType != typeof(string))
                    {
                        // 1. Find the CheckForInnerFieldChanges<T>
                        MethodInfo? genericMethod =
                            typeof(UpdateController).GetMethods(
                                BindingFlags.NonPublic | BindingFlags.Static
                                )
                            .Where(
                                m => m.Name == nameof(CheckForInnerFieldChanges)
                                && m.IsGenericMethodDefinition
                                && m.GetParameters().Length == 2)
                            .FirstOrDefault();

                        //if we found the method, we can invoke it
                        if (genericMethod != null)
                        {
                            // 2. create the func with the nested type
                            MethodInfo constructedMethod =
                                genericMethod.MakeGenericMethod(propType);

                            // 3. run the func with the nested source and original values
                            // The result is a List<List<string>>?
                            List<List<string>>? innerPaths =
                                (List<List<string>>?)constructedMethod.Invoke(
                                    null,//calling object doesn't exist = static
                                    new object?[]
                                    { sourceValue, originalValue }
                                    )!;

                            if (innerPaths != null && innerPaths.Any())
                            {
                                // Found changes deep inside. Prepend the current property name (prop.Name)
                                // to the beginning of every path and add to the collective list.
                                foreach (var path in innerPaths)
                                {
                                    path.Insert(0, prop.Name);
                                    allChanges.Add(path);
                                }
                            }
                        }
                    }
                    // --- Case B: Simple Types (Value Types and Strings) ---
                    else
                    {
                        // Unpack Nullable Value Types to their underlying value for comparison
                        object? comparableSource = sourceValue;

                        // Check if the property type is Nullable<T> (e.g., int?, DateTime?)
                        Type? underlyingType = Nullable.GetUnderlyingType(propType);

                        if (underlyingType != null)
                        {
                            // If it is Nullable<T>, extract the actual T value 
                            PropertyInfo? valueProperty = propType.GetProperty("Value");
                            comparableSource = valueProperty?.GetValue(sourceValue);
                        }

                        // Compare simple types/strings using string representation for reliable comparison of boxed value types.
                        if (!string.Equals(comparableSource?.ToString(), originalValue?.ToString()))
                        {
                            // Found change at this level. Add a new path list with only the property name.
                            allChanges.Add(new List<string> { prop.Name });
                        }
                    }
                }
            }
            return allChanges.Any() ? allChanges : null; // Return all changes or null
        }
        //private static List<string>? CheckForInnerFieldChanges<T>(T source, T original) where T : BaseEntity
        //{
        //    PropertyInfo[] properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        //    foreach (PropertyInfo prop in properties)
        //    {
        //        // Skip Idx
        //        if (prop.Name == "Idx")
        //        {
        //            continue;
        //        }

        //        //get values of the property from source and original
        //        object? sourceValue = prop.GetValue(source);
        //        object? originalValue = prop.GetValue(original);
        //        bool isValueProvided = false;
        //        Type propType = prop.PropertyType;

        //        if (sourceValue != null)
        //        {
        //            // 1. Check for Nullable Value Types
        //            // (e.g., int?, bool?, DateTime?)
        //            if (propType.IsGenericType &&
        //                propType.GetGenericTypeDefinition() == typeof(Nullable<>))
        //            {
        //                //gets the HasValue property of the nullable type
        //                var hasValue =
        //                    propType.GetProperty("HasValue")?
        //                    .GetValue(sourceValue, null);
        //                //if HasValue is true, then the inner field was provided
        //                //the HasValue is always a bool, but we check to be safe
        //                if (hasValue is bool b && b)
        //                {
        //                    isValueProvided = true; // Inner field provided
        //                }
        //            }
        //            ////2. Check for Value Types (int, bool, DateTime, enum)
        //            //else if (propType.IsGenericType)
        //            //{
        //            //    isValueProvided = true; // Inner field provided (non-null value type)
        //            //}

        //            // 2. Check for non-nullables )
        //            else // if (propType.IsClass)
        //            {
        //                isValueProvided = true; // Inner field provided (non-null)
        //            }
        //        }

        //        // If a value was explicitly provided by the client, check if it differs from the original.
        //        if (isValueProvided)
        //        {
        //            // A: Nested BaseEntity Derivative (Recursive Check) ---
        //            // If the property is a complex class that inherits
        //            // from BaseEntity (a nested DTO/Entity)
        //            if (typeof(BaseEntity).IsAssignableFrom(propType)
        //                && propType.IsClass && propType != typeof(string))
        //            {
        //                // 1. Find the CheckForInnerFieldChanges<T>
        //                // (T source, T original) func to invoke recursively
        //                // to recurse we need to change the T to the nested type
        //                // so we use reflection to get the generic method definition
        //                // by same name, is generic and has 2 parameters
        //                MethodInfo? genericMethod =
        //                    typeof(UpdateController).GetMethods(
        //                        BindingFlags.NonPublic | BindingFlags.Static
        //                        )
        //                    .Where(
        //                        m => m.Name == nameof(CheckForInnerFieldChanges)
        //                        && m.IsGenericMethodDefinition
        //                        && m.GetParameters().Length == 2)
        //                    .FirstOrDefault();

        //                //if we found the method, we can invoke it
        //                if (genericMethod != null)
        //                {
        //                    // 2. crate the func with the nested type
        //                    MethodInfo constructedMethod =
        //                        genericMethod.MakeGenericMethod(propType);

        //                    // 3. run the func with the nested source and original values
        //                    // The result is a bool indicating
        //                    // if an unauthorized inner change was detected.
        //                    List<string>? innerPath =
        //                        //run with null for static methods
        //                        //and send the parameters as object array with
        //                        //sourceValue, originalValue
        //                        (List<string>?)constructedMethod.Invoke(
        //                            null,//calling object doesn't exist = static
        //                            new object?[]
        //                            { sourceValue, originalValue }
        //                            )!;
        //                    //! -> don't make a null error compilar

        //                    //runs recursively, until it finds simple types
        //                    //or no changes
        //                    if (innerPath != null)
        //                    {
        //                        // Found change. add to start the current property name (prop.Name) and return.
        //                        innerPath.Insert(0, prop.Name);
        //                        return innerPath;
        //                    }
        //                }
        //            }
        //            // --- Case B: Simple Types (Value Types and Strings) ---
        //            else
        //            {
        //                // Unpack Nullable Value Types to their underlying value for comparison
        //                object? comparableSource = sourceValue;
        //                //if is Type? ->
        //                //change comparableSource to sourceValue.Value like needed
        //                Type? underlyingType = Nullable.GetUnderlyingType(propType);
        //                if (underlyingType != null)
        //                {
        //                    PropertyInfo? valueProperty =
        //                        propType.GetProperty("Value");
        //                    comparableSource = valueProperty?.GetValue(sourceValue);
        //                }

        //                // Compare simple types/strings. If they differ, it's an unauthorized change.
        //                if (!object.Equals(comparableSource, originalValue))
        //                {
        //                    return new List<string> { prop.Name }; // Found differing field in the calling object
        //                }
        //            }
        //        }
        //    }
        //    return null; // No differing non-Idx inner fields were provided
        //}

        /// <summary>
        /// Overload for string and classes.
        /// </summary>
        private static bool TryUpdateProperty<T>(T? source, Action<T> setter)
            where T : class //like string
        {
            // This handles strings and other nullable reference types
            if (source != null)
            {
                setter(source);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Overload for based on BaseEntity.
        /// </summary>
        private static bool TryUpdateProperty<T>(T? source, T? toChange, Action<T> setter)
           where T : BaseEntity
        {
            // handles classes derived from BaseEntity
            if (source != null && toChange != null)
            {
                List<List<string>>? deepChanges = CheckForInnerFieldChanges(source, toChange);

                //deepChanges.Any() - deepChanges is not empty
                if (deepChanges != null && deepChanges.Any())
                {
                    // --- Construct the Combined Error Message and Throw ---
                    List<string> errorDetails = new List<string>();
                    string rootEntityType = typeof(T).Name;
                    int rootEntityIdx = source.Idx;

                    foreach (var path in deepChanges)
                    {
                        // --- 1. Identify Innermost Entity and Index ---
                        // Note: Here 'source' is the DTO property we are checking (e.g., RunInfo), not the root update DTO.
                        // We start traversal from 'source' here.
                        BaseEntity? currentEntity = source;
                        int innermostIdx = source.Idx;
                        string innermostEntityTypeName = rootEntityType;

                        // Traverse path elements *before* the last one (the simple field name)
                        for (int i = 0; i < path.Count - 1; i++)
                        {
                            string propName = path[i];
                            PropertyInfo? prop = currentEntity?.GetType().GetProperty(propName);

                            if (prop != null)
                            {
                                BaseEntity? nextEntity = prop.GetValue(currentEntity) as BaseEntity;
                                if (nextEntity != null)
                                {
                                    currentEntity = nextEntity;
                                    innermostIdx = nextEntity.Idx;
                                    innermostEntityTypeName = nextEntity.GetType().Name;
                                }
                            }
                        }

                        // --- 2. Format Error Detail ---
                        string fullPath = string.Join("/", path);
                        string fieldName = path.Last();

                        // Format the error for this single change
                        string detail =
                            $"Change detected in nested entity '{innermostEntityTypeName}' (Idx: {innermostIdx}). " +
                            $"Path from current property: '{rootEntityType}/{fullPath}'. Field changed: '{fieldName}'.";

                        errorDetails.Add(detail);
                    }

                    // Combine all details into a single error message
                    string combinedDetails = string.Join("\n\t- ", errorDetails);

                    // Throw exception with combined error message.
                    string finalErrorMessage =
                        $"Invalid Update Attempt in Table/Entity '{rootEntityType}' (Idx: {rootEntityIdx}): " +
                        $"Multiple unauthorized deep changes were detected:\n\t- {combinedDetails}\n" +
                        $"To update these nested entity fields, you must use the separate update functions for the respective types.";

                    // Assuming ExpandedException is mapped to HTTP 422 by the middleware.
                    throw new ExpandedException(finalErrorMessage);
                }
                // --- END OF ERROR BLOCK ---

                if (source.Idx != toChange.Idx)
                {
                    return TryUpdateProperty(source, setter);
                }
            }
            return false;
        }

        //private static bool TryUpdateProperty<T>(T? source, T? toChange, Action<T> setter)
        //    where T : BaseEntity //like string
        //{
        //    // handles classes derived from BaseEntity
        //    if (source != null && toChange != null)
        //    {
        //        List<string> path = CheckForInnerFieldChanges(source, toChange);
        //        if (path != null)
        //        {
        //            string className = typeof(T).Name;
        //            // Throw exception because the user attempted to modify an inner field.
        //            // Create the path string: e.g., "RunInfo -> EnemyInLastLevel -> Task"
        //            // The path returned is innermost first, so we reverse and join.
        //            path.Reverse();
        //            string pathString = string.Join(" -> ", path.ToArray());

        //            // Throw exception because the user attempted to modify an inner field.
        //            string errorMessage =   
        //                $"Invalid Use of Update: Attempted to update fields of the nested entity '{className}' " +
        //                $"(Idx: {source.Idx}) during the update of the containing object. " +
        //                $"Unauthorized change detected at field path: '{pathString}'. " +
        //                $" To update the nested entity's fields, " +
        //                $"you must use the separate update function for {className}.";

        //            throw new ExpandedException(errorMessage);
        //        }

        //        if (source.Idx != toChange.Idx)
        //        {
        //            return TryUpdateProperty(source, setter);
        //        }
        //    }
        //    return false;
        //}

        /// <summary>
        /// Overload for Nullable Value Types (DateTime?, bool?).
        /// </summary>
        private static bool TryUpdateProperty<T>(T? source, Action<T> setter)
            where T : struct //like int, DateTime, bool and rest
        {
            // This handles DateTime?, bool?, int?, etc.
            if (source.HasValue)
            {
                setter(source.Value);
                return true;
            }
            return false;
        }

        //--- ADMIN UPDATE ---
        [HttpPut]
        [ActionName("AdminUpdator")]
        public IActionResult UpdateAdmin([FromBody] AdminDTO admin)
        {
            // NOTE: The entire try/catch block is removed.
            // Any exception (404 from SelectByIdx, or 409/400 from SaveChanges) 
            // will now bubble up to the ExceptionHandler middleware.

            AdminsDB adminsDB = new AdminsDB();

            // 1. Fetch current DB values. If not found, AdminsDB.SelectByIdx is expected 
            //    to throw an ExpandedException which the middleware handles as 404.
            Admin originalAdmin = AdminsDB.SelectByIdx(admin.Idx);

            bool isModified = false;

            // Check and update fields only if they are provided in the DTO
            // |= like += but for ||
            // 1. Strings (Nullable Reference Types)
            isModified |= TryUpdateProperty(admin.Id, val => originalAdmin.Id = val);
            isModified |= TryUpdateProperty(admin.Email, val => originalAdmin.Email = val);
            isModified |= TryUpdateProperty(admin.Password, val => originalAdmin.Password = val);
            isModified |= TryUpdateProperty(admin.Username, val => originalAdmin.Username = val);

            // 2. Nullable Value Types (DateTime?, bool?)
            isModified |= TryUpdateProperty(admin.StartDate, val => originalAdmin.StartDate = val);
            isModified |= TryUpdateProperty(admin.Birthday, val => originalAdmin.Birthday = val);
            isModified |= TryUpdateProperty(admin.IsLoggedIn, val => originalAdmin.IsLoggedIn = val);

            int changedRecords = 0;
            if (isModified)
            {
                adminsDB.Update(originalAdmin);

                // 2. SaveChanges will throw exceptions on DB constraint violations (Unique/Not Null/FK),
                //    which are handled by the ExceptionHandler middleware.
                changedRecords = adminsDB.SaveChanges();
            }

            if (changedRecords > 0)
            {
                // Success with changes: 200 OK.
                return StatusCode(200, $"OK: Record for Admin Idx=" +
                    $" {admin.Idx} successfully updated.\n" +
                    $" Records changed: {changedRecords}");
            }
            else //changedRecords == 0 -> no changes made
            {
                // Success with no changes: 200 OK with a specific message.
                return StatusCode(200, $"OK: Record for Admin Idx=" +
                    $"{admin.Idx} was not changed as the data was identical, " +
                    $"Records changed: {changedRecords}");
            }
        }

        //--- EnemyInLastLevel UPDATE ---
        [HttpPut]
        [ActionName("EnemyInLastLevelUpdator")]
        public IActionResult UpdateEnemyInLastLevel([FromBody] EnemyInLastLevelDTO enemyInLastLevel)
        {
            // NOTE: The entire try/catch block is removed.
            // Any exception (404 from SelectByIdx, or 409/400 from SaveChanges) 
            // will now bubble up to the ExceptionHandler middleware.

            EnemiesInLastLevelDB enemyInLastLevelsDB = new EnemiesInLastLevelDB();

            // 1. Fetch current DB values. If not found, enemyInLastLevelsDB.SelectByIdx is expected 
            //    to throw an ExpandedException which the middleware handles as 404.
            EnemyInLastLevel originalEnemyInLastLevel = EnemiesInLastLevelDB.SelectByIdx(enemyInLastLevel.Idx);

            bool isModified = false;

            // Check and update fields only if they are provided in the DTO
            // |= like += but for ||
            // 1. Strings (Nullable Reference Types)
            isModified |= TryUpdateProperty(enemyInLastLevel.RunInfo,
               originalEnemyInLastLevel.RunInfo,
                val => originalEnemyInLastLevel.RunInfo = val);

            // 2. Nullable Value Types (DateTime?, bool?)
            isModified |= TryUpdateProperty(enemyInLastLevel.Name, val => originalEnemyInLastLevel.Name = val);
            isModified |= TryUpdateProperty(enemyInLastLevel.Amount, val => originalEnemyInLastLevel.Amount = val);

            int changedRecords = 0;
            if (isModified)
            {
                enemyInLastLevelsDB.Update(originalEnemyInLastLevel);

                // 2. SaveChanges will throw exceptions on DB constraint violations (Unique/Not Null/FK),
                //    which are handled by the ExceptionHandler middleware.
                changedRecords = enemyInLastLevelsDB.SaveChanges();
            }

            if (changedRecords > 0)
            {
                // Success with changes: 200 OK.
                return StatusCode(200, $"OK: Record for EnemyInLastLevel Idx=" +
                    $" {enemyInLastLevel.Idx} successfully updated.\n" +
                    $" Records changed: {changedRecords}");
            }
            else //changedRecords == 0 -> no changes made
            {
                // Success with no changes: 200 OK with a specific message.
                return StatusCode(200, $"OK: Record for EnemyInLastLevel Idx=" +
                    $"{enemyInLastLevel.Idx} was not changed as the data was identical, " +
                    $"Records changed: {changedRecords}");
            }
        }


    }
}
