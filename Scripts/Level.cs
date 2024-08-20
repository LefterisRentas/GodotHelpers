namespace Godot.RPG.Scripts.Character;

/// <summary>
/// Represents the level of a character.
/// Each level has a specific experience threshold.
/// The experience threshold is the amount of experience required to reach the next level.
/// The value of the Level enum is the experience threshold.
/// </summary>
public enum Level
{
    One = 0,
    Two = 1000,
    Three = Two + 1000,
    Four = Three + 1000,
    Five = Four + 1000,
    Six = Five + 1000,
    Seven = Six + 1000,
    Eight = Seven + 1000,
    Nine = Eight + 1000,
    Ten = Nine + 1000,
}