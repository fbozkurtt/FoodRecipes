using Internative.FoodRecipes.Application.Common.Interfaces;
using Internative.FoodRecipes.Application.Security;
using Internative.FoodRecipes.Domain.Entities;
using Internative.FoodRecipes.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Internative.FoodRecipes.Infrastructure.Security
{
    public class PermissionService : IPermissionService
    {
        #region Fields

        private readonly UserManager<IdentityUser<int>> _userManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;
        private readonly IIdentityService _identityService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IRepository<PermissionRecord> _permissionRecordRepository;
        private readonly IRepository<PermissionRecordIdentityRoleMapping> _permissionRecordIdentityRoleMappingRepository;

        #endregion

        #region Ctor

        public PermissionService(
            UserManager<IdentityUser<int>> userManager, 
            RoleManager<IdentityRole<int>> roleManager,
            IIdentityService identityService,
            ICurrentUserService currentUserService,
            IRepository<PermissionRecord> permissionRecordRepository,
            IRepository<PermissionRecordIdentityRoleMapping> permissionRecordIdentityRoleMappingRepository)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _identityService = identityService;
            _currentUserService = currentUserService;
            _permissionRecordRepository = permissionRecordRepository;
            _permissionRecordIdentityRoleMappingRepository = permissionRecordIdentityRoleMappingRepository;
        }

        #endregion

        #region Utilities

        protected virtual async Task<IList<PermissionRecord>> GetPermissionRecordsByIdentityRoleIdAsync(int identityRoleId)
        {

            var permissionRecordIds = (await _permissionRecordIdentityRoleMappingRepository.GetAllAsync(query =>
            {
                return query.Where(_ => _.IdentityRoleId == identityRoleId);
            })).Select(_=>_.PermissionRecordId);
                

            return (await _permissionRecordRepository.GetAllAsync(query=>
            {
                return query.Where(_ => permissionRecordIds.Contains(_.Id));
            })).ToList();
        }

        protected virtual async Task DeletePermissionRecordAsync(PermissionRecord permission)
        {
            await _permissionRecordRepository.DeleteAsync(permission);
        }

        protected virtual async Task InsertPermissionRecordAsync(PermissionRecord permission)
        {
            await _permissionRecordRepository.InsertAsync(permission);
        }

        protected virtual async Task<PermissionRecord> GetPermissionRecordBySystemNameAsync(string systemName)
        {
            if (string.IsNullOrWhiteSpace(systemName))
                return null;

            return (await _permissionRecordRepository.GetAllAsync(query=> {
                return query.Where(_ => _.SystemName.Equals(systemName));
            })).FirstOrDefault();
        }

        #endregion

        #region Methods

        public async Task<bool> AuthorizeAsync(PermissionRecord permission)
        {
            var currentUser = await _identityService.GetUserAsync(_currentUserService.UserId);
            return await AuthorizeAsync(permission, currentUser);
        }

        public async Task<bool> AuthorizeAsync(PermissionRecord permission, IdentityUser<int> user)
        {
            if (permission == null)
                return false;

            if (user == null)
                return false;

            return await AuthorizeAsync(permission.SystemName, user);
        }

        public async Task<bool> AuthorizeAsync(string permissionRecordSystemName)
        {
            var currentUser = await _identityService.GetUserAsync(_currentUserService.UserId);
            return await AuthorizeAsync(permissionRecordSystemName, currentUser);
        }

        public async Task<bool> AuthorizeAsync(string permissionRecordSystemName, IdentityUser<int> user)
        {
            if (string.IsNullOrEmpty(permissionRecordSystemName))
                return false;

            var customerRoles = await _userManager.GetRolesAsync(user);
            foreach (var roleName in customerRoles)
            {
                var role = (await _roleManager.FindByNameAsync(roleName));

                if (await AuthorizeAsync(permissionRecordSystemName, role.Id))
                    //yes, we have such permission
                    return true;
            }

            //no permission found
            return false;
        }

        public async Task<bool> AuthorizeAsync(string permissionRecordSystemName, int identityRoleId)
        {
            if (string.IsNullOrEmpty(permissionRecordSystemName))
                return false;

            var permissions = await GetPermissionRecordsByIdentityRoleIdAsync(identityRoleId);

            foreach (var permission in permissions)
                if (permission.SystemName.Equals(permissionRecordSystemName, StringComparison.InvariantCultureIgnoreCase))
                    return true;

            return false;
        }

        public async Task DeletePermissionRecordIdentityRoleMappingAsync(string permissionId, int identityRoleId)
        {
            var mapping = (await _permissionRecordIdentityRoleMappingRepository.GetAllAsync(query =>
            {
                return query.Where(_ => _.IdentityRoleId == identityRoleId && _.PermissionRecordId == permissionId);
            })).FirstOrDefault();

            if (mapping is null)
                return;

            await _permissionRecordIdentityRoleMappingRepository.DeleteAsync(mapping);
        }

        public async Task<IList<PermissionRecord>> GetAllPermissionRecordsAsync()
        {
            return (await _permissionRecordRepository.GetAllAsync(query=> {
                return query.OrderBy(_ => _.PermissionName);
            })).ToList();
        }

        public async Task<IList<PermissionRecordIdentityRoleMapping>> GetMappingByPermissionRecordIdAsync(string permissionId)
        {
            return (await _permissionRecordIdentityRoleMappingRepository.GetAllAsync(query =>
            {
                return query.Where(_ => _.PermissionRecordId == permissionId);
            })).ToList();
        }

        public async Task<PermissionRecord> GetPermissionRecordByIdAsync(string permissionId)
        {
            return await _permissionRecordRepository.GetByIdAsync(permissionId);
        }

        public async Task InsertPermissionRecordIdentityRoleMappingAsync(PermissionRecordIdentityRoleMapping permissionRecordIdentityRoleMapping)
        {
            await _permissionRecordIdentityRoleMappingRepository.InsertAsync(permissionRecordIdentityRoleMapping);
        }

        public async Task InstallPermissionsAsync(IPermissionProvider permissionProvider)
        {
            //install new permissions
            var permissions = permissionProvider.GetPermissions();
            //default customer role mappings
            var defaultPermissions = permissionProvider.GetDefaultPermissions().ToList();

            foreach (var permission in permissions)
            {
                var permission1 = await GetPermissionRecordBySystemNameAsync(permission.SystemName);
                if (permission1 != null)
                    continue;

                //new permission (install it)
                permission1 = new PermissionRecord
                {
                    Name = permission.Name,
                    SystemName = permission.SystemName,
                    Category = permission.Category
                };

                //save new permission
                await InsertPermissionRecordAsync(permission1);

                foreach (var defaultPermission in defaultPermissions)
                {
                    var identityRole = (await _roleManager.FindByNameAsync(defaultPermission.systemRoleName));
                    if (identityRole == null)
                    {
                        //new role (save it)
                        identityRole = new IdentityRole<int>(defaultPermission.systemRoleName);

                        await _roleManager.CreateAsync(identityRole);
                    }

                    var defaultMappingProvided = defaultPermission.permissions.Any(p => p.SystemName == permission1.SystemName);

                    if (!defaultMappingProvided)
                        continue;

                    await InsertPermissionRecordIdentityRoleMappingAsync(new PermissionRecordIdentityRoleMapping { IdentityRoleId = identityRole.Id, PermissionRecordId = permission1.Id });
                }
            }
        }

        public async Task UpdatePermissionRecordAsync(PermissionRecord permission)
        {
            await _permissionRecordRepository.UpdateAsync(permission);
        }

        #endregion
    }
}