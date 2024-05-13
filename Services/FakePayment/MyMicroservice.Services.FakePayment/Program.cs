using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//protect
var requiredAuthorizepolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.Authority = builder.Configuration["IdentityServerURL"];
    options.Audience = "resource_payment";
    options.RequireHttpsMetadata = false;

    //bunu eklemessek apiden gelen "sub" key .net tarafýnda name tarzýnda bir ada donusuyor, hata alýrýz
    options.MapInboundClaims = false;
});

builder.Services.AddControllers(opt =>
{
    opt.Filters.Add(new AuthorizeFilter(requiredAuthorizepolicy));
});

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
