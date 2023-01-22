using Grpc.Core;
using GrpcTraining.Clients;
using GrpcTraining.Resources.OpenDota;
using System.Runtime.CompilerServices;

namespace GrpcTraining.Services
{
    public class DotaService : Dota.DotaBase
    {
        private readonly IOpenDotaClient _openDotaClient;
        private readonly ILogger<DotaService> _logger;

        public DotaService(IOpenDotaClient openDotaClient, ILogger<DotaService> logger)
        {
            _openDotaClient = openDotaClient;
            _logger = logger;
        }

        public override async Task<GetHeroesResponse> GetHeroes(GetHeroesRequest request, ServerCallContext context)
        {
            _logger.LogInformation("DotaService GetHeroes - test message", DateTime.UtcNow);

            var openDotaheroes = await _openDotaClient.GetHeroes();

            var heroes = MapHeroes(openDotaheroes);

            var response = new GetHeroesResponse();

            response.Heroes.AddRange(heroes);

            return await Task.FromResult(response);
        }

        private Hero[] MapHeroes(IEnumerable<OpenDotaHero> heroes)
        {
            return heroes.Select(h => new Hero
            {
                Id = h.Id,
                Name = h.Name,
                LocalName = h.LocalizedName,
                AttackType = h.AttackType,
                PrimaryAttribute = h.PrimaryAttr
            })
            .ToArray();
        }
    }
}
