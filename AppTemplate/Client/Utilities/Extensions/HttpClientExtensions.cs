using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading.Tasks;

namespace AppTemplate.Client.Utilities.Extensions
{
    public static class HttpClientExtensions
    {
        /* This provides a centralized location for setting authorziation to our httpclient.
         * You can use the following code to set authorization header within a component's code section
         * 
         *         var token = await TokenProvider.GetTokenAsync();
         *         Http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
         *
         *          agents = await Http.GetJsonAsync<Agent[]>("api/agents");
         */

        /// <summary>
        /// Sends an authorized GET request to an authorization requiring API.
        /// </summary>
        /// <typeparam name="T">Expected return type.</typeparam>
        /// <param name="httpClient">The HttpClient instance.</param>
        /// <param name="url">The secure API url.</param>
        /// <param name="authorization">The authentication header</param>
        /// <returns></returns>
        public static async Task<T> GetAuthorizedJsonAsync<T>(this HttpClient httpClient, string url, AuthenticationHeaderValue authorization)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Authorization = authorization;

            // send the request to the secure API
            var response = await httpClient.SendAsync(request);

            // receive deserialize response into a generic type (T). Usually "T" will represent a model type.
            var responseAsJsonBytes = await response.Content.ReadAsByteArrayAsync();
            return JsonSerializer.Deserialize<T>(responseAsJsonBytes,
                new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        }
    }
}
