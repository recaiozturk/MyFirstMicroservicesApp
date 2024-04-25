// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;
using System.Linq;

namespace MyMicroservice.IdentityServer
{
    public static class Config
    {
        public static IEnumerable<ApiResource> ApiResources = new ApiResource[]
        {
            new ApiResource("resource_catalog"){Scopes={"catalog_fullpermisiion" }},
            new ApiResource("photo_stock_catalog"){Scopes={"photo_stock_fullpermisiion" }},
            new ApiResource(IdentityServerConstants.LocalApi.ScopeName)
        };

        public static IEnumerable<IdentityResource> IdentityResources =>
                   new IdentityResource[]
                   {
                   };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("catalog_fullpermisiion","Full access for Catalog API"),
                new ApiScope("photo_stock_fullpermisiion","Full access for PhotoStock API"),
                new ApiScope(IdentityServerConstants.LocalApi.ScopeName)
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                // m2m client credentials flow client
                new Client
                {
                    ClientName="Asp Net Core MVC",
                    ClientId="WebMvcClient",
                    ClientSecrets={new Secret ("secret".Sha256())},
                    AllowedGrantTypes=GrantTypes.ClientCredentials,
                    AllowedScopes={ "catalog_fullpermisiion", "photo_stock_fullpermisiion", IdentityServerConstants.LocalApi.ScopeName}
                },

                // interactive client using code flow + pkce
                new Client
                {
                },
            };
    }
}