using Internative.FoodRecipes.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Internative.FoodRecipes.Application.Security
{
    /// <summary>
    /// Permission service interface
    /// </summary>
    public partial interface IPermissionService
    {
        /// <summary>
        /// Gets all permissions
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the permissions
        /// </returns>
        Task<IList<PermissionRecord>> GetAllPermissionRecordsAsync();

        /// <summary>
        /// Inserts a permission
        /// </summary>
        /// <param name="permission">Permission</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task InsertPermissionRecordAsync(PermissionRecord permission);

        /// <summary>
        /// Gets a permission record by identifier
        /// </summary>
        /// <param name="permission">Permission</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains a permission record
        /// </returns>
        Task<PermissionRecord> GetPermissionRecordByIdAsync(int permissionId);

        /// <summary>
        /// Updates the permission
        /// </summary>
        /// <param name="permission">Permission</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task UpdatePermissionRecordAsync(PermissionRecord permission);

        /// <summary>
        /// Deletes the permission
        /// </summary>
        /// <param name="permission">Permission</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task DeletePermissionRecordAsync(PermissionRecord permission);

        /// <summary>
        /// Install permissions
        /// </summary>
        /// <param name="permissionProvider">Permission provider</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task InstallPermissionsAsync(IPermissionProvider permissionProvider);

        /// <summary>
        /// Authorize permission
        /// </summary>
        /// <param name="permission">Permission record</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the rue - authorized; otherwise, false
        /// </returns>
        Task<bool> AuthorizeAsync(PermissionRecord permission);

        /// <summary>
        /// Authorize permission
        /// </summary>
        /// <param name="permission">Permission record</param>
        /// <param name="customer">Customer</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the rue - authorized; otherwise, false
        /// </returns>
        Task<bool> AuthorizeAsync(PermissionRecord permission, IdentityUser user);

        /// <summary>
        /// Authorize permission
        /// </summary>
        /// <param name="permissionRecordSystemName">Permission record system name</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the rue - authorized; otherwise, false
        /// </returns>
        Task<bool> AuthorizeAsync(string permissionRecordSystemName);

        /// <summary>
        /// Authorize permission
        /// </summary>
        /// <param name="permissionRecordSystemName">Permission record system name</param>
        /// <param name="customer">Customer</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the rue - authorized; otherwise, false
        /// </returns>
        Task<bool> AuthorizeAsync(string permissionRecordSystemName, IdentityUser user);

        /// <summary>
        /// Authorize permission
        /// </summary>
        /// <param name="permissionRecordSystemName">Permission record system name</param>
        /// <param name="customerRoleId">Customer role identifier</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the rue - authorized; otherwise, false
        /// </returns>
        Task<bool> AuthorizeAsync(string permissionRecordSystemName, int customerRoleId);

        /// <summary>
        /// Gets a permission record-customer role mapping
        /// </summary>
        /// <param name="permissionId">Permission identifier</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task<IList<PermissionRecordIdentityRoleMapping>> GetMappingByPermissionRecordIdAsync(int permissionId);

        /// <summary>
        /// Delete a permission record-customer role mapping
        /// </summary>
        /// <param name="permissionId">Permission identifier</param>
        /// <param name="customerRoleId">Customer role identifier</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task DeletePermissionRecordCustomerRoleMappingAsync(int permissionId, int customerRoleId);

        /// <summary>
        /// Inserts a permission record-customer role mapping
        /// </summary>
        /// <param name="permissionRecordCustomerRoleMapping">Permission record-customer role mapping</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task InsertPermissionRecordCustomerRoleMappingAsync(PermissionRecordIdentityRoleMapping permissionRecordCustomerRoleMapping);
    }
}
