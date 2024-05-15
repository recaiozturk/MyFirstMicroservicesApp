// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyMicroservice.IdentityServer
{
    public static class Config
    {
        public static IEnumerable<ApiResource> ApiResources = new ApiResource[]
        {
            new ApiResource("resource_catalog"){Scopes={"catalog_fullpermisiion" }},
            new ApiResource("resource_photo_stock"){Scopes={"photo_stock_fullpermisiion" }},
            new ApiResource("resource_basket"){Scopes={"basket_fullpermisiion" }},
            new ApiResource("resource_discount"){Scopes={"discount_fullpermisiion" }},
            new ApiResource("resource_order"){Scopes={"order_fullpermisiion" }},
            new ApiResource("resource_payment"){Scopes={" " }},
            new ApiResource("resource_gateway"){Scopes={"gateway_fullpermisiion" }},

            new ApiResource(IdentityServerConstants.LocalApi.ScopeName)
        };

        public static IEnumerable<IdentityResource> IdentityResources =>
                   new IdentityResource[]
                   {
                       new IdentityResources.Email(),
                       new IdentityResources.OpenId(),
                       new IdentityResources.Profile(),
                       new IdentityResource(){Name="roles",DisplayName="Roles",Description="Kullanıcı Rolleri",UserClaims=new []{"role"}}
                   };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("catalog_fullpermisiion","Full access for Catalog API"),
                new ApiScope("photo_stock_fullpermisiion","Full access for PhotoStock API"),
                new ApiScope("basket_fullpermisiion","Full access for Basket API"),
                new ApiScope("discount_fullpermisiion","Full access for Discount API"),
                new ApiScope("order_fullpermisiion","Full access for Order API"),
                new ApiScope("payment_fullpermisiion","Full access for Payment API"),
                new ApiScope("gateway_fullpermisiion","Full access for Payment API"),
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
                    AllowedScopes={ "catalog_fullpermisiion", "photo_stock_fullpermisiion", "gateway_fullpermisiion", IdentityServerConstants.LocalApi.ScopeName}
                },

                new Client
                {
                    ClientName="Asp Net Core MVC",
                    ClientId="WebMvcClientForUser",
                    ClientSecrets={new Secret ("secret".Sha256())},
                    AllowedGrantTypes=GrantTypes.ResourceOwnerPassword,
                    AllowedScopes={"basket_fullpermisiion","discount_fullpermisiion","order_fullpermisiion","payment_fullpermisiion","gateway_fullpermisiion",
                     IdentityServerConstants.StandardScopes.Email,IdentityServerConstants.StandardScopes.OpenId,
                     IdentityServerConstants.StandardScopes.Profile,IdentityServerConstants.StandardScopes.OfflineAccess,IdentityServerConstants.LocalApi.ScopeName,"roles"},
                    AllowOfflineAccess=true,
                    AccessTokenLifetime=1*60*60, //1 saat
                    RefreshTokenExpiration=TokenExpiration.Absolute,
                    AbsoluteRefreshTokenLifetime=(int)(DateTime.Now.AddDays(60)-DateTime.Now).TotalSeconds,
                    RefreshTokenUsage=TokenUsage.ReUse
                },
            };
    }
}