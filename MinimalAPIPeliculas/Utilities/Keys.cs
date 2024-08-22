using Microsoft.IdentityModel.Tokens;

namespace MinimalAPIPeliculas.Utilities
{
    public static class Keys
    {
        public const string OwnIssuer = "our-app";
        private const string KeysSection = "Authentication:Schemes:Bearer:SigningKeys";
        private const string KeysSection_Emisor = "Issuer";
        private const string KeysSection_Value = "Value";

        public static IEnumerable<SecurityKey> GetKey(IConfiguration configuration) => GetKey(configuration, OwnIssuer);

        public static IEnumerable<SecurityKey> GetKey(IConfiguration configuration, string issuer)
        {
            var signingKeys = configuration.GetSection(KeysSection)
                .GetChildren()
                .SingleOrDefault(key => key[KeysSection_Emisor] == issuer);
            if(signingKeys is not null && signingKeys[KeysSection_Value] is string keyvalue)
            {
                yield return new SymmetricSecurityKey(Convert.FromBase64String(keyvalue));
            }
        }
        public static IEnumerable<SecurityKey> GetAllKey(IConfiguration configuration)
        {
            var signingKeys = configuration.GetSection(KeysSection)
                .GetChildren();
            foreach (var signingKey in signingKeys)
            {
                if (signingKey[KeysSection_Value] is string keyvalue)
                {
                    yield return new SymmetricSecurityKey(Convert.FromBase64String(keyvalue));
                }
            }
        }
    }
}
