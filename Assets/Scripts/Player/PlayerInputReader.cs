using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputReader : MonoBehaviour
{
    [Header("Input Actions")]
    public InputActionReference Move; // Vector2
    public InputActionReference Jump; // Button
    public InputActionReference vertical; // Optional: Vector2 or float
    public float Horizontal { get; private set; }
    public float VerticalAxis { get; private set; }
    public float Vertical => VerticalAxis;
    public bool JumpPressedThisFrame { get; private set; }
    private void OnEnable()
    {
        Move.action.Enable();
        Jump.action.Enable();
        if (vertical != null) vertical.action.Enable();
    }
    private void OnDisable()
    {
        Move.action.Disable();
        Jump.action.Disable();
        if (vertical != null) vertical.action.Disable();
    }
    private void Update()
    {
        var move = Move.action.ReadValue<Vector2>();
        Horizontal = move.x;
        if (vertical != null)
        {
            // If Vertical is bound as Vector2, take y; if float, switch this.
            var v = vertical.action.ReadValue<Vector2>();
            VerticalAxis = v.y;
        }
        JumpPressedThisFrame = Jump.action.WasPressedThisFrame();
    }
}
