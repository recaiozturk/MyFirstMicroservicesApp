


using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);


        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Configuration.AddJsonFile($"ocelot.json", false, true);
        builder.Services.AddOcelot(builder.Configuration);


        //protect
        var requiredAuthorizepolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
        builder.Services.AddAuthentication().AddJwtBearer("GatewayAuthenticationScheme", options =>
        {
            options.Authority = builder.Configuration["IdentityServerURL"];
            options.Audience = "resource_gateway";
            options.RequireHttpsMetadata = false;
        });


        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
        }
        app.UseOcelot().Wait();

        app.UseHttpsRedirection();

        //app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}


