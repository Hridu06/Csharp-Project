using System;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace VisitorManagementSystem.Blazor.Helpers
{
    public class JwtParser
    {
        public JwtPayload Parse(string jwt)
        {
            if (string.IsNullOrWhiteSpace(jwt)) return null;
            var parts = jwt.Split('.');
            if (parts.Length < 2) return null;
            string payload = parts[1];
            // pad
            payload = payload.PadRight(payload.Length + (4 - payload.Length % 4) % 4, '=');
            try
            {
                var jsonBytes = Convert.FromBase64String(payload.Replace('-', '+').Replace('_', '/'));
                var json = Encoding.UTF8.GetString(jsonBytes);
                var doc = JsonDocument.Parse(json);
                var root = doc.RootElement;

                string role = null;
                // role could be claimTypes role or "roles" etc. check common keys
                if (root.TryGetProperty("role", out var r1)) role = r1.GetString();
                else if (root.TryGetProperty("roles", out var r2)) role = r2.ToString();
                else if (root.TryGetProperty("http://schemas.microsoft.com/ws/2008/06/identity/claims/role", out var r3)) role = r3.GetString();
                else if (root.TryGetProperty("roles", out var r4)) role = r4.ToString();

                root.TryGetProperty("exp", out var expEl);
                long exp = expEl.ValueKind == JsonValueKind.Number ? expEl.GetInt64() : 0;

                return new JwtPayload
                {
                    Raw = json,
                    Role = role,
                    Exp = exp
                };
            }
            catch
            {
                return null;
            }
        }
    }

    public class JwtPayload
    {
        public string? Raw { get; set; }
        public string? Role { get; set; }
        public long Exp { get; set; }
    }
}
