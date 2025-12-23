using UnityEngine;
public sealed class IdleState : IPlayerState
{
    private readonly PlayerContext _ctx;
    private readonly PlayerStateMachine _sm;
    public IdleState(PlayerContext ctx, PlayerStateMachine sm)
    {
        _ctx = ctx;
        _sm = sm;
    }
    public void Enter()
    {
        Debug.Log($"[{_ctx.DebugTag}] Enter: Idle");
        var v = _ctx.Rb.velocity;
        _ctx.Rb.velocity = new Vector2(0f, v.y);
    }
    public void Tick(float dt)
    {
        // Transition rules (match Task 01)
        //if (_ctx.Ladder.IsOnLadder && Mathf.Abs(_ctx.Input.Vertical) > 0.01f)
        //{
        //    _sm.ChangeState(new ClimbState(_ctx, _sm));
        //    return;
        //}
        //if (_ctx.Input.JumpPressedThisFrame && _ctx.Ground.IsGrounded)
        //{
        //    _sm.ChangeState(new JumpState(_ctx, _sm));
        //    return;
        //}
        //if (Mathf.Abs(_ctx.Input.Horizontal) > 0.01f)
        //{
        //    _sm.ChangeState(new MoveState(_ctx, _sm));
        //    return;
        //}
    }
    public void Exit()
    {
        Debug.Log($"[{_ctx.DebugTag}] Exit: Idle");
    }
}