using UnityEngine;

public sealed class PlayerGroundCheck : MonoBehaviour
{
    [Header("References")]
    public Transform groundCheck;

    [Header("Ground Check Control")]
    public float checkRadius = 0.15f;
    public LayerMask groundLayer;
    public bool IsGrounded;

    void Update()
    {
        IsGrounded = Physics2D.OverlapCircle(
            groundCheck.position,
            checkRadius,
            groundLayer
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