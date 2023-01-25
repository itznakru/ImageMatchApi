using MatchEngineApi.DTO;
using Microsoft.EntityFrameworkCore;

namespace MatchEngineApi.Services
{
    public interface IInboundDbService
    {
        DbSet<VectorDto> VECTORS { get; set; }
        DbContext CTX { get; }
    }
}