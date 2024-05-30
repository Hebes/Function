using UnityEngine;
using System.Collections;

/**
 * A tool used for mining ore and chopping wood.
 * Tools have strength that gets used up each time
 * they are used. When their strength is depleted
 * they should be destroyed by the user.
 *
 * 用于开采矿石和劈柴的工具。
 * 工具的力量每次都会消耗殆尽
 * 他们被使用。当他们的力量耗尽
 * 它们应该被用户销毁。
 *
 * 工具组件
 */
public class ToolComponent : MonoBehaviour
{
    [Header("工具完整度")]public float strength; // [0..1] or 0% to 100%

    void Start()
    {
        strength = 1; // full strength 完整度
    }

    /**
     * Use up the tool by causing damage. Damage should be a percent
     * 用完工具造成损坏。损失应该是百分之一
     * from 0 to 1, where 1 is 100%.
     * 从0到1,1等于100%
     */
    public void use(float damage)
    {
        strength -= damage;
    }

    /// <summary>
    /// 被毁的
    /// </summary>
    /// <returns></returns>
    public bool destroyed()
    {
        return strength <= 0f;
    }
}