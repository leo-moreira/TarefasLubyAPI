using System;

namespace TarefasLuby.V1.Models
{
    public class RefreshToken
    {
        public string Token { get; set; }
        public DateTime Expiration  { get; set; }
        public string TokenRefresh { get; set; }
        public DateTime ExpirationRefreshToken { get; set; }
    }
}
