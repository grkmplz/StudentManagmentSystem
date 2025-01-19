using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StudentCoursesManagement.Domain.Entities;
using StudentCoursesManagement.WebApi.Dtos.Role;
using StudentCoursesManagement.WebApi.Dtos.User;
using StudentCoursesManagement.WebApi.Services.Interfaces;
using System.Data;

namespace StudentCoursesManagement.WebApi.Services
{
    public class RoleService : IRoleService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;

        public RoleService(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }


        public async Task<List<SettingsDto>> GetRolesAsync()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            var roleViewModels = new List<SettingsDto>();
            
            foreach (var role in roles)
            {
                var userCount = (await _userManager.GetUsersInRoleAsync(role.Name)).Count;

                roleViewModels.Add(new SettingsDto
                {
                    RoleId = role.Id,
                    RoleName = role.Name,
                    UserCount = userCount,
                    IsActive = role.IsActive,
                });
            }

            return roleViewModels;
        }

        public async Task<SettingsDto> GetRoleByIdAsync(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);

            if (role == null) 
                return null;

            return new SettingsDto
            { 
                RoleName = role.Name 
            };

        }

        public async Task CreateRoleAsync(CreateRoleDto model)
        {
            var role = new Role 
            { 
                Name = model.RoleName 
            };

            await _roleManager.CreateAsync(role);
        }

        public async Task UpdateRoleAsync(EditRoleDto model)
        {
            var role = await _roleManager.FindByIdAsync(model.RoleId);

            if (role != null)
            {
                role.Name = model.RoleName;
                await _roleManager.UpdateAsync(role);
            }
        }

        public async Task DeleteRoleAsync(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);

            if (role != null)
            {
                await _roleManager.DeleteAsync(role);
            }
        }

        public async Task<List<UserRoleDto>> GetUsersInRoleAsync(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);

            if (role == null)
                return new List<UserRoleDto>();

            var users = await _userManager.Users.ToListAsync();

            var userRoleViewModels = new List<UserRoleDto>();

            foreach (var user in users)
            {
                var isSelected = await _userManager.IsInRoleAsync(user, role.Name);

                if(user.Id != "1")
                {
                    userRoleViewModels.Add(new UserRoleDto
                    {
                        UserId = user.Id,
                        UserName = user.UserName,
                        IsSelected = isSelected
                    });
                }
              
            }

            return userRoleViewModels;
        }

        public async Task UpdateUsersInRoleAsync(string roleId, List<UserRoleDto> users)
        {
            var role = await _roleManager.FindByIdAsync(roleId);

            if (role == null) 
                return;

            foreach (var user in users)
            {
                var identityUser = await _userManager.FindByIdAsync(user.UserId);
                var isInRole = await _userManager.IsInRoleAsync(identityUser, role.Name);

                if (identityUser == null) 
                    continue;

                if (user.IsSelected && !isInRole)
                {
                    var userRoles = await _userManager.GetRolesAsync(identityUser);
                    foreach (var roles in userRoles)
                    {
                        await _userManager.RemoveFromRoleAsync(identityUser, roles);
                    }
                    await _userManager.AddToRoleAsync(identityUser, role.Name);
                }
                else if (!user.IsSelected && isInRole)
                {
                    await _userManager.RemoveFromRoleAsync(identityUser, role.Name);
                }
            }
        }
    }
}
