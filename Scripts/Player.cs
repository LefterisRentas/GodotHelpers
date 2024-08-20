using System;
using System.Diagnostics;
using Godot;
using Godot.RPG.Scripts.Character.Abstractions;
using Godot.RPG.Scripts.Character.Implementations;

namespace project_1.Scripts;

public partial class Player : CharacterBody3D
{
    [Export] private Node3D _playerCharacterReference;
    private ICharacter _character;

    private CharacterBody3D _characterBody3D;

    #region Camera
    
    private Camera3D _camera3D;
    [Export] private Vector3 _camera3dPersonOffset = new(0, 1.5f, -5);
    [Export] private Vector3 _cameraTopDownOffset = new(0, 10, 0);
    [Export] private Vector3 _cameraFirstPersonOffset = new(0, 1.5f, 0);
    private bool _freeLookEnabled;

    [Export] private float _mouseSensitivity = 0.1f;
    private float _cameraPitch; // To track the vertical rotation (pitch)
    
    #endregion

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        // Check if the character is already set
        if (_playerCharacterReference == null)
        {
            _character = GetNode<ICharacter>("PlayerCharacter");
            if (_character == null)
            {
                _character = new PlayerCharacter();
                AddChild(_character as Node);
                Console.Error.WriteLine("Player Character not found. Created a new one.");
            }
        }
        else
        {
            _character = _playerCharacterReference.GetNode<ICharacter>("PlayerCharacter");
        }

        _characterBody3D = this;

        _camera3D = GetNode<Camera3D>("Head/Camera3D");

        Input.MouseMode = Input.MouseModeEnum.Captured; // Capture the mouse for camera control

        Debug.WriteLine("Player Ready");
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        Move(delta);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        base._Process(delta);
        RotateCamera(delta);
    }

    private async void Move(double delta)
    {
        double moveSpeed = await _character.GetMovementSpeed();
        Vector3 velocity = _characterBody3D.Velocity;
        if (CanMove())
        {
            Vector3 movementDirection = Vector3.Zero;

            if (Input.IsActionPressed("MoveForward"))
            {
                movementDirection += Vector3.Forward;
            }
            if (Input.IsActionPressed("MoveBackward"))
            {
                movementDirection += Vector3.Back;
            }
            if (Input.IsActionPressed("MoveLeft"))
            {
                movementDirection += Vector3.Left;
            }
            if (Input.IsActionPressed("MoveRight"))
            {
                movementDirection += Vector3.Right;
            }

            // Normalize the movement direction to ensure consistent speed in all directions
            if (movementDirection != Vector3.Zero)
            {
                movementDirection = movementDirection.Normalized();
            }

            // Calculate the horizontal velocity (x and z) while preserving the y velocity
            Vector3 horizontalVelocity = movementDirection * (float)moveSpeed;

            // Combine the horizontal movement with the existing y velocity (gravity)
            velocity = new Vector3(horizontalVelocity.X, velocity.Y, horizontalVelocity.Z);

            // Use MoveAndCollide to move the character, allowing collisions to be handled by the physics engine
        }
        _characterBody3D.MoveAndCollide(velocity * (float)delta);
    }

    private float OsuSensitivityToMultiplier(float osuSensitivity)
    {
        // Assuming osuSensitivity is the multiplier, where 1.0 is default sensitivity
        return osuSensitivity * 0.05f; // Adjust the factor to suit the desired speed, 0.5f as a starting point
    }

private void RotateCamera(double delta)
{
    _freeLookEnabled = Input.IsKeyPressed(Key.Alt);

    if (IsInMenu()) return;

    // Apply osu! sensitivity calculation
    float effectiveSensitivity = OsuSensitivityToMultiplier(_mouseSensitivity);

    // Get the mouse motion scaled by the calculated sensitivity and delta
    Vector2 mouseMotion = Input.GetLastMouseVelocity() * effectiveSensitivity * (float)delta;

    if (_freeLookEnabled)
    {
        // Rotate the camera independently (free look)
        _camera3D.RotateY(Mathf.DegToRad(-mouseMotion.X));

        // Adjust vertical rotation (pitch) and clamp it
        _cameraPitch -= mouseMotion.Y;
        _cameraPitch = Mathf.Clamp(_cameraPitch, -89.0f, 89.0f);

        // Apply the pitch rotation to the camera
        _camera3D.RotationDegrees = new Vector3(_cameraPitch, _camera3D.RotationDegrees.Y, _camera3D.RotationDegrees.Z);
    }
    else
    {
        // Rotate the player horizontally (yaw)
        _characterBody3D.RotateY(Mathf.DegToRad(-mouseMotion.X));

        // Adjust vertical rotation (pitch) and clamp it
        _cameraPitch -= mouseMotion.Y;
        _cameraPitch = Mathf.Clamp(_cameraPitch, -89.0f, 89.0f);

        // Apply the pitch rotation to the camera, but keep yaw aligned with player
        _camera3D.RotationDegrees = new Vector3(_cameraPitch, _characterBody3D.RotationDegrees.Y, 0.0f);
    }
}




    private bool CanMove()
    {
        // This is a simple check to see if the player is grounded or not
        // If you want the player to move in the air, you need to modify this check.
        // The check should be if the player's y velocity is larger or equal to 0, then the player is grounded or is moving up.
        return _characterBody3D.Velocity.Y > -0.01f && _characterBody3D.Velocity.Y < 0.01f;
    }

    private static bool IsInMenu()
    {
        // This is a simple check to see if the player is in a menu or not
        // You can modify this check to fit your game's needs
        return false;
    }
}
