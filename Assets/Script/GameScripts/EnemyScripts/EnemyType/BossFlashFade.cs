using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class BossFlashFade : MonoBehaviour
{
    public float Direction;

    private void OnEnable()
    {
        if (Direction == 1)
        {
            transform.localScale = new Vector3(-5f, 2.4f, 1f);
        }
        else
        {
            transform.localScale = new Vector3(5f, 2.4f, 1f);
        }

        Debug.Log("Flash is active");
    }

    public void DirectionFlash(float dir)
    {
        Direction = dir;
    }
}
