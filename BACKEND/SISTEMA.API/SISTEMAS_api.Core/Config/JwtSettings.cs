using System;

namespace SISTEMA.API.SISTEMAS_api.Core.Config;

public class JwtSettings
{
    public string Key { get; set; } = string.Empty;
    public string Issuer { get; set; } = string.Empty;
}
