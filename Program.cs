using ItZnak.Infrastruction.Extentions;
using MatchEngineApi.Services;
using MatchEngineApi.Middleware;
using ItZnak.Infrastruction.Services;
using MatchEngineApi;
using MatchEngineApi.Controllers;
using  MatchEngineApi.Controllers.Tools;

RedisCache _cache;
IServiceProvider _serviceProvider;
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
builder.Services.AddSingleton<IInboundDbService, InboundDbService>();


var redisConfig = builder.Services
                        .GetService<IConfigService>()
                        .GetObject<RedisCacheConfig>(RedisCacheConfig.rootName);

_cache = new RedisCache(builder.Services, redisConfig);
builder.Services.AddSingleton<IDistributeCache>(_cache);

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

/* LOAD A CACHE */
_cacheLoader = new CacheLoader(builder.Services.GetService<IDistributeCache>(),
                               builder.Services.GetService<IInboundDbService>());
_cacheLoader.Run();

app.Run();