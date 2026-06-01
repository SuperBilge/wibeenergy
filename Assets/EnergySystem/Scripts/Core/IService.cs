using System.Threading;
using Cysharp.Threading.Tasks;

namespace EnergySystem.Core
{
    public interface IService
    {
        UniTask InitializeAsync(CancellationToken ct);
        UniTask ReleaseAsync(CancellationToken ct);
    }
}