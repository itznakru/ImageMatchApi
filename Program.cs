using ItZnak.Infrastruction.Extentions;
using MatchEngineApi.Services;
using MatchEngineApi.Middleware;
using ItZnak.Infrastruction.Services;
using MatchEngineApi;
using MatchEngineApi.Controllers;
using MatchEngineApi.Controllers.Tools;
using System.Net;
using Infrastraction.Services.MemoryCache;

internal class Program
{
    private static void Main(string[] args)
    {
        const int HTTP_PORT = 8888;
        //RedisCache _cache;
        MemoryCache<byte[]> _cache;
        CacheLoader _cacheLoader;

        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        /* Add my services */

        builder.Services.AddLogService();
        builder.Services.AddConfigService();
        builder.Services.AddScoped<IInboundDbService, InboundDbService>();

        var redisConfig = builder.Services
                                .GetService<IConfigService>()
                                .GetObject<RedisCacheConfig>(RedisCacheConfig.rootName);

        _cache = new MemoryCache<byte[]>();
        builder.Services.AddSingleton<IMemoryCache<byte[]>>(_cache);

        /* ConfigureKestrel*/
        builder.WebHost.ConfigureKestrel(options => options.Listen(IPAddress.Any, HTTP_PORT));

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        //app.UseHttpsRedirection();

        app.UseAuthorization();
        app.MapControllers();
        app.UseExceptionHandlerMdl();
        app.UseWriteBalancerMdl();

        /* SET ENVIROMENT */
        new InfrastractionSettings(builder.Services.GetService<IConfigService>(),
                                   builder.Services.GetService<ILogService>()).ApplaySettings();

        /* LOAD A CACHE */
        _cacheLoader = new CacheLoader(builder.Services.GetService<IMemoryCache<byte[]>>(),
                                       builder.Services.GetService<IInboundDbService>(),
                                       builder.Services.GetService<ILogService>());
        _cacheLoader.Run();

        app.Run();
    }
}