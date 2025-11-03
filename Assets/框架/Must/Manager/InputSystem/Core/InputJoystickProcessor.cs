using UnityEngine;

/// <summary>
/// 输入操纵杆处理器(UI)
/// </summary>
public class InputJoystickProcessor
{
    
    private const float Threshold = 0.0100000007f;
    private bool _wasPressed;// 过去是否按下
    private bool _isPressed;
    public bool IsOpen = true;
    public Vector2 Value;
    public Vector2 ValueRaw;
    public bool OnPressed => Input1.JoystickIsOpen && IsOpen && _isPressed && !_wasPressed;
    public bool Pressed => Input1.JoystickIsOpen && IsOpen && _isPressed;
    public bool OnReleased => Input1.JoystickIsOpen && IsOpen && !_isPressed && _wasPressed;
    public bool Released => Input1.JoystickIsOpen && IsOpen && !_isPressed;
    
    public void Update(Vector2 axis, Vector2 axisRaw = default(Vector2))
    {
        axis = new Vector2(UnityEngine.Input.GetAxis("Horizontal"), UnityEngine.Input.GetAxis("Vertical"));
        if (IsOpen)
        {
            Value = axis;
            ValueRaw = axisRaw;
        }
        else
        {
            Value = default(Vector2);
            ValueRaw = default(Vector2);
        }

        _wasPressed = _isPressed;
        _isPressed = (axis.sqrMagnitude >= 0.0100000007f);
    }

}