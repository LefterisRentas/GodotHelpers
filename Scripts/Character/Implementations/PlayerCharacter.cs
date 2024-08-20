using System.Threading;
using System.Threading.Tasks;
using Godot.RPG.Scripts.Character.Abstractions;

// ReSharper disable once CheckNamespace
namespace Godot.RPG.Scripts.Character.Implementations;

public partial class PlayerCharacter : Node, ICharacter
{
    [Export] public double Health { get; set; }
    [Export]public double MaxHealth { get; set; }
    [Export] public double Stamina { get; set; }
    [Export] public double MaxStamina { get; set; }
    [Export] public double Resource { get; set; }
    [Export] public double MaxResource { get; set; }
    [Export] public double MovementSpeed { get; set; }
    [Export] public double Experience { get; set; }
    [Export] public Level Level { get; set; }
    [Export] public bool IsAlive { get; set; }
    [Export] public uint Attack { get; set; }
    [Export] public int Defense { get; set; }
    [Export] public int Magic { get; set; }
    [Export] public int Luck { get; set; }
    [Export] public ushort SkillPoints { get; set; }
    [Export] public uint ExperienceThreshold { get; set; }
    
    public async Task<bool> TakeDamage(double damage, CancellationToken cancellationToken = default)
    {
        if (IsAlive)
        {
            Health -= damage;
            if (Health <= 0)
            {
                await Die(cancellationToken);
            }
            return true;
        }
        return false;
    }
    
    public Task<bool> Heal(double amount, CancellationToken cancellationToken = default)
    {
        if (IsAlive)
        {
            Health += amount;
            if (Health > MaxHealth)
            {
                Health = MaxHealth;
            }
            return Task.FromResult(true);
        }
        return Task.FromResult(false);
    }
    
    public Task<bool> Die(CancellationToken cancellationToken = default)
    {
        if (IsAlive)
        {
            IsAlive = false;
            Health = 0;
            return Task.FromResult(true);
        }
        return Task.FromResult(false);
    }
    
    public Task<bool> Revive(CancellationToken cancellationToken = default)
    {
        if (!IsAlive)
        {
            IsAlive = true;
            Health = MaxHealth;
            return Task.FromResult(true);
        }
        return Task.FromResult(false);
    }
    
    public Task<bool> LevelUp(CancellationToken cancellationToken = default)
    {
        if (Experience >= ExperienceThreshold)
        {
            Experience -= ExperienceThreshold;
            switch (Level)
            {
                case Level.Ten:
                    return Task.FromResult(false);
                case Level.One:
                    Level = Level.Two;
                    break;
                case Level.Two:
                    Level = Level.Three;
                    break;
                case Level.Three:
                    Level = Level.Four;
                    break;
                case Level.Four:
                    Level = Level.Five;
                    break;
                case Level.Five:
                    Level = Level.Six;
                    break;
                case Level.Six:
                    Level = Level.Seven;
                    break;
                case Level.Seven:
                    Level = Level.Eight;
                    break;
                case Level.Eight:
                    Level = Level.Nine;
                    break;
                case Level.Nine:
                    Level = Level.Ten;
                    break;
            }

            SkillPoints += 5;
            ExperienceThreshold = (uint)Level;
            return Task.FromResult(true);
        }
        return Task.FromResult(false);
    }
    
    public Task<bool> GainExperience(double amount, CancellationToken cancellationToken = default)
    {
        Experience += amount;
        return Task.FromResult(true);
    }
    
    public Task<Level> GetLevel(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(Level);
    }
    
    public Task<bool> SetExperience(double amount, CancellationToken cancellationToken = default)
    {
        Experience = amount;
        return Task.FromResult(true);
    }

    public Task<double> GetMovementSpeed(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(MovementSpeed);
    }
}