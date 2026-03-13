using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class HoldToLoadLevel : MonoBehaviour
{
    public float holdDuration = 1f;
    public Image fillCircle;

    private float holdTimer = 0;
    private bool isHolding = false;

    public int healAmount = 1;
    public static event Action<int> OnHoldComplete;

    // Update is called once per frame
    void Update()
    {
        if (isHolding)
        {
            holdTimer += Time.deltaTime;
            fillCircle.fillAmount = holdTimer / holdDuration;
            if(holdTimer >= holdDuration)
            {
                //Load next level
                //OnHoldComplete.Invoke();                  use this for gate to next level

                //healing
                OnHoldComplete.Invoke(healAmount);
                ResetHold();
            }
        }
    }

    public void OnHold(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            isHolding = true;
        }
        else if (context.canceled)
        {
            ResetHold();
        }
    }

    private void ResetHold()
    {
        isHolding = false;
        holdTimer = 0;
        fillCircle.fillAmount = 0;
    }
}
