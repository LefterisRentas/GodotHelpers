// ReSharper disable once CheckNamespace
namespace Godot.RPG.Scripts.Character.Abstractions;

public interface ICharacterStatProvider
{
    double GetHealth();
    bool IsAlive();
    void Heal(double healAmount);
    void IncreaseStamina(double staminaAmount);
    void DecreaseStamina(double staminaAmount);
    double GetResource();
    void IncreaseResource(double amount);
    void DecreaseResource(double amount);
}