using GrpcTraining.Resources.OpenDota;

namespace GrpcTraining.Clients
{
    public interface IOpenDotaClient
    {
        Task<IEnumerable<OpenDotaHero>> GetHeroes();
    }
}
