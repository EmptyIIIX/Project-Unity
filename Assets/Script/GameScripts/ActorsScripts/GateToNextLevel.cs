using System;
using System.Collections;
using UnityEngine;

public class GateToNextLevel : MonoBehaviour
{
    public static event Action IntoGate;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(waitForNextLevel());
        }
    }

    private IEnumerator waitForNextLevel()
    {
        yield return new WaitForSeconds(3);

        IntoGate.Invoke();
    }
}
