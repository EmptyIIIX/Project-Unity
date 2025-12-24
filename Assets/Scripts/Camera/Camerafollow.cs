using TMPro;
using UnityEngine;

public class Camerafollow : MonoBehaviour
{
    private Vector3 offset = new Vector3(0f, 0f, -1f);
    private float smoothTime = 0.25f;
    private Vector3 velocity = Vector3.zero;
    Vector3 targetPosition;
    public bool Active = false;

    [SerializeField] private Transform target;
    [SerializeField] private Transform roomtarget;

    private void Update()
    {
        if (!Active)
        {
            targetPosition = target.position + offset;
        }
        else
        {
            targetPosition = new Vector3((roomtarget.position.x / 2f),(roomtarget.position.y / 2f),(-1f));
        }
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("roomActive"))
        {
            Active = true;
        }
    }
}
