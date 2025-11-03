using System;
using UnityEngine;

public class PS4InputPlayer : IInputPlayer
{
    public bool GetButton(string buttonName)
    {
        switch (buttonName)
        {
            case "DpadLeft": return UnityEngine.Input.GetAxisRaw("dpad1_horizontal") < -0.2f;
            case "DpadRight": return UnityEngine.Input.GetAxisRaw("dpad1_horizontal") > 0.2f;
            case "DpadUp": return UnityEngine.Input.GetAxisRaw("dpad1_vertical") > 0.2f;
            case "DpadDown": return UnityEngine.Input.GetAxisRaw("dpad1_vertical") < -0.2f;
            case "LSLeft": return UnityEngine.Input.GetAxisRaw("leftstick1horizontal") < -0.002f;
            case "LSRight": return UnityEngine.Input.GetAxisRaw("leftstick1horizontal") > 0.002f;
            case "LSUp": return UnityEngine.Input.GetAxisRaw("leftstick1vertical") < -0.002f;
            case "LSDown": return UnityEngine.Input.GetAxisRaw("leftstick1vertical") > 0.002f;
            case "Cross": return UnityEngine.Input.GetKey(KeyCode.Joystick1Button0);
            case "Circle": return UnityEngine.Input.GetKey(KeyCode.Joystick1Button1);
            case "Triangle": return UnityEngine.Input.GetKey(KeyCode.Joystick1Button3);
            case "Square": return UnityEngine.Input.GetKey(KeyCode.Joystick1Button2);
            case "R1": return UnityEngine.Input.GetKey(KeyCode.Joystick1Button5);
            case "L1": return UnityEngine.Input.GetKey(KeyCode.Joystick1Button4);
            case "R2": return UnityEngine.Input.GetAxisRaw("joystick1_right_trigger") < -0.1f;
            case "L2": return UnityEngine.Input.GetAxisRaw("joystick1_left_trigger") > 0.1f;
            case "R3": return UnityEngine.Input.GetKey(KeyCode.Joystick1Button8);
            case "L3": return UnityEngine.Input.GetKey(KeyCode.Joystick1Button9);
            case "Options": return UnityEngine.Input.GetKey(KeyCode.Joystick1Button7);
        }

        throw new ArgumentOutOfRangeException(nameof(buttonName), buttonName, $"Button \"{buttonName}\" is not exist.");
    }

    public Vector2 GetJoystick(string axis)
    {
        if (axis != null)
        {
            switch (axis)
            {
                case "LS":
                    return new Vector2(UnityEngine.Input.GetAxis("leftstick1horizontal"), UnityEngine.Input.GetAxis("leftstick1vertical"));
                case "RS":
                    return new Vector2(UnityEngine.Input.GetAxis("rightstick1horizontal"), UnityEngine.Input.GetAxis("rightstick1vertical"));
            }
        }

        throw new ArgumentOutOfRangeException("axis", axis);
    }

    public Vector2 GetJoystickRaw(string axis)
    {
        if (axis != null)
        {
            switch (axis)
            {
                case "LS":
                    return new Vector2(UnityEngine.Input.GetAxisRaw("leftstick1horizontal"), UnityEngine.Input.GetAxisRaw("leftstick1vertical"));
                case "RS":
                    return new Vector2(UnityEngine.Input.GetAxisRaw("rightstick1horizontal"), UnityEngine.Input.GetAxisRaw("rightstick1vertical"));
            }
        }

        throw new ArgumentOutOfRangeException("axis", axis);
    }

    public void SetVibration(float leftMotorValue, float rightMotorValue)
    {
    }

    public static class DSKeyCode1
    {
        public const KeyCode Cross = KeyCode.Joystick1Button0;

        public const KeyCode Circle = KeyCode.Joystick1Button1;

        public const KeyCode Square = KeyCode.Joystick1Button2;

        public const KeyCode Triangle = KeyCode.Joystick1Button3;

        public const KeyCode L1 = KeyCode.Joystick1Button4;

        public const KeyCode R1 = KeyCode.Joystick1Button5;

        public const KeyCode TouchPadButton = KeyCode.Joystick1Button6;

        public const KeyCode Options = KeyCode.Joystick1Button7;

        public const KeyCode L3 = KeyCode.Joystick1Button8;

        public const KeyCode R3 = KeyCode.Joystick1Button9;
    }

    public static class DSAxis1
    {
        public const string DpadHorizontal = "dpad1_horizontal";

        public const string DpadVertical = "dpad1_vertical";

        public const string LeftStickHorizontal = "leftstick1horizontal";

        public const string LeftStickVertical = "leftstick1vertical";

        public const string RightStickHorizontal = "rightstick1horizontal";

        public const string RightStickVertical = "rightstick1vertical";

        public const string L2 = "joystick1_left_trigger";

        public const string R2 = "joystick1_right_trigger";
    }
}