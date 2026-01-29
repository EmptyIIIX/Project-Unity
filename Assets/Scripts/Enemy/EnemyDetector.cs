using UnityEngine;

public class EnemyDetector : MonoBehaviour
{
    [Header("References")]
    public Transform detector;

    [Header("Detector Control")]
    public LayerMask playerLayer;
    [SerializeField] private float sizeX;
    [SerializeField] private float sizeY;

    public bool isPlayerDetected;

    // Update is called once per frame
    void Update()
    {
        Vector2 SizeDet = new Vector2(sizeX, sizeY);
        isPlayerDetected = Physics2D.OverlapBox(detector.position, SizeDet, 0, playerLayer);
    }
    private void OnDrawGizmosSelected()
    {
        Vector2 SizeDet = new Vector2(sizeX, sizeY);
        if (detector == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(detector.position, SizeDet);
    }
}
