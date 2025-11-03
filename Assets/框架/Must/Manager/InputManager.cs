using UnityEngine;

/// <summary>
/// 输入控制
/// </summary>
public class InputManager
{
    private static InputManager _i;
    public static InputManager I => _i ??= new InputManager();
    
    
}