using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using System.Text.Json;

namespace AppTemplate.Client.Utilities.Authentication
{
    public class TokenAuthenticationStateProvider : AuthenticationStateProvider
    {
        private IJSRuntime _jsRuntime;

        // requires DI of jsRuntime to handle state 
        public TokenAuthenticationStateProvider(IJSRuntime jsRuntime)
            => _jsRuntime = jsRuntime;

        /// <summary>
        ///  Retreives token from browser's local storage.
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetTokenAsync() 
            => await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "authToken");
        
        /// <summary>
        /// Adds or removes token from browser's local storage and refreshes app authentication state.
        /// </summary>
        /// <param name="token">The token received from the server.</param>
        /// <returns></returns>
        public async Task SetTokenAsync(string token)
        {
            if (token == null)
                await _jsRuntime.InvokeAsync<object>("localStorage.removeItem", "authToken");
            else
                await _jsRuntime.InvokeAsync<object>("localStorage.setItem", "authToken", token);

            // cascade new authentication state throughout our app
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        /// <summary>
        /// Retreives current session's authentication state.
        /// </summary>
        /// <returns></returns>
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var sJwt = await GetTokenAsync();
            
            // create new or restore existing identity
            // to restore we must parse the claims from the stringified json web token ("jwt")
            var identity = string.IsNullOrEmpty(sJwt)
                ? new ClaimsIdentity()
                : new ClaimsIdentity(ParseClaimsFromJwt(sJwt), "jwt");

            // instantiate new auth state using using the new/restored claims identity and pass to caller to refresh app state
            return new AuthenticationState(new ClaimsPrincipal(identity));
        }

        #region Private Methods

        private static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            // split string into an array of string and select the item in index 1
            var payload = jwt.Split('.')[1];
            // converty string into an array of bytes (which is in json formatting)
            var jsonBytes = ParseBase64WithoutPadding(payload);
            // deserialize the json bytes as a dictionary collection of type string & object
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
            // convert the <string, object> collection into a collection of Claims
            return keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()));
        }

        private static byte[] ParseBase64WithoutPadding(string base64)
        {
            // get the remainder of the length divided by 4
            switch (base64.Length % 4)
            {
                // if the remainder is 2 then add "==" to the end of base64 string
                case 2: base64 += "=="; break;
                // if the remainder is 3 then add "==" to the end of base64 string
                case 3: base64 += "="; break;
                    // else don't add anything
            }
            return Convert.FromBase64String(base64);
        }
        #endregion
    }
}
