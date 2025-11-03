using UnityEngine;

/// <summary>
/// 遥感的
/// </summary>
public interface IInputPlayer
{
    /// <summary>
    /// 获取游戏手柄
    /// </summary>
    /// <param name="axis"></param>
    /// <returns></returns>
    Vector2 GetJoystick(string axis);

    /// <summary>
    /// 获取原始游戏手柄数据
    /// </summary>
    /// <param name="axis"></param>
    /// <returns></returns>
    Vector2 GetJoystickRaw(string axis);

    /// <summary>
    /// 获取按钮
    /// </summary>
    /// <param name="buttonName"></param>
    /// <returns></returns>
    bool GetButton(string buttonName);

    /// <summary>
    /// 设置振动
    /// </summary>
    /// <param name="leftMotorValue"></param>
    /// <param name="rightMotorValue"></param>
    void SetVibration(float leftMotorValue, float rightMotorValue);
}