using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
public sealed class ClimbState : IPlayerState
{
    private readonly PlayerContext _ctx;
    private readonly PlayerStateMachine _sm;
    public ClimbState(PlayerContext ctx, PlayerStateMachine sm)
    {
        _ctx = ctx;
        _sm = sm;
    }
    public void Enter()
    {
        Debug.Log($"[{_ctx.DebugTag}] Enter: Climb");
        _ctx.Rb.gravityScale = 0f;
        _ctx.Rb.velocity = Vector2.zero;
    }
    public void Tick(float dt)
    {
        // Exit rules
        //if (!_ctx.Ladder.IsOnLadder)
        //{
        //    _sm.ChangeState(new FallState(_ctx, _sm));
        //    return;
        //}
        //if (_ctx.Input.JumpPressedThisFrame)
        //{
        //    _sm.ChangeState(new JumpState(_ctx, _sm));
        //    return;
        //}
        // Movement on ladder (vertical only, simple)
        float vy = _ctx.ClimbSpeed * _ctx.Input.Vertical;
        _ctx.Rb.velocity = new Vector2(0f, vy);
        // If grounded and not pressing vertical, settle to Idle
        if (_ctx.Ground.IsGrounded && Mathf.Abs(_ctx.Input.Vertical) <= 0.01f)
        {
            _sm.ChangeState(new IdleState(_ctx, _sm));
        }
    }
    public void Exit()
    {
        Debug.Log($"[{_ctx.DebugTag}] Exit: Climb");
        _ctx.Rb.gravityScale = 1f; // restore default; if you store original, restore that instead
    }
}