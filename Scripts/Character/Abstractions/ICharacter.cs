using System.Threading;
using System.Threading.Tasks;

// ReSharper disable once CheckNamespace
namespace Godot.RPG.Scripts.Character.Abstractions;

/// <summary>
/// Character interface.
/// Contains helper methods for managing a character.
/// </summary>
public interface ICharacter : IDamageable
{
    protected double Health { get; set; }
    protected double MaxHealth { get; set; }
    protected double Stamina { get; set; }
    protected double MaxStamina { get; set; }
    protected double Resource { get; set; }
    protected double MaxResource { get; set; }
    protected double MovementSpeed { get; set; }
    protected double Experience { get; set; }
    protected Level Level { get; set; }
    public bool IsAlive { get; protected set; }
    /// <summary>
    /// Heal the character by a specified amount.
    /// </summary>
    /// <param name="amount"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<bool> Heal(double amount, CancellationToken cancellationToken = default);
    /// <summary>
    /// Kill the character.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<bool> Die(CancellationToken cancellationToken = default);
    /// <summary>
    /// Revive the character.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<bool> Revive(CancellationToken cancellationToken = default);
    /// <summary>
    /// Level up the character.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<bool> LevelUp(CancellationToken cancellationToken = default);
    /// <summary>
    /// Gain experience.
    /// </summary>
    /// <param name="amount"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<bool> GainExperience(double amount, CancellationToken cancellationToken = default);
    /// <summary>
    /// Get the character's level.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Level> GetLevel(CancellationToken cancellationToken = default);
    /// <summary>
    /// Get the character's experience.
    /// </summary>
    /// <param name="amount"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<bool> SetExperience(double amount, CancellationToken cancellationToken = default);
    /// <summary>
    /// Get the character's health.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<double> GetMovementSpeed(CancellationToken cancellationToken = default);
}