using UnityEngine;

/// <summary>
/// 虚拟控制器
/// </summary>
public class VirtualController : IController
{
    private static MobileInputPlayer Player => MobileInputPlayer.instance;

    public void Update()
    {
        Button1.Update(Player.GetButton("Button1"));
        Button2.Update(Player.GetButton("Button2"));
        Button3.Update(Player.GetButton("Button3"));
        Button4.Update(Player.GetButton("Button4"));
        Button5.Update(Player.GetButton("Button5"));
        Button6.Update(Player.GetButton("Options"));
        Options.Update(Player.GetButton("Options") || UnityEngine.Input.GetKey(KeyCode.Escape));
        AnyKey.Update(Button1.Pressed || Button2.Pressed || Button3.Pressed || Button4.Pressed ||
                                        Button5.Pressed || UnityEngine.Input.touchCount > 0);
        L2.Update(Player.GetButton("L2"));
        R2.Update(Player.GetButton("R2"));
        LeftJoystick.Update(Player.GetJoystick("Joystick"));
        LeftSwipe.Update(Player.GetJoystick("Swipe"));
    }

    public static readonly InputButtonProcessor AnyKey = new InputButtonProcessor();

    public static readonly InputButtonProcessor Button1 = new InputButtonProcessor();

    public static readonly InputButtonProcessor Button2 = new InputButtonProcessor();

    public static readonly InputButtonProcessor Button3 = new InputButtonProcessor();

    public static readonly InputButtonProcessor Button4 = new InputButtonProcessor();

    public static readonly InputButtonProcessor Button5 = new InputButtonProcessor();

    public static readonly InputButtonProcessor Button6 = new InputButtonProcessor();

    public static readonly InputButtonProcessor Options = new InputButtonProcessor();

    public static readonly InputButtonProcessor L2 = new InputButtonProcessor();

    public static readonly InputButtonProcessor R2 = new InputButtonProcessor();

    public static readonly InputJoystickProcessor LeftJoystick = new InputJoystickProcessor();

    public static readonly InputJoystickProcessor LeftSwipe = new InputJoystickProcessor();
}