using Grpc.Core;
using GrpcTraining.Clients;
using GrpcTraining.Resources.OpenDota;
using System.Runtime.CompilerServices;

namespace GrpcTraining.Services
{
    public class DotaService : Dota.DotaBase
    {
        private readonly IOpenDotaClient _openDotaClient;

        public DotaService(IOpenDotaClient openDotaClient)
        {
            _openDotaClient = openDotaClient;
        }

        public override async Task<GetHeroesResponse> GetHeroes(GetHeroesRequest request, ServerCallContext context)
        {
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
