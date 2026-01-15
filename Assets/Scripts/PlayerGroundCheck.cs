using UnityEngine;

public sealed class PlayerGroundCheck : MonoBehaviour
{
    [Header("References")]
    public Transform groundCheck;
    public PhysicsMaterial2D PhyMat;

    [Header("Ground Check Control")]
    public float checkRadius = 0.15f;
    public LayerMask groundLayer;
    public LayerMask platformLayer;
    public LayerMask clingLayer;
    public bool IsGrounded;
    public bool IsClingable;

    void Update()
    {
        IsGrounded = Physics2D.OverlapCircle(
            groundCheck.position,
            checkRadius,
            groundLayer
        );

        IsClingable = Physics2D.OverlapCircle(
            groundCheck.position,
            checkRadius,
            clingLayer
            );
    }
    void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
            Gizmos.color = Color.yellow;
            
        Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
    }
}
public sealed class LadderChecker : MonoBehaviour
{
    public bool IsOnLadder { get; private set; }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ladder")) IsOnLadder = true;
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Ladder")) IsOnLadder = false;
    }
}