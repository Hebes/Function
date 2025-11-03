using UnityEngine;

/// <summary>
/// PC端移动
/// </summary>
public class PCController : IController
{
    private readonly IInputPlayer _player = new PCInputPlayer();

    public void Update()
    {
        Down.Update(_player.GetButton("Down"));
        Up.Update(_player.GetButton("Up"));
        Left.Update(_player.GetButton("Left"));
        Right.Update(_player.GetButton("Right"));
        Attack.Update(_player.GetButton("Attack"));
        Jump.Update(_player.GetButton("Jump"));
        Options.Update(_player.GetButton("Options") || UnityEngine.Input.GetKey(KeyCode.Escape));
        AnyKey.Update(Down.Pressed || Up.Pressed || Left.Pressed 
                      || Right.Pressed || Attack.Pressed || Jump.Pressed || Options.Pressed);
    }

    public static readonly InputKeyCodeProcessor Down = new InputKeyCodeProcessor(); //下
    public static readonly InputKeyCodeProcessor Up = new InputKeyCodeProcessor(); //上
    public static readonly InputKeyCodeProcessor Left = new InputKeyCodeProcessor(); //左
    public static readonly InputKeyCodeProcessor Right = new InputKeyCodeProcessor(); //右
    public static readonly InputKeyCodeProcessor Attack = new InputKeyCodeProcessor(); //攻击
    public static readonly InputKeyCodeProcessor Jump = new InputKeyCodeProcessor(); //跳跃
    public static readonly InputKeyCodeProcessor Options = new InputKeyCodeProcessor(); //选项
    public static readonly InputKeyCodeProcessor AnyKey = new InputKeyCodeProcessor(); //任何按键
}