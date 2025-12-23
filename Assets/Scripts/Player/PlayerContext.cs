using UnityEngine;
public sealed class PlayerContext
{
    // Core refs
    public readonly Rigidbody2D Rb;
    public readonly Transform Transform;
    // Inputs (from Lab 02)
    public readonly PlayerInputReader Input;
    // Sensors (from Lab 02)
    public readonly PlayerGroundCheck Ground;
    public readonly LadderChecker Ladder;
    // Tunables
    public readonly float MoveSpeed;
    public readonly float JumpImpulse;
    public readonly float ClimbSpeed;
    // Optional: for debugging
    public readonly string DebugTag;
    public PlayerContext(
    Rigidbody2D rb,
    Transform transform,
    PlayerInputReader input,
    PlayerGroundCheck ground,
    LadderChecker ladder,
    float moveSpeed,
    float jumpImpulse,
    float climbSpeed,
    string debugTag = "PlayerFSM"
    )
    {
        Rb = rb;
        Transform = transform;
        Input = input;
        Ground = ground;
        Ladder = ladder;
        MoveSpeed = moveSpeed;
        JumpImpulse = jumpImpulse;
        ClimbSpeed = climbSpeed;
        DebugTag = debugTag;
    }
}