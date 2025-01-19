using StudentCoursesManagement.Application.ViewModels.SettingsViewModel;
using StudentCoursesManagement.Application.ViewModels.StudentCoursesViewModels;
using System.Net.Http.Json;

namespace StudentCoursesManagement.Application.Services
{
    public class RoleService
    {
        private readonly IHttpClientFactory _httpClient;

        public RoleService(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<SettingsViewModel>> GetAllRolesAsync()
        {
            var client = _httpClient.CreateClient("AuthenticatedClient");
            var response = await client.GetAsync("http://localhost:5133/api/Role");

            if(response == null || !response.IsSuccessStatusCode)
            {
                return new List<SettingsViewModel>();
            }

            var serializedData = await response.Content.ReadFromJsonAsync<List<SettingsViewModel>>();

            return serializedData;
        }

        public async Task<RoleViewModel> GetByIdRoleAsync(string roleId)
        {
            var client = _httpClient.CreateClient("AuthenticatedClient");
            var responseGetRoleById = await client.GetAsync($"http://localhost:5133/api/Role/GetByIdRole?id={roleId}");

            if (responseGetRoleById == null || !responseGetRoleById.IsSuccessStatusCode)
            {
                return new RoleViewModel() ;
            }

            var serializedResponseGetRoleById = await responseGetRoleById.Content.ReadFromJsonAsync<RoleViewModel>();

            return serializedResponseGetRoleById;
        }

        public async Task<EditRoleViewModel> UpdateRoleGetAsync(string roleId)
        {
            var getRoleById = await GetByIdRoleAsync(roleId);

            var client = _httpClient.CreateClient("AuthenticatedClient");

            var responseGetUserInRoles = await client.GetAsync($"http://localhost:5133/api/Role/GetUsersDependOnRole?id={roleId}");

            var serializedResponseGetUsersInRoles = await responseGetUserInRoles.Content.ReadFromJsonAsync<List<UserRoleViewModel>>();

            var model = new EditRoleViewModel
            {
                RoleId = roleId,
                RoleName = getRoleById.RoleName,
                Users = serializedResponseGetUsersInRoles,
            };

            return model;
        }

        public async Task<bool> UpdateRolePostAsync(EditRoleViewModel model)
        {
            var client = _httpClient.CreateClient("AuthenticatedClient");
            var responseUpdateRole = await client.PutAsJsonAsync("http://localhost:5133/api/Role/UpdateRole", model);
            var responseUpdateUsersByRole = await client.PutAsJsonAsync($"http://localhost:5133/api/Role/UpdateUsersDependOnRole?roleId={model.RoleId}", model.Users);

            if(!responseUpdateRole.IsSuccessStatusCode || !responseUpdateUsersByRole.IsSuccessStatusCode)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> CreateRoleAsync(CreateRoleViewModel model)
        {        
           var client = _httpClient.CreateClient("AuthenticatedClient");
           var respone = await client.PostAsJsonAsync("http://localhost:5133/api/Role/CreateRole", model);

           if(!respone.IsSuccessStatusCode)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> DeleteRole(string id)
        {
            var client = _httpClient.CreateClient("AuthenticatedClient");
            var responseDeleteRole = await client.DeleteAsync($"http://localhost:5133/api/Role/DeleteRole?id={id}");
            if (!responseDeleteRole.IsSuccessStatusCode)
            {
                return false;
            }

            return true;
        }
    }
}
