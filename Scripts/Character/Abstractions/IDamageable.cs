using System.Threading;
using System.Threading.Tasks;

namespace Godot.RPG.Scripts.Character.Abstractions;

public interface IDamageable
{
    public Task<bool> TakeDamage(double damage, CancellationToken cancellationToken = default);
}