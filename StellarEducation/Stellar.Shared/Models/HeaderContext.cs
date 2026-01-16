using System;
using System.Collections.Generic;
using System.Text.Json;
using Stellar.Shared.Constants;

namespace Stellar.Shared.Models
{
    public class HeaderContext
    {
        public string? TaiKhoanId { get; set; }
        public string? Ten { get; set; }
        public string? TaiKhoan { get; set; }
        public string? UngDung { get; set; }
        public string? DeviceId { get; set; }
        public string? Roles { get; set; }
        public bool IsAdmin { get; set; }
        
        public Guid? UserId { get; set; }
        public string? Name { get; set; }
        public string? Username { get; set; }
        public string? ApplicationCode { get; set; }
        public string TraceId { get; set; } = Guid.NewGuid().ToString();

        public Dictionary<string, object> ExtraData { get; set; } = new Dictionary<string, object>();

        public HeaderContext() { }

        public HeaderContext(string? xUserHeader)
        {
            if (!string.IsNullOrEmpty(xUserHeader))
            {
                try
                {
                    ExtraData = JsonSerializer.Deserialize<Dictionary<string, object>>(xUserHeader) ?? new Dictionary<string, object>();
                    
                    if (ExtraData.TryGetValue("taiKhoanId", out var tkId) && tkId is JsonElement tkIdElem)
                        TaiKhoanId = tkIdElem.ToString();
                    
                    if (ExtraData.TryGetValue("taiKhoan", out var tk) && tk is JsonElement tkElem)
                        TaiKhoan = tkElem.ToString();

                    if (ExtraData.TryGetValue("ten", out var ten) && ten is JsonElement tenElem)
                        Ten = tenElem.ToString();

                    if (ExtraData.TryGetValue("ungDung", out var ud) && ud is JsonElement udElem)
                        UngDung = udElem.ToString();

                    if (ExtraData.TryGetValue("deviceId", out var did) && did is JsonElement didElem)
                        DeviceId = didElem.ToString();

                    if (ExtraData.TryGetValue("roles", out var r) && r is JsonElement rElem)
                        Roles = rElem.ToString();

                    if (ExtraData.TryGetValue("isAdmin", out var admin) && admin is JsonElement adminElem)
                        IsAdmin = adminElem.ValueKind == JsonValueKind.True;

                    if (Guid.TryParse(TaiKhoanId, out var uid))
                    {
                        UserId = uid;
                    }

                    Name = Ten;
                    Username = TaiKhoan;
                    ApplicationCode = UngDung;
                }
                catch
                {
                    // Handle or log parsing error if necessary
                    // For now, silently fail or partial population
                }
            }
        }
    }
}
