﻿namespace AppTemplate.Shared.Authentication
{
    public class LoginResult
    {
        public static readonly LoginResult Unauthorized = new LoginResult();

        public string Token { get; set; }
    }
}
