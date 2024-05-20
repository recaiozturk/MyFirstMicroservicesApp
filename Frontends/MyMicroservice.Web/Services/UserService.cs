using MyMicroservice.Web.Models;
using MyMicroservice.Web.Services.Interfaces;

namespace MyMicroservice.Web.Services
{
    public class UserService : IUserService
    {
        private readonly HttpClient _httpClient;

        public UserService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<UserViewModel> GetUser()
        {
            return await _httpClient.GetFromJsonAsync<UserViewModel>("api/user/getuser"); 
        }
    }
}
