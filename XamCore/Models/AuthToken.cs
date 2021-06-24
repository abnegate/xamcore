using System;

namespace XamCore.Models
{
    public class AuthToken
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public DateTime Expiration { get; set; }

        public AuthToken(
            string token,
            string refreshToken,
            DateTime expiration)
        {
            Token = token;
            RefreshToken = refreshToken;
            Expiration = expiration;
        }
    }
}
