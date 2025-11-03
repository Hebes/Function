public class DS4Controller : IController
{
    private readonly IInputPlayer _player = new PS4InputPlayer();

    public static readonly InputButtonProcessor AnyKey = new InputButtonProcessor();

    public static readonly InputButtonProcessor Down = new InputButtonProcessor();

    public static readonly InputButtonProcessor Up = new InputButtonProcessor();

    public static readonly InputButtonProcessor Left = new InputButtonProcessor();

    public static readonly InputButtonProcessor Right = new InputButtonProcessor();

    public static readonly InputButtonProcessor Cross = new InputButtonProcessor();

    public static readonly InputButtonProcessor Circle = new InputButtonProcessor();

    public static readonly InputButtonProcessor Triangle = new InputButtonProcessor();

    public static readonly InputButtonProcessor Square = new InputButtonProcessor();

    public static readonly InputButtonProcessor R1 = new InputButtonProcessor();

    public static readonly InputButtonProcessor R2 = new InputButtonProcessor();

    public static readonly InputButtonProcessor R3 = new InputButtonProcessor();

    public static readonly InputButtonProcessor L1 = new InputButtonProcessor();

    public static readonly InputButtonProcessor L2 = new InputButtonProcessor();

    public static readonly InputButtonProcessor L3 = new InputButtonProcessor();

    public static readonly InputButtonProcessor Options = new InputButtonProcessor();

    public static readonly InputButtonProcessor LSDown = new InputButtonProcessor();

    public static readonly InputButtonProcessor LSUp = new InputButtonProcessor();

    public static readonly InputButtonProcessor LSLeft = new InputButtonProcessor();

    public static readonly InputButtonProcessor LSRight = new InputButtonProcessor();

    public static readonly InputJoystickProcessor LS = new InputJoystickProcessor();

    public static readonly InputJoystickProcessor RS = new InputJoystickProcessor();
    
    public void Update()
    {
        bool button = _player.GetButton("DpadLeft");
        bool button2 = _player.GetButton("DpadRight");
        bool button3 = _player.GetButton("DpadUp");
        bool button4 = _player.GetButton("DpadDown");
        bool button5 = _player.GetButton("LSLeft");
        bool button6 = _player.GetButton("LSRight");
        bool button7 = _player.GetButton("LSUp");
        bool button8 = _player.GetButton("LSDown");
        bool button9 = _player.GetButton("Cross");
        bool button10 = _player.GetButton("Circle");
        bool button11 = _player.GetButton("Triangle");
        bool button12 = _player.GetButton("Square");
        bool button13 = _player.GetButton("R1");
        bool button14 = _player.GetButton("L1");
        bool button15 = _player.GetButton("L2");
        bool button16 = _player.GetButton("R2");
        bool button17 = _player.GetButton("L3");
        bool button18 = _player.GetButton("R3");
        bool button19 = _player.GetButton("Options");
        Down.Update(button4);
        Up.Update(button3);
        Left.Update(button);
        Right.Update(button2);
        Cross.Update(button9);
        Circle.Update(button10);
        Triangle.Update(button11);
        Square.Update(button12);
        R1.Update(button13);
        R2.Update(button16);
        R3.Update(button18);
        L1.Update(button14);
        L2.Update(button15);
        L3.Update(button17);
        Options.Update(button19);
        LSDown.Update(button8);
        LSUp.Update(button7);
        LSLeft.Update(button5);
        LSRight.Update(button6);
        bool isPressed = Down.Pressed || Up.Pressed || Left.Pressed || Right.Pressed || Cross.Pressed ||
                         Circle.Pressed || Triangle.Pressed || Square.Pressed || R1.Pressed || R2.Pressed ||
                         R3.Pressed || L1.Pressed || L2.Pressed || L3.Pressed || LSDown.Pressed || LSUp.Pressed ||
                         LSLeft.Pressed || LSRight.Pressed;
        AnyKey.Update(isPressed);
        LS.Update(_player.GetJoystick("LS"), _player.GetJoystickRaw("LS"));
        RS.Update(_player.GetJoystick("RS"), _player.GetJoystickRaw("RS"));
    }

    
}