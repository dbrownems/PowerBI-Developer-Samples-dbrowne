// ----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.
// ----------------------------------------------------------------------------

using AppOwnsData.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using System;
using System.Threading.Tasks;

namespace AppOwnsData.Services
{
    public class UserMappingService
    {
        private string clientId;
        private string clientSecret;
        private string tenantId;
        

        public UserMappingService(IConfiguration config) 
        {
            var section = config.GetRequiredSection("EffectiveIdentityUser");
            clientId= section["ClientId"];
            clientSecret = section["ClientSecret"];
            tenantId = section["TenantId"];
        }

        private async Task<string> GetAccessToken(string clientId, string clientSecret, string tenantId)
        {

            //set scope for accessing Azure SQL Database
            string[] scopes = new string[] { "https://database.windows.net/.default" };

            // Create a confidential client to authorize the app with the AAD app
            IConfidentialClientApplication clientApp = ConfidentialClientApplicationBuilder
                                                                            .Create(clientId)
                                                                            .WithClientSecret(clientSecret)
                                                                            .WithAuthority(new Uri($"https://login.microsoftonline.com/{tenantId}/oauth2/token"))
                                                                            .Build();
            // Make a client call if Access token is not available in cache
            var authenticationResult = await clientApp.AcquireTokenForClient(scopes).ExecuteAsync();
            return authenticationResult.AccessToken;
        }

        private (string clientId, string clientSecret) LookupEffectiveIdentityUser(string userID)
        {
            //if the web user doesn't have report access throw an exception
       
            //lookup clientID and clientSecret for the user
            return (clientId, clientSecret);
        }

        //get access token for Azure SQL Database
        public async Task<string> GetAccessTokenForUser(string userID)
        {
            
            var (clientId, clientSecret) = LookupEffectiveIdentityUser(userID);

            var token = await GetAccessToken(clientId, clientSecret, tenantId);
            return token;
        }



    }

}