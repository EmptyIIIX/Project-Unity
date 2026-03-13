using UnityEngine;

public class TestFlash : MonoBehaviour
{
    private DamgeFlash _damgeFlash;
    private void Awake()
    {
        _damgeFlash = GetComponent<DamgeFlash>();
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            _damgeFlash.CallDamgeFlash();
        }
    }
}
