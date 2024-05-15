using IdentityModel.Client;
using MyMicroservice.Shared.Dtos;
using MyMicroservice.Web.Models;

namespace MyMicroservice.Web.Services.Interfaces
{
    public interface IIdentityService
    {
        Task<Response<bool>> SignIn(SignInInput signInInput);
        Task<TokenResponse> GetAccessTokenByRefreshtoken();
        Task RevokeRefreshtoken();
    }
}
