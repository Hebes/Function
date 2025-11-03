using UnityEngine;

/// <summary>
/// 输入轴处理器
/// </summary>
public class InputAxisProcessor
{
    private const float Threshold = 0.1f;
    private bool _wasPressed;
    private bool _isPressed;
    public bool IsOpen = true;
    public float Value;
    public float ValueRaw;
    public bool isReverse;
    public bool OnPressed => Input1.JoystickIsOpen && IsOpen && _isPressed && !_wasPressed;
    public bool Pressed => Input1.JoystickIsOpen && IsOpen && _isPressed;
    public bool OnReleased => Input1.JoystickIsOpen && IsOpen && !_isPressed && _wasPressed;
    public bool Released => Input1.JoystickIsOpen && IsOpen && !_isPressed;

    public void Update(float axis, float axisRaw = 0f)
    {
        if (IsOpen)
        {
            Value = axis * ((!isReverse) ? 1 : -1);
            ValueRaw = axisRaw * ((!isReverse) ? 1 : -1);
        }
        else
        {
            Value = 0f;
            ValueRaw = 0f;
        }

        _wasPressed = _isPressed;
        _isPressed = (Mathf.Abs(axisRaw) >= 0.1f);
    }
}