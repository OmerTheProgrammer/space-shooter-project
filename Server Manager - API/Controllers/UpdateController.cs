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
        #region field updateing functions:
            /// <summary>
            /// Overload for based on BaseDTO.
            /// </summary>
            private static bool TryUpdateProperty<TEntity, TDTO>(
                TDTO? source, TDTO? originalDTO, Action<TEntity> setter)
                where TEntity : BaseEntity, new()
                where TDTO : BaseDTO<TEntity, TDTO>, new()
            {
                if (source != null && originalDTO != null)
                {
                    // Compare the incoming DTO against the populated DTO from DB
                    List<List<string>>? deepChanges = 
                        CheckForInnerFieldChanges<TEntity, TDTO>
                        (source, originalDTO);
                
                    if (deepChanges != null && deepChanges.Any())
                    {
                        string details = string.Join(", ", deepChanges.Select(
                            p => $"{typeof(TEntity).Name}/" + string.Join("/", p)));
                        throw new ExpandedException($"Invalid Use of Update in {typeof(TDTO).Name}. " +
                            $"You provided values for nested fields: [{details}]. " +
                            $"Nested updates are forbidden; use the specific controller for that type.");
                    }

                    // Check if the reference (ID) changed
                    if (source.Idx != originalDTO.Idx)
                    {
                        setter(source.ToEntity());
                        return true;
                    }
                }
                return false;
            }

            /// <summary>
            /// Overload for string and classes.
            /// </summary>
            private static bool TryUpdateProperty<T>(T? source,
                Action<T> setter) 
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
            /// Overload for Nullable Value Types (DateTime?, bool?).
            /// </summary>
            private static bool TryUpdateProperty<T>(T? source,
                Action<T> setter)
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


            /// <summary>
            /// fills a DTO from a Database Entity by mapping matching properties.
            /// Handles nested DTOs recursively.
            /// </summary>
            private static object? MapFullEntityToDTO(object entity, Type entityType, Type dtoType)
            {
                var dtoInstance = Activator.CreateInstance(dtoType);
                if (dtoInstance == null) return null;

                PropertyInfo[] dtoFields = dtoType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                PropertyInfo[] entFields = entityType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

                foreach (var dProp in dtoFields)
                {
                    var eProp = entFields.FirstOrDefault(e => e.Name == dProp.Name);
                    if (eProp == null || !dProp.CanWrite) continue;

                    object? eVal = eProp.GetValue(entity);
                    if (eVal == null) continue;

                    // If the property is a nested DTO, we must map the nested entity recursively
                    if (IsBaseDtoType(dProp.PropertyType))
                    {
                        Type subEntity = dProp.PropertyType.BaseType!.GetGenericArguments()[0];
                        Type subDto = dProp.PropertyType.BaseType!.GetGenericArguments()[1];
                        dProp.SetValue(dtoInstance, MapFullEntityToDTO(eVal, subEntity, subDto));
                    }
                    // Otherwise, perform direct assignment for simple types and Nullables
                    else
                    {
                        dProp.SetValue(dtoInstance, eVal);
                    }
                }
                return dtoInstance;
            }

        private static string GetDbClassName(string entityName)
            {
                // Define rules: (Pattern to look for, Replacement)
                var rules = new Dictionary<string, string>
                {
                    { "Enemy", "Enemies" },
                    { "Player", "Players" },
                    { "Group", "Groups" },
                    { "Request", "Requests" },
                    { "Run", "Runs" },
                    { "User", "Users" },
                    { "Admin", "Admins" },
                    { "ProfileEdit", "ProfileEditRequests" } // Specific override for ProfileEditRequest
                };

                foreach (var rule in rules)
                {
                    if (entityName.StartsWith(rule.Key))
                    {
                        // Special case for ProfileEditRequest -> ProfileEditRequestsDB
                        if (entityName == "ProfileEditRequest") return "ProfileEditRequestsDB";

                        // Replace only the first occurrence (the subject of the sentence)
                        return entityName.Replace(rule.Key, rule.Value) + "DB";
                    }
                }

                // Fallback: Just add 's' to the end
                return entityName + "sDB";
            }

            /// <summary>
            /// Helper method to determine if a type inherits from BaseDTO<TEntity, TDTO>
            /// </summary>
            private static bool IsBaseDtoType(Type? type)
                {
                    while (type != null && type != typeof(object))
                    {
                        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(BaseDTO<,>))
                        {
                            return true;
                        }
                        type = type.BaseType;
                    }
                    return false;
                }

            /// <summary>
            /// Helper method to check if any non-Idx field was provided
            /// in a DTO of a BaseEntity derivative 
            /// AND if that provided value is different from the original value.
            /// </summary>
            private static List<List<string>>?
                CheckForInnerFieldChanges<TEntity, TDTO>
                (TDTO source, TDTO original, bool IsRoot = true)
                where TEntity : BaseEntity, new()
                where TDTO : BaseDTO<TEntity, TDTO>, new()
            {
                PropertyInfo[] entityProperties =
                    typeof(TDTO).GetProperties(
                        BindingFlags.Public | BindingFlags.Instance
                    );
                List<List<string>> allChanges = new List<List<string>>();
                foreach (PropertyInfo prop in entityProperties)
                {
                    // Skip Idx when called from the og 
                    if (IsRoot)
                    {
                        if(prop.Name == "Idx")
                        {
                            continue;
                        }
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
                        // Case A: Nested DTO (Recursive Check)
                        // Check if the property type is a DTO inheriting from BaseDTO
                        if (IsBaseDtoType(propType) && propType.IsClass &&
                        propType != typeof(string))
                        {
                            // 1. Find the CheckForInnerFieldChanges<T>
                            MethodInfo? genericMethod =
                                typeof(UpdateController).GetMethods(
                                    BindingFlags.NonPublic | BindingFlags.Static
                                    )
                                .Where(
                                    m => m.Name == nameof(CheckForInnerFieldChanges)
                                    && m.IsGenericMethodDefinition
                                    && m.GetParameters().Length == 3)
                                .FirstOrDefault();

                            //if we found the method, we can invoke it
                            if (genericMethod != null)
                            {
                                // Extract TEntity and TDTO from the nested DTO's BaseType
                                Type nestedEntity = propType.BaseType!.GetGenericArguments()[0];
                                Type nestedDto = propType.BaseType!.GetGenericArguments()[1];

                                #region Case: Update Idx of Nested -> Update OgNested to new Idx
                                    // if the idx of  (only root - TryUpdate caller) nested entity updates:
                                    if (IsRoot && original?.Idx != null && source.Idx != original?.Idx)
                                    {
                                        //make the Nested Entity class's DB name
                                        string dbClassName = GetDbClassName(
                                            typeof(TEntity).Name);
                                        //get the DB of Nested Entity
                                        Type? dbType = Type.GetType(
                                            $"ViewModel.DBs.{dbClassName}, ViewModel");
                                        if (dbType != null)
                                        {
                                            //SelectByIdx of new updated nested entity
                                            var selectMethod = dbType.GetMethod(
                                                "SelectByIdx", BindingFlags.Public | BindingFlags.Static);
                                            var freshEntity = selectMethod?.Invoke(null,
                                                new object[] { source.Idx });

                                            if (freshEntity != null)
                                            {
                                                // Update orignal
                                                original = (TDTO)MapFullEntityToDTO(freshEntity, typeof(TEntity), typeof(TDTO))!;
                                                //and the value we return to recousion
                                                originalValue = prop.GetValue(original);
                                            }
                                        }
                                    }
                                #endregion

                                #region call Generic Recorsivly
                                    MethodInfo constructedMethod = 
                                                    genericMethod.MakeGenericMethod
                                                    (nestedEntity, nestedDto);
                                                    List<List<string>>? innerPaths = 
                                                    (List<List<string>>?)
                                                    constructedMethod.Invoke(null,
                                                        new object?[] {
                                                            sourceValue, originalValue, false
                                                    });
                                #endregion

                                #region adding the bad proprty name to the start
                                    if (innerPaths != null && innerPaths.Any())
                                        {
                                            foreach (var path in innerPaths)
                                            {
                                                path.Insert(0, prop.Name);
                                                allChanges.Add(path);
                                            }
                                        }
                                #endregion
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
                                comparableSource =
                                    propType.GetProperty("Value")?.GetValue(sourceValue);
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
        #endregion

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
            #region Updating Fields
            // Check and update fields only if they are provided in the DTO
            // |= like += but for ||
            // 1. Strings (Nullable Reference Types)
            isModified |= TryUpdateProperty(admin.Id,
                val => originalAdmin.Id = val);
            isModified |= TryUpdateProperty(admin.Email,
                val => originalAdmin.Email = val);
            isModified |= TryUpdateProperty(admin.Password,
                val => originalAdmin.Password = val);
            isModified |= TryUpdateProperty(admin.Username,
                val => originalAdmin.Username = val);

            // 2. Nullable Value Types (DateTime?, bool?)
            isModified |= TryUpdateProperty(admin.StartDate,
                val => originalAdmin.StartDate = val);
            isModified |= TryUpdateProperty(admin.Birthday,
                val => originalAdmin.Birthday = val);
            isModified |= TryUpdateProperty(admin.IsLoggedIn,
                val => originalAdmin.IsLoggedIn = val);
            #endregion
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
        public IActionResult UpdateEnemyInLastLevel(
            [FromBody] EnemyInLastLevelDTO enemyInLastLevel)
        {
            // NOTE: The entire try/catch block is removed.
            // Any exception (404 from SelectByIdx, or 409/400 from SaveChanges) 
            // will now bubble up to the ExceptionHandler middleware.

            EnemiesInLastLevelDB enemyInLastLevelsDB =
                new EnemiesInLastLevelDB();

            // 1. Fetch current DB values. If not found, enemyInLastLevelsDB.SelectByIdx is expected 
            //    to throw an ExpandedException which the middleware handles as 404.
            EnemyInLastLevel originalEnemyInLastLevel =
                EnemiesInLastLevelDB.SelectByIdx(enemyInLastLevel.Idx);

            bool isModified = false;
            #region Updating Fields
            // Check and update fields only if they are provided in the DTO
            // |= like += but for ||
            // 1. Strings (Nullable Reference Types)
            //originalEnemyInLastLevel RunInfo turned to Full DTO
            RunInfoDTO OgRunInfoDTO = (RunInfoDTO)MapFullEntityToDTO(
                    originalEnemyInLastLevel.RunInfo,
                    typeof(RunInfo),
                    typeof(RunInfoDTO)
                )!;
            isModified |= TryUpdateProperty<RunInfo, RunInfoDTO>(
                enemyInLastLevel.RunInfo,
                OgRunInfoDTO,
                val => originalEnemyInLastLevel.RunInfo = val);

            // 2. Nullable Value Types (DateTime?, bool?)
            isModified |= TryUpdateProperty(enemyInLastLevel.Name,
                val => originalEnemyInLastLevel.Name = val);
            isModified |= TryUpdateProperty(enemyInLastLevel.Amount,
                val => originalEnemyInLastLevel.Amount = val);
            #endregion
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

        //--- GROUP UPDATE ---
        [HttpPut]
        [ActionName("GroupUpdator")]
        public IActionResult UpdateGroup([FromBody] GroupDTO group)
        {
            // NOTE: The entire try/catch block is removed.
            // Any exception (404 from SelectByIdx, or 409/400 from SaveChanges) 
            // will now bubble up to the ExceptionHandler middleware.

            GroupsDB groupsDB = new GroupsDB();

            // 1. Fetch current DB values. If not found, GroupsDB.SelectByIdx is expected 
            //    to throw an ExpandedException which the middleware handles as 404.
            Model.Entitys.Group
                originalGroup = GroupsDB.SelectByIdx(group.Idx);

            bool isModified = false;
            #region Updating Fields
            // Check and update fields only if they are provided in the DTO
            // |= like += but for ||
            // 1. Strings (Nullable Reference Types)
            isModified |= TryUpdateProperty(group.Name,
                val => originalGroup.Name = val);

            // 2. Nullable Value Types (DateTime?, bool?)
            isModified |= TryUpdateProperty(group.Score,
                val => originalGroup.Score = val);
            #endregion
            int changedRecords = 0;
            if (isModified)
            {
                groupsDB.Update(originalGroup);

                // 2. SaveChanges will throw exceptions on DB constraint violations (Unique/Not Null/FK),
                //    which are handled by the ExceptionHandler middleware.
                changedRecords = groupsDB.SaveChanges();
            }

            if (changedRecords > 0)
            {
                // Success with changes: 200 OK.
                return StatusCode(200, $"OK: Record for Group Idx=" +
                    $" {group.Idx} successfully updated.\n" +
                    $" Records changed: {changedRecords}");
            }
            else //changedRecords == 0 -> no changes made
            {
                // Success with no changes: 200 OK with a specific message.
                return StatusCode(200, $"OK: Record for Group Idx=" +
                    $"{group.Idx} was not changed as the data was identical, " +
                    $"Records changed: {changedRecords}");
            }
        }

        //--- PlayerAndGroup UPDATE ---
        [HttpPut]
        [ActionName("PlayerAndGroupUpdator")]
        public IActionResult UpdatePlayerAndGroup(
            [FromBody] PlayerAndGroupDTO playerAndGroup)
        {
            // NOTE: The entire try/catch block is removed.
            // Any exception (404 from SelectByIdx, or 409/400 from SaveChanges) 
            // will now bubble up to the ExceptionHandler middleware.

            PlayersAndGroupsDB playerAndGroupsDB =
                new PlayersAndGroupsDB();

            // 1. Fetch current DB values. If not found, playerAndGroupsDB.SelectByIdx is expected 
            //    to throw an ExpandedException which the middleware handles as 404.
            PlayerAndGroup originalPlayerAndGroup =
                PlayersAndGroupsDB.SelectByIdx(playerAndGroup.Idx);

            bool isModified = false;
            #region Updating Fields
            // Check and update fields only if they are provided in the DTO
            // |= like += but for ||
            // 1. Strings (Nullable Reference Types)
            //turn originalPlayerAndGroup Player to DTO
            PlayerDTO OgPlayerDTO = (PlayerDTO)MapFullEntityToDTO(
                originalPlayerAndGroup.Player,
                typeof(Player),
                typeof(PlayerDTO)
            )!;
            isModified |= TryUpdateProperty<Player,PlayerDTO>(
                playerAndGroup.Player, OgPlayerDTO,
                val => originalPlayerAndGroup.Player = val);

            //turn originalPlayerAndGroup Group to DTO
            GroupDTO OgGroupDTO = (GroupDTO)MapFullEntityToDTO(
                originalPlayerAndGroup.Group,
                typeof(Model.Entitys.Group),
                typeof(GroupDTO)
            )!;
            isModified |= TryUpdateProperty<Model.Entitys.Group,
                GroupDTO>( playerAndGroup.Group, OgGroupDTO,
                val => originalPlayerAndGroup.Group = val);
            #endregion
            int changedRecords = 0;
            if (isModified)
            {
                playerAndGroupsDB.Update(originalPlayerAndGroup);

                // 2. SaveChanges will throw exceptions on DB constraint violations (Unique/Not Null/FK),
                //    which are handled by the ExceptionHandler middleware.
                changedRecords = playerAndGroupsDB.SaveChanges();
            }

            if (changedRecords > 0)
            {
                // Success with changes: 200 OK.
                return StatusCode(200, $"OK: Record for PlayerAndGroup Idx=" +
                    $" {playerAndGroup.Idx} successfully updated.\n" +
                    $" Records changed: {changedRecords}");
            }
            else //changedRecords == 0 -> no changes made
            {
                // Success with no changes: 200 OK with a specific message.
                return StatusCode(200, $"OK: Record for PlayerAndGroup Idx=" +
                    $"{playerAndGroup.Idx} was not changed as the data was identical, " +
                    $"Records changed: {changedRecords}");
            }
        }

        //--- PLAYER UPDATE ---
        [HttpPut]
        [ActionName("PlayerUpdator")]
        public IActionResult UpdatePlayer([FromBody] PlayerDTO player)
        {
            // NOTE: The entire try/catch block is removed.
            // Any exception (404 from SelectByIdx, or 409/400 from SaveChanges) 
            // will now bubble up to the ExceptionHandler middleware.

            PlayersDB playersDB = new PlayersDB();

            // 1. Fetch current DB values. If not found, PlayersDB.SelectByIdx is expected 
            //    to throw an ExpandedException which the middleware handles as 404.
            Player originalPlayer = PlayersDB.SelectByIdx(player.Idx);

            bool isModified = false;
            #region Updating Fields
                // Check and update fields only if they are provided in the DTO
                // |= like += but for ||
                // 1. Strings (Nullable Reference Types)
                isModified |= TryUpdateProperty(player.Id,
                    val => originalPlayer.Id = val);
                isModified |= TryUpdateProperty(player.Email,
                    val => originalPlayer.Email = val);
                isModified |= TryUpdateProperty(player.Password,
                    val => originalPlayer.Password = val);
                isModified |= TryUpdateProperty(player.Username,
                    val => originalPlayer.Username = val);

                // 2. Nullable Value Types (DateTime?, bool?)
                isModified |= TryUpdateProperty(player.Birthday,
                    val => originalPlayer.Birthday = val);
                isModified |= TryUpdateProperty(player.IsLoggedIn,
                    val => originalPlayer.IsLoggedIn = val);
                isModified |= TryUpdateProperty(player.IsMusicOn,
                    val => originalPlayer.IsMusicOn = val);
                isModified |= TryUpdateProperty(player.IsSoundOn,
                    val => originalPlayer.IsSoundOn = val);
                isModified |= TryUpdateProperty(player.MaxLevel,
                    val => originalPlayer.MaxLevel = val);
                isModified |= TryUpdateProperty(player.TotalScore,
                    val => originalPlayer.TotalScore = val);
            #endregion
            int changedRecords = 0;
            if (isModified)
            {
                playersDB.Update(originalPlayer);

                // 2. SaveChanges will throw exceptions on DB constraint violations (Unique/Not Null/FK),
                //    which are handled by the ExceptionHandler middleware.
                changedRecords = playersDB.SaveChanges();
            }

            if (changedRecords > 0)
            {
                // Success with changes: 200 OK.
                return StatusCode(200, $"OK: Record for Player Idx=" +
                    $" {player.Idx} successfully updated.\n" +
                    $" Records changed: {changedRecords}");
            }
            else //changedRecords == 0 -> no changes made
            {
                // Success with no changes: 200 OK with a specific message.
                return StatusCode(200, $"OK: Record for Player Idx=" +
                    $"{player.Idx} was not changed as the data was identical, " +
                    $"Records changed: {changedRecords}");
            }
        }

        //--- ProfileEditRequests UPDATE ---
        [HttpPut]
        [ActionName("ProfileEditRequestUpdator")]
        public IActionResult UpdateProfileEditRequest(
            [FromBody] ProfileEditRequestDTO profileEditRequest)
        {
            // NOTE: The entire try/catch block is removed.
            // Any exception (404 from SelectByIdx, or 409/400 from SaveChanges) 
            // will now bubble up to the ExceptionHandler middleware.

            ProfileEditRequestsDB profileEditRequestsDB =
                new ProfileEditRequestsDB();

            // 1. Fetch current DB values. If not found, ProfileEditRequestsDB.SelectByIdx is expected 
            //    to throw an ExpandedException which the middleware handles as 404.
            ProfileEditRequest originalProfileEditRequest = 
                ProfileEditRequestsDB.SelectByIdx(
                    profileEditRequest.Idx
                );

            bool isModified = false;
            #region Updating Fields
            // Check and update fields only if they are provided in the DTO
            // |= like += but for ||
            // 1. Strings (Nullable Reference Types)
            PlayerDTO OgRequstingPlayerDTO = (PlayerDTO)MapFullEntityToDTO(
                originalProfileEditRequest.RequestingPlayer,
                typeof(Player),
                typeof(PlayerDTO)
            )!;
            isModified |= TryUpdateProperty<Player, PlayerDTO>(
                profileEditRequest.RequestingPlayer, 
                OgRequstingPlayerDTO,
                val => originalProfileEditRequest.RequestingPlayer = val);

            AdminDTO OgAdressingAdminDTO = (AdminDTO)MapFullEntityToDTO(
                originalProfileEditRequest.AdressingAdmin,
                typeof(Admin),
                typeof(AdminDTO)
            )!;
            isModified |= TryUpdateProperty<Admin, AdminDTO>(
                profileEditRequest.AdressingAdmin,
                OgAdressingAdminDTO,
                val => originalProfileEditRequest.AdressingAdmin = val);
            
            // 2. Nullable Value Types (DateTime?, bool?)
            isModified |= TryUpdateProperty(
                profileEditRequest.RequestingDate,
                val => originalProfileEditRequest.RequestingDate = val);
            isModified |= TryUpdateProperty(
                profileEditRequest.ReviewingDate,
                val => originalProfileEditRequest.ReviewingDate = val);
            isModified |= TryUpdateProperty(
                profileEditRequest.Status,
                val => originalProfileEditRequest.Status = val);
            #endregion
            int changedRecords = 0;
            if (isModified)
            {
                profileEditRequestsDB.Update(originalProfileEditRequest);

                // 2. SaveChanges will throw exceptions on DB constraint violations (Unique/Not Null/FK),
                //    which are handled by the ExceptionHandler middleware.
                changedRecords = profileEditRequestsDB.SaveChanges();
            }

            if (changedRecords > 0)
            {
                // Success with changes: 200 OK.
                return StatusCode(200, $"OK: Record for ProfileEditRequest Idx=" +
                    $" {profileEditRequest.Idx} successfully updated.\n" +
                    $" Records changed: {changedRecords}");
            }
            else //changedRecords == 0 -> no changes made
            {
                // Success with no changes: 200 OK with a specific message.
                return StatusCode(200, $"OK: Record for ProfileEditRequest Idx=" +
                    $"{profileEditRequest.Idx} was not changed as the data was identical, " +
                    $"Records changed: {changedRecords}");
            }
        }

        //--- RequestData UPDATE ---
        [HttpPut]
        [ActionName("RequestDataUpdator")]
        public IActionResult UpdateRequestData(
            [FromBody] RequestDataDTO requestData)
        {
            // NOTE: The entire try/catch block is removed.
            // Any exception (404 from SelectByIdx, or 409/400 from SaveChanges) 
            // will now bubble up to the ExceptionHandler middleware.

            RequestsDataDB requestDatasDB = new RequestsDataDB();

            // 1. Fetch current DB values. If not found, RequestsDataDB.SelectByIdx is expected 
            //    to throw an ExpandedException which the middleware handles as 404.
            RequestData originalRequestData = 
                RequestsDataDB.SelectByIdx(requestData.Idx);

            bool isModified = false;
            #region Updating Fields
            // Check and update fields only if they are provided in the DTO
            // |= like += but for ||
            // 1. Strings (Nullable Reference Types)
            isModified |= TryUpdateProperty(requestData.Field,
                val => originalRequestData.Field = val);
            isModified |= TryUpdateProperty(requestData.NewValue,
                val => originalRequestData.NewValue = val);
            isModified |= TryUpdateProperty(requestData.OldValue,
                val => originalRequestData.OldValue = val);
            #endregion
            int changedRecords = 0;
            if (isModified)
            {
                requestDatasDB.Update(originalRequestData);

                // 2. SaveChanges will throw exceptions on DB constraint violations (Unique/Not Null/FK),
                //    which are handled by the ExceptionHandler middleware.
                changedRecords = requestDatasDB.SaveChanges();
            }

            if (changedRecords > 0)
            {
                // Success with changes: 200 OK.
                return StatusCode(200, $"OK: Record for RequestData Idx=" +
                    $" {requestData.Idx} successfully updated.\n" +
                    $" Records changed: {changedRecords}");
            }
            else //changedRecords == 0 -> no changes made
            {
                // Success with no changes: 200 OK with a specific message.
                return StatusCode(200, $"OK: Record for RequestData Idx=" +
                    $"{requestData.Idx} was not changed as the data was identical, " +
                    $"Records changed: {changedRecords}");
            }
        }

        //--- RunInfo UPDATE ---
        [HttpPut]
        [ActionName("RunInfoUpdator")]
        public IActionResult UpdateRunInfo([FromBody] RunInfoDTO runInfo)
        {
            // NOTE: The entire try/catch block is removed.
            // Any exception (404 from SelectByIdx, or 409/400 from SaveChanges) 
            // will now bubble up to the ExceptionHandler middleware.

            RunsInfoDB runInfosDB = new RunsInfoDB();

            // 1. Fetch current DB values. If not found, RunsInfoDB.SelectByIdx is expected 
            //    to throw an ExpandedException which the middleware handles as 404.
            RunInfo originalRunInfo = RunsInfoDB.SelectByIdx(runInfo.Idx);

            bool isModified = false;
            #region Updating Fields
            // Check and update fields only if they are provided in the DTO
            // |= like += but for ||
            // 1. Strings (Nullable Reference Types)
            PlayerDTO OgPlayerDTO = (PlayerDTO)MapFullEntityToDTO(
                originalRunInfo.Player,
                typeof(Player),
                typeof(PlayerDTO)
            )!; 
            isModified |= TryUpdateProperty<Player, PlayerDTO>(
                runInfo.Player,
                OgPlayerDTO,
                val => originalRunInfo.Player = val);

            // 2. Nullable Value Types (DateTime?, bool?)
            isModified |= TryUpdateProperty(runInfo.CurrentHp,
                val => originalRunInfo.CurrentHp = val);
            isModified |= TryUpdateProperty(runInfo.CurrentShieldLevel,
                val => originalRunInfo.CurrentShieldLevel = val);
            isModified |= TryUpdateProperty(runInfo.CurrentBlasterCount,
                val => originalRunInfo.CurrentBlasterCount = val);
            isModified |= TryUpdateProperty(runInfo.CurrentLevel,
                val => originalRunInfo.CurrentLevel = val);
            isModified |= TryUpdateProperty(runInfo.CurrentScore,
                val => originalRunInfo.CurrentScore = val);
            isModified |= TryUpdateProperty(runInfo.IsRunOver,
                val => originalRunInfo.IsRunOver = val);
            isModified |= TryUpdateProperty(runInfo.RunStopDate,
                val => originalRunInfo.RunStopDate = val);
            #endregion
            int changedRecords = 0;
            if (isModified)
            {
                runInfosDB.Update(originalRunInfo);

                // 2. SaveChanges will throw exceptions on DB constraint violations (Unique/Not Null/FK),
                //    which are handled by the ExceptionHandler middleware.
                changedRecords = runInfosDB.SaveChanges();
            }

            if (changedRecords > 0)
            {
                // Success with changes: 200 OK.
                return StatusCode(200, $"OK: Record for RunInfo Idx=" +
                    $" {runInfo.Idx} successfully updated.\n" +
                    $" Records changed: {changedRecords}");
            }
            else //changedRecords == 0 -> no changes made
            {
                // Success with no changes: 200 OK with a specific message.
                return StatusCode(200, $"OK: Record for RunInfo Idx=" +
                    $"{runInfo.Idx} was not changed as the data was identical, " +
                    $"Records changed: {changedRecords}");
            }
        }

        //--- USER UPDATE ---
        [HttpPut]
        [ActionName("UserUpdator")]
        public IActionResult UpdateUser([FromBody] UserDTO user)
        {
            // NOTE: The entire try/catch block is removed.
            // Any exception (404 from SelectByIdx, or 409/400 from SaveChanges) 
            // will now bubble up to the ExceptionHandler middleware.

            UsersDB usersDB = new UsersDB();

            // 1. Fetch current DB values. If not found, UsersDB.SelectByIdx is expected 
            //    to throw an ExpandedException which the middleware handles as 404.
            User originalUser = UsersDB.SelectByIdx(user.Idx);

            bool isModified = false;
            #region Updating Fields

            // Check and update fields only if they are provided in the DTO
            // |= like += but for ||
            // 1. Strings (Nullable Reference Types)
            isModified |= TryUpdateProperty(user.Id,
                val => originalUser.Id = val);
            isModified |= TryUpdateProperty(user.Email,
                val => originalUser.Email = val);
            isModified |= TryUpdateProperty(user.Password,
                val => originalUser.Password = val);
            isModified |= TryUpdateProperty(user.Username,
                val => originalUser.Username = val);

            // 2. Nullable Value Types (DateTime?, bool?)
            isModified |= TryUpdateProperty(user.Birthday,
                val => originalUser.Birthday = val);
            isModified |= TryUpdateProperty(user.IsLoggedIn,
                val => originalUser.IsLoggedIn = val);
            #endregion
            int changedRecords = 0;
            if (isModified)
            {
                usersDB.Update(originalUser);

                // 2. SaveChanges will throw exceptions on DB constraint violations (Unique/Not Null/FK),
                //    which are handled by the ExceptionHandler middleware.
                changedRecords = usersDB.SaveChanges();
            }

            if (changedRecords > 0)
            {
                // Success with changes: 200 OK.
                return StatusCode(200, $"OK: Record for User Idx=" +
                    $" {user.Idx} successfully updated.\n" +
                    $" Records changed: {changedRecords}");
            }
            else //changedRecords == 0 -> no changes made
            {
                // Success with no changes: 200 OK with a specific message.
                return StatusCode(200, $"OK: Record for User Idx=" +
                    $"{user.Idx} was not changed as the data was identical, " +
                    $"Records changed: {changedRecords}");
            }
        }

    }
}
