using MyMicroservice.Web.Models;

namespace MyMicroservice.Web.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserViewModel> GetUser();
    }
}
