using DsK.AuthServer.TestApp.Server.HttpClients;
using DsK.AuthServer.TestApp.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DsK.AuthServer.TestApp.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SecurityController : ControllerBase
    {
        HttpClient _Http;
        private readonly TokenSettingsModel _tokenSettings;
        public SecurityController(AuthorizarionServerAPIHttpClient authorizarionServerAPIHttpClient, IOptions<TokenSettingsModel> tokenSettings)
        {
            _Http = authorizarionServerAPIHttpClient.Client;
            _tokenSettings = tokenSettings.Value;
        }

        [HttpPost]
        [Route("ValidateLoginToken")]
        public async Task<IActionResult> ValidateLoginToken(ValidateLoginTokenDto model)
        {
            //todo : fix this line
            model.TokenKey = _tokenSettings.Key;
            var response = await _Http.PostAsJsonAsync($"https://localhost:7045/authentication/ValidateLoginToken", model);

            if (!response.IsSuccessStatusCode) return NotFound();

            var result = await response.Content.ReadFromJsonAsync<TokenModel>();

            if (result == null) return NotFound();

            return Ok(result);
        }
    }

}
