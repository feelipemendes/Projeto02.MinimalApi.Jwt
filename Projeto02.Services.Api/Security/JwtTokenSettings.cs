﻿namespace Projeto02.Services.Api.Security
{
    public class JwtTokenSettings
    {
        public string? SecretKey { get; set; }
        public int ExperationInHours { get; set; }
    }
}
