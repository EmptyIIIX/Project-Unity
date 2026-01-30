using UnityEngine;

public class HorizontalParallax : MonoBehaviour
{
    public Transform cameraTransform;
    public float parallaxMultiplier = 1f;
    private float startX;
    private float length;
    void Start()
    {
        startX = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void LateUpdate()
    {
        float cameraDeltaX = cameraTransform.position.x;
        transform.position = new Vector3(
        startX + cameraDeltaX * parallaxMultiplier,
        transform.position.y,
        transform.position.z
        );

        float movement = cameraTransform.transform.position.x * (1- parallaxMultiplier);

        if(movement > startX + length)
        {
            startX += length;
        }
        else if(movement < startX - length)
        {
            startX -= length;
        }
    }
}
