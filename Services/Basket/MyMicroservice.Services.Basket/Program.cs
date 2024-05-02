using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Options;
using MyMicroservice.Services.Basket.Services;
using MyMicroservice.Services.Basket.Settings;
using MyMicroService.Shared.Services;
using System.IdentityModel.Tokens.Jwt;

var builder = WebApplication.CreateBuilder(args);

//protect
var requiredAuthorizepolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();


//bunu eklemessek apiden gelen "sub" key .net tarafýnda name tarzýnda bir ada donusuyor, hata alýrýz
//JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("sub");


// Add services to the container.

builder.Services.AddControllers(opt =>
{
    opt.Filters.Add(new AuthorizeFilter(requiredAuthorizepolicy));
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//protect
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.Authority = builder.Configuration["IdentityServerURL"];
    options.Audience = "resource_basket";
    options.RequireHttpsMetadata = false;

    //bunu eklemessek apiden gelen "sub" key .net tarafýnda name tarzýnda bir ada donusuyor, hata alýrýz
    options.MapInboundClaims = false;
});

builder.Services.Configure<RedisSettings>(builder.Configuration.GetSection("RedisSettings"));
builder.Services.AddScoped<ISharerdIdentityService,SharedIdentityService>();
builder.Services.AddScoped<IBasketService, BasketService>();

builder.Services.AddSingleton<RedisService>(sp =>
{
    var redisSettings = sp.GetRequiredService<IOptions<RedisSettings>>().Value;
    var redis = new RedisService(redisSettings.Host, redisSettings.Port);
    redis.Connect();
    return redis;
});

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
