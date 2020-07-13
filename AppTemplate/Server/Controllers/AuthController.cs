using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppTemplate.Shared.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Configuration;
using Microsoft.Extensions.Configuration;
using AppTemplate.Shared.Models;
using DapperWrapper.DBAccess;

namespace AppTemplate.Server.Controllers
{
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private IModel_RecordBase _model;

        public AuthController(IConfiguration configuration, IModel_RecordBase model)
        {
            _configuration = configuration;
            _model = model;
        }

        [HttpPost]
        [Route("api/auth/login")]
        public LoginResult Login(LoginCredentials credentials)
        {
            return ValidateCredentials(credentials)
                ? new LoginResult { Token = GenerateJWT(credentials.UserName) }
                : LoginResult.Unauthorized;
        }

        
        #region Private Methods
        private bool ValidateCredentials(LoginCredentials credentials)
        {
            var result = _model.LoadRecordWhere<UserAccount>(credentials.UserName, credentials.Password);

            return result.Result.Count < 1
                ? false
                : true;
        }

        private string GenerateJWT(string username)
        {
            // Also consider using AsymmetricSecurityKey if you want the client to be able to validate the token
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                new[] { new Claim(ClaimTypes.Name, username) },
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256));
            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }
        #endregion
    }
}
