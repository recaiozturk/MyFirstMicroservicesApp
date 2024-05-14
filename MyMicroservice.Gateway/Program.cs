


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


