using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using MyMicroservice.Shared.Dtos;
using MyMicroservice.Web.Models;
using MyMicroservice.Web.Services.Interfaces;
using MyMicroService.Shared.Dtos;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text.Json;

namespace MyMicroservice.Web.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ClientSettings _clientSettings;
        private readonly ServiceApiSettings _serviceApiSettings;

        public IdentityService(HttpClient httpClient, IHttpContextAccessor contextAccessor, IOptions<ClientSettings> clientSettings, IOptions<ServiceApiSettings> serviceApiSettings)
        {
            _httpClient = httpClient;
            _httpContextAccessor = contextAccessor;
            _clientSettings = clientSettings.Value;
            _serviceApiSettings = serviceApiSettings.Value;
        }

        public Task<TokenResponse> GetAccessTokenByRefreshtoken()
        {
            throw new NotImplementedException();
        }

        public Task RevokeRefreshtoken()
        {
            throw new NotImplementedException();
        }

        public async Task<Response<bool>> SignIn(SignInInput signInInput)
        {
            var disco = await _httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address=_serviceApiSettings.BaseUri,
                Policy=new DiscoveryPolicy { RequireHttps=false}
            });

            if (disco.IsError)
            {
                throw disco.Exception;
            }

            var passwordTokenRequest = new PasswordTokenRequest
            {
                ClientId = _clientSettings.WebClientForUser.ClientId,
                ClientSecret = _clientSettings.WebClientForUser.ClientSecret,
                UserName = signInInput.Email,
                Password = signInInput.Password,
                Address = disco.TokenEndpoint
            };

            var token=await _httpClient.RequestPasswordTokenAsync(passwordTokenRequest);

            if(token.IsError)
            {
                var responseContent=await token.HttpResponse.Content.ReadAsStringAsync();

                var errorDto = JsonSerializer.Deserialize<ErrorDto>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = false });

                return Response<bool>.Fail(errorDto.Errors, 400);
            }

            var userInfoRequest = new UserInfoRequest
            {
                Token = token.AccessToken,
                Address = disco.UserInfoEndpoint
            };

            var userInfo=await _httpClient.GetUserInfoAsync(userInfoRequest);

            if(userInfo.IsError)
            {
                throw userInfo.Exception;
            }

            //uygulamaya username ve role u nereden alacagın ıbelirtir girdiğimiz name ve admin degerleri -->cookiden alsın
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(userInfo.Claims, CookieAuthenticationDefaults.AuthenticationScheme, "name", "role");

            //cookie olusturma prensibi
            ClaimsPrincipal claimsPrincipal=new ClaimsPrincipal(claimsIdentity);

            var authonticationProperties = new AuthenticationProperties();

            authonticationProperties.StoreTokens(new List<AuthenticationToken>()
            {
                new AuthenticationToken{Name=OpenIdConnectParameterNames.AccessToken,Value=token.AccessToken},
                new AuthenticationToken{Name=OpenIdConnectParameterNames.RefreshToken,Value=token.RefreshToken},

                //herhangi bir kulture baglı kalmdan token süresi belirtiyoruz
                new AuthenticationToken{Name=OpenIdConnectParameterNames.ExpiresIn,Value=
                    DateTime.Now.AddSeconds(token.ExpiresIn).ToString("o",CultureInfo.InvariantCulture)}
            });

            authonticationProperties.IsPersistent = signInInput.IsRememberMe;

            await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, authonticationProperties);

            return Response<bool>.Success(200);

        }
    }
}
