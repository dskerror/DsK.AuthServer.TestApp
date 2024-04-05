using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace DsK.AuthServer.TestApp.Client.Services
{
    public partial class SecurityServiceClient
    {
        private readonly ILocalStorageService _localStorageService;
        private readonly HttpClient _httpClient;
        private readonly AuthenticationStateProvider _authenticationStateProvider;

        public SecurityServiceClient(ILocalStorageService localStorageService,
            HttpClient httpClient, AuthenticationStateProvider authenticationStateProvider)
        {
            _localStorageService = localStorageService;
            _httpClient = httpClient;
            _authenticationStateProvider = authenticationStateProvider;
        }
        private async Task PrepareBearerToken()
        {
            var token = await GetTokenAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
        }

        public async Task<string> GetTokenAsync()
        {
            string token = await _localStorageService.GetItemAsync<string>("token");
            if (string.IsNullOrEmpty(token))
                return string.Empty;


            if (TokenHelpers.IsTokenExpired(token))
                token = await TryRefreshToken();

            return token;
        }

        private async Task<string> TryRefreshToken()
        {
            //TODO : FIX

            return "";
        }
        public bool HasPermission(ClaimsPrincipal user, string permission)
        {
            //ClaimsPrincipal newuser = user;
            // && (x.Value.Contains(permission)|| x.Value.Contains("Admin")
            var permissions = user.Claims.Where(x => x.Type == ClaimTypes.Role).FirstOrDefault();

            if (UserHasPermission(permissions, permission) || UserHasPermission(permissions, "Admin"))
                return true;
            else
                return false;
        }

        public int GetUserId(ClaimsPrincipal user)
        {
            string userId = user.Claims.Where(_ => _.Type == "UserId").Select(_ => _.Value).FirstOrDefault();
            int userIdParsed = 0;
            int.TryParse(userId, out userIdParsed);
            return userIdParsed;
        }
        private bool UserHasPermission(Claim permissions, string permission)
        {
            if (permissions != null)
            {
                var schema = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role : ";
                var parsedPermissions = permissions.Value.ToString().Replace(schema, "").Trim().TrimStart('[').TrimEnd(']').Replace("\"", "").Split(',');

                foreach (var parsedPermission in parsedPermissions)
                {
                    if (parsedPermission == permission) return true;
                }
            }
            return false;
        }
    }

}
