using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Goap行动
/// 来源：https://github.com/sploreg/goap
/// https://gamerboom.com/archives/83622
/// </summary>
public abstract class GoapAction : MonoBehaviour
{
    /// <summary>
    /// 构造函数
    /// </summary>
    public GoapAction()
    {
        preconditions = new HashSet<KeyValuePair<string, object>>();
        effects = new HashSet<KeyValuePair<string, object>>();
    }

    #region 字段

    /// <summary>
    /// 先决条件
    /// </summary>
    private HashSet<KeyValuePair<string, object>> preconditions;

    /// <summary>
    /// 影响
    /// </summary>
    private HashSet<KeyValuePair<string, object>> effects;

    /// <summary>
    /// 在范围内
    /// </summary>
    private bool inRange = false;

    /// <summary>
    /// 执行动作的成本。
    /// 计算出适合动作的重量。
    /// 改变它会影响在计划中选择的行动。
    /// </summary>
    public float cost = 1f;

    /// <summary>
    /// 一个动作通常必须在一个对象上执行。这就是那个物体。可以为空。
    /// </summary>
    public GameObject target;

    #endregion

    #region 属性

    /// <summary>
    /// 先决条件(复数)
    /// </summary>
    public HashSet<KeyValuePair<string, object>> Preconditions => preconditions;

    /// <summary>
    /// 影响(复数)
    /// </summary>
    public HashSet<KeyValuePair<string, object>> Effects => effects;

    /// <summary>
    /// 我们在目标的射程内吗?
    /// MoveTo状态会设置这个并且每次执行这个动作时它都会重置。
    /// </summary>
    /// <returns></returns>
    public bool isInRange() => inRange;

    #endregion

    #region 抽象方法

    /// <summary>
    /// 在计划再次发生之前，重置任何需要重置的变量。
    /// </summary>
    public abstract void reset();

    /// <summary>
    /// 动作完成了吗?
    /// </summary>
    /// <returns></returns>
    public abstract bool isDone();

    /// <summary>
    /// 前提条件(检查行动是否可以执行)
    /// </summary>
    /// <param name="agent"></param>
    /// <returns></returns>
    public abstract bool checkProceduralPrecondition(GameObject agent);

    /// <summary>
    /// 运行动作。
    /// 如果操作执行成功，返回True或false
    /// 如果发生了什么事情，它不能再执行。在这种情况下
    /// 动作队列清空，目标无法达到。
    /// </summary>
    /// <param name="agent"></param>
    /// <returns></returns>
    public abstract bool perform(GameObject agent);

    /// <summary>
    /// 这个动作是否需要在目标游戏对象的范围内?
    /// 如果不是，则不需要为该操作运行moveTo状态。
    /// </summary>
    /// <returns></returns>
    public abstract bool requiresInRange();

    #endregion

    #region 公开方法

    /// <summary>
    /// 设置目标的射程
    /// </summary>
    /// <param name="inRange"></param>
    public void setInRange(bool inRange)
    {
        this.inRange = inRange;
    }

    /// <summary>
    /// 重置
    /// </summary>
    public void doReset()
    {
        inRange = false;
        target = null;
        reset();
    }

    /// <summary>
    /// 添加先决条件
    /// </summary>
    /// <param name="key">先决条件的名称</param>
    /// <param name="value">先决条件的bool</param>
    public void addPrecondition(string key, object value)
    {
        preconditions.Add(new KeyValuePair<string, object>(key, value));
    }

    /// <summary>
    /// 删除先决条件
    /// </summary>
    /// <param name="key"></param>
    public void RemovePrecondition(string key)
    {
        KeyValuePair<string, object> remove = default(KeyValuePair<string, object>);
        foreach (KeyValuePair<string, object> kvp in preconditions)
        {
            if (kvp.Key.Equals(key))
                remove = kvp;
        }

        if (!default(KeyValuePair<string, object>).Equals(remove))
            preconditions.Remove(remove);
    }

    /// <summary>
    /// 添加对于自身的影响
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public void addEffect(string key, object value)
    {
        effects.Add(new KeyValuePair<string, object>(key, value));
    }

    /// <summary>
    /// 移除影响
    /// </summary>
    /// <param name="key"></param>
    public void removeEffect(string key)
    {
        KeyValuePair<string, object> remove = default(KeyValuePair<string, object>);
        foreach (KeyValuePair<string, object> kvp in effects)
        {
            if (kvp.Key.Equals(key))
                remove = kvp;
        }

        if (!default(KeyValuePair<string, object>).Equals(remove))
            effects.Remove(remove);
    }

    #endregion
}