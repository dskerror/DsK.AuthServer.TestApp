using System.ComponentModel;
using System.Reflection;

namespace DsK.AuthServer.TestApp.Shared
{
    public class TokenModel
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }

        public TokenModel(string token, string refreshToken)
        {
            Token = token;
            RefreshToken = refreshToken;
        }
    }

    public class TokenSettingsModel
    {
        public string? Issuer { get; set; }
        public string? Audience { get; set; }
        public string? Key { get; set; }
    }

    public class ValidateLoginTokenDto
    {
        public string LoginToken { get; set; } = string.Empty;
        public string TokenKey { get; set; } = string.Empty;
    }

    public static class Access
    {
        public const string Admin = "Admin";

        [DisplayName("Counter")]
        [Description("Counter Permissions")]
        public static class CounterPage
        {
            public const string CounterFunction = "TestApp.Counter";
        }

        /// <summary>
        /// Returns a list of Permissions.
        /// </summary>
        /// <returns></returns>
        public static List<string> GetRegisteredPermissions()
        {
            var permissions = new List<string>();
            foreach (var prop in typeof(Access).GetNestedTypes().SelectMany(c => c.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)))
            {
                var propertyValue = prop.GetValue(null);
                if (propertyValue is not null)
                    permissions.Add(propertyValue.ToString());
            }
            return permissions;
        }
    }

}
