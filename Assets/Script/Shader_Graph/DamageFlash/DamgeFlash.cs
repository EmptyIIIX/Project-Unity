using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;

public class DamgeFlash : MonoBehaviour
{
    [ColorUsage(true,true)]
    [SerializeField] private Color _flashColor = Color.white;
    [SerializeField] private float _flashTime = 0.25f;
    [SerializeField] private AnimationCurve _flashSpeedCurve;

    private SpriteRenderer[] _spriteRenderers;
    private Material[] _materials;

    private Coroutine _damgeFlashCoroutine;

    private void Awake()
    {
        _spriteRenderers = GetComponentsInChildren<SpriteRenderer>();

        Init();
    }

    private void Init()
    {
        _materials = new Material[_spriteRenderers.Length];

        //assign sprite renderer materials to _materials
        for (int i = 0; i < _spriteRenderers.Length; i++)
        {
            _materials[i] = _spriteRenderers[i].material;
        }
    }

    public void CallDamgeFlash()
    {
        _damgeFlashCoroutine = StartCoroutine(DamageFlasher());
    }

    private IEnumerator DamageFlasher()
    {
        //set The color
        SetFlashColor();
        //lerp the flash amount
        float currentFlashAmount = 0f;
        float elapsedTime = 0f;
        while (elapsedTime < _flashTime)
        {
            //iterate elapsedTime
            elapsedTime += Time.deltaTime;
            //lerp the flash amount
            currentFlashAmount = Mathf.Lerp(1f, _flashSpeedCurve.Evaluate(elapsedTime), (elapsedTime / _flashTime));
            SetFlashAmount(currentFlashAmount);

            yield return null;
        }
    }
    private void SetFlashColor()
    {
        //set the color
        for (int i = 0; i < _materials.Length; i++) 
        {
            _materials[i].SetColor("_Flash",_flashColor);
        }
    }

    private void SetFlashAmount(float amount)
    {
        //set the flash amount
        for(int i = 0; i < _materials.Length; i++)
        {
            _materials[i].SetFloat("_Flashamount", amount);
        }
    }
}
