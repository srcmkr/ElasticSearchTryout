using ElasticSearchTryout.Models;
using ElasticSearchTryout.Services;
using Nest;

namespace ElasticSearchTryout;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddAuthorization();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        
        // Add ElasticClient
        var settings = new ConnectionSettings(new Uri("http://localhost:9200"))
            .DefaultMappingFor<DbUserModel>(m => m
                .IndexName("users")
            );
        var client = new ElasticClient(settings);
        client.Map<DbUserModel>(m => m.AutoMap());
        
        builder.Services.AddSingleton<IElasticClient>(client);
        builder.Services.AddScoped<ElasticService>();

        builder.Services.AddControllers();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();
        app.Run();
    }
}