using Microsoft.IdentityModel.Tokens;
using System;

namespace Inspektor_API_REST.Utilities
{
    public class JwtOption
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Subject { get; set; }
        public byte[] AccessSecret { get; set; }
        public byte[] AccessExternalSecret { get; set; }
        public byte[] RefreshSecret { get; set; }
        public DateTime IssuedAt => DateTime.UtcNow;
        public TimeSpan AccessValidFor { get; set; } = TimeSpan.FromMinutes(60);
        public TimeSpan AccessValidExternalFor { get; set; } = TimeSpan.FromMinutes(5256000);
        public TimeSpan RefreshValidFor { get; set; } = TimeSpan.FromMinutes(43200);
        public DateTime NotBefore => DateTime.UtcNow;
        public DateTime AccessExpiration => IssuedAt.Add(AccessValidFor);
        public DateTime AccessExpireExternal => IssuedAt.Add(AccessValidExternalFor);
        public DateTime RefreshExpiration => IssuedAt.Add(RefreshValidFor);
        public SigningCredentials AccessSigningCredentials { get; set; }
        public SigningCredentials AccessExternalSigningCredentials { get; set; }
        public SigningCredentials RefreshSigningCredentials { get; set; }
    }
}