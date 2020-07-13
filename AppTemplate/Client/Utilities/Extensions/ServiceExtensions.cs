using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Components.Authorization;
using AppTemplate.Client.Utilities.Authentication;

namespace AppTemplate.Client.Utilities
{
    public static class ServiceExtensions
    {
        /// <summary>
        /// An extension method for type of IServiceCollection. Use this to add a scoped instance
        /// of "TokenAuthenticationStateProvider". Injecting "AuthenticationStateProvider" will be
        /// synonymous to injecting "TokenAuthenticationStateProvider" when this service is added.
        /// </summary>
        /// <param name="services"></param>
        public static void AddTokenAuthenticationStateProvider(this IServiceCollection services)
        {
            // Make the same instance accessible as both AuthenticationStateProvider and TokenAuthenticationStateProvider
            services.AddScoped<TokenAuthenticationStateProvider>();
            services.AddScoped<AuthenticationStateProvider>(provider => provider.GetRequiredService<TokenAuthenticationStateProvider>());
            // this means even if we inject AuthenticationStateProvider into a component it will still be used as TokenAuthenticationStateProvider
            // i'm still not sure why he did this though?
        }

 
    }
}
