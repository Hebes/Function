using UnityEngine;

/// <summary>
/// 按下按钮的处理
/// </summary>
public class InputButtonProcessor
{
    public bool IsOpen = true;// 输入是否开启
    private bool _wasPressed;// 过去是否按下
    private bool _isPressed;// 是否按下
    private float _pressedTime;// 按下时间
    private int _pressedFrame;// 按下的帧
    private const float LongPressTime = 0.3f;// 长按时间
    public bool OnClick => OnReleased && _pressedTime < 0.3f;// 是否点击
    public bool OnPressed => Pressed && !_wasPressed;// 是否按下
    public bool OnReleased => Input1.JoystickIsOpen && IsOpen && !_isPressed && _wasPressed;// 是否释放了
    public bool Pressed => Input1.JoystickIsOpen && IsOpen && _isPressed;// 是否按住
    public bool LongPressed => Pressed && _pressedTime >= 0.3f;// 是否长时间按下
    public bool Released => Input1.JoystickIsOpen && IsOpen && !_isPressed;// 是否释放了
    
    /// <summary>
    /// 按下并持续几秒钟
    /// </summary>
    /// <param name="second"></param>
    /// <param name="accelerate"></param>
    /// <returns></returns>
    public bool OnPressedForSeveralSeconds(float second = 0.333333343f, bool accelerate = true)
    {
        int num = WorldTime.SecondToFrame(second);
        int num2 = WorldTime.SecondToFrame(0.0833333358f);
        int b = num2;
        if (accelerate)
        {
            int a = num - _pressedFrame / num2;
            num = Mathf.Max(a, b);
        }

        return OnPressed || (Pressed && _pressedFrame != 0 && _pressedFrame % num == 0);
    }

    public void Update(bool isPressed)
    {
        if (_wasPressed && isPressed)
        {
            _pressedTime += Time.unscaledDeltaTime;
            _pressedFrame++;
        }
        else if (!_wasPressed && !isPressed)
        {
            _pressedTime = 0f;
            _pressedFrame = 0;
        }

        _wasPressed = _isPressed;
        _isPressed = isPressed;
    }
}