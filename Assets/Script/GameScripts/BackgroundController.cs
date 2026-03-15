using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    private float startPos;
    private float length;
    public GameObject cam;
    public float parallacEffect;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float distance = cam.transform.position.x * parallacEffect;
        float movement = cam.transform.position.x * (1 - parallacEffect);

        transform.position = new Vector3(startPos + distance, transform.position.y, transform.position.z);

        //cam bounds background
        if(movement > startPos + length)
        {
            startPos += length;
        }
        else if(movement < startPos - length)
        {
            startPos -= length;
        }
    }
}
