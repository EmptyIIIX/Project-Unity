using UnityEngine;
public sealed class PlayerFSMController : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private PlayerInputReader input;
    [SerializeField] private PlayerGroundCheck ground;
    [SerializeField] private LadderChecker ladder;
    [Header("Tuning")]
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float jumpImpulse = 12f;
    [SerializeField] private float climbSpeed = 4f;
    private PlayerStateMachine _sm;
    private PlayerContext _ctx;
    private void Awake()
    {
        _sm = new PlayerStateMachine();
        _ctx = new PlayerContext(
        rb,
        transform,
        input,
        ground,
        ladder,
        moveSpeed,
        jumpImpulse,
        climbSpeed,
        debugTag: "Lab03"
        );
    }
    private void Start()
    {
        _sm.ChangeState(new IdleState(_ctx, _sm));
    }
    private void Update()
    {
        _sm.Tick(Time.deltaTime);
    }
}
