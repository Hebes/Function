using System;
using System.Collections.Generic;

/// <summary>
/// 游戏事件
/// </summary>
public enum GameEvent
{
    /// <summary>
    /// 战斗评估
    /// </summary>
    Assessment = 1,
}

#region 有GC,拆装箱

public delegate void OnEventAction(params object[] udata);

/// <summary>
/// 事件管理器
/// </summary>
public class EventManager
{
    private static readonly Dictionary<Enum, EventData> EventDic = new Dictionary<Enum, EventData>();

    public static void Register(Enum enumValue, OnEventAction action)
    {
        if (!EventDic.ContainsKey(@enumValue))
            EventDic.Add(@enumValue, new EventData());
        EventDic[enumValue].Add(action);
    }
    public static void UnRegister(Enum enumValue, OnEventAction action)
    {
        if (!EventDic.ContainsKey(@enumValue)) return;
        EventDic[enumValue].UnAdd(action);
    }
    public static void Trigger(Enum enumValue, params object[] data)
    {
        if (EventDic.TryGetValue(enumValue, out EventData actionList))
            actionList?.Trigger(data);
    }
}


/// <summary>
/// 事件数据
/// </summary>
public class EventData
{
    private readonly List<OnEventAction> _actionList = new List<OnEventAction>();

    public void Add(OnEventAction action)
    {
        if (_actionList.Contains(action)) return;
        _actionList.Add(action);
    }

    public void UnAdd(OnEventAction action)
    {
        _actionList.Remove(action);
    }

    public void Trigger(params object[] data)
    {
        for (var i = 0; i < _actionList.Count; i++)
            _actionList[i].Invoke(data);
    }
}

#endregion

#region 无GC,拆箱装箱

public class EventManager2
{
    private static EventManager2 _i;
    public static EventManager2 I => _i ??= new EventManager2();

    //**************事件回收对象池**************
    //public  Dictionary<Enum, Queue<IEvenData>> EventPool = new Dictionary<Enum, Queue<IEvenData>>();
    private readonly Dictionary<string, Queue<IEvenData>> _eventPool = new Dictionary<string, Queue<IEvenData>>();

    private T Get<T>() where T : class
    {
        string keyName = typeof(T).FullName;
        if (string.IsNullOrEmpty(keyName))
            throw new Exception("未知错误异常");
        if (!_eventPool.TryGetValue(keyName, out Queue<IEvenData> evenDates)) return null;
        return evenDates.TryDequeue(out IEvenData obj) ? (T)obj : null;
    }

    public void Push(IEvenData evenData)
    {
        string keyName = evenData.GetType().FullName;
        if (string.IsNullOrEmpty(keyName))
            throw new Exception("未知错误异常");
        if (!_eventPool.ContainsKey(keyName))
            _eventPool.Add(keyName,new Queue<IEvenData>());
        _eventPool[keyName].Enqueue(evenData);
    }

    //**************事件监听池**************
    private readonly Dictionary<Enum, IEvenData> _eventListenDic = new Dictionary<Enum, IEvenData>();

    public void Register(Enum enumValue, Action action)
    {
        if (_eventListenDic.TryGetValue(enumValue, out IEvenData iEvenData))
        {
            ((EvenData)iEvenData).ActionValue = action;
        }
        else
        {
            EvenData eventInfo = Get<EvenData>() ?? new EvenData();
            eventInfo.ActionValue = action;
            _eventListenDic.Add(enumValue, eventInfo);
        }
    }
    public void Register<TAction>(Enum enumValue, TAction action) where TAction : MulticastDelegate
    {
        if (_eventListenDic.TryGetValue(enumValue, out IEvenData evenData))
        {
            var info = (MultipleParameterEventInfo<TAction>)evenData;
            info.ActionValue = action;
        }
        else
        {
            var newEventInfo = Get<MultipleParameterEventInfo<TAction>>() ?? new MultipleParameterEventInfo<TAction>();
            newEventInfo.ActionValue = action;
            _eventListenDic.Add(enumValue, newEventInfo);
        }
    }
    public void Register1<T0, T1>(Enum enumValue, Action<T0, T1> action) => Register(enumValue, action);
    public void Register1<T0, T1, T2>(Enum enumValue, Action<T0, T1, T2> action) => Register(enumValue, action);
    public void Trigger(Enum enumValue, Action action)
    {
        if (_eventListenDic.TryGetValue(enumValue, out IEvenData iEvenData))
            ((EvenData)iEvenData).ActionValue?.Invoke();
    }
    public void Trigger<T>(Enum enumValue, T arg)
    {
        if (_eventListenDic.TryGetValue(enumValue, out IEvenData evenData))
            ((MultipleParameterEventInfo<Action<T>>)evenData).ActionValue?.Invoke(arg);
    }
    public void Trigger<T0, T1>(Enum enumValue, T0 arg0, T1 arg1)
    {
        if (_eventListenDic.TryGetValue(enumValue, out IEvenData evenData))
            ((MultipleParameterEventInfo<Action<T0, T1>>)evenData).ActionValue?.Invoke(arg0, arg1);
    }
    public void Trigger<T0, T1, T2>(Enum enumValue, T0 arg0, T1 arg1, T2 arg2)
    {
        if (_eventListenDic.TryGetValue(enumValue, out IEvenData evenData))
            ((MultipleParameterEventInfo<Action<T0, T1, T2>>)evenData).ActionValue?.Invoke(arg0, arg1, arg2);
    }
    public void UnRegister(Enum enumValue, Action action)
    {
        if (_eventListenDic.TryGetValue(enumValue, out IEvenData eventInfo))
            ((EvenData)eventInfo).ActionValue -= action;
    }
    public void UnRegister<TAction>(Enum enumValue, TAction action) where TAction : MulticastDelegate
    {
        if (_eventListenDic.TryGetValue(enumValue, out IEvenData eventInfo))
        {
            MultipleParameterEventInfo<TAction> info = (MultipleParameterEventInfo<TAction>)eventInfo;
            info.ActionValue = (TAction)Delegate.Remove(info.ActionValue, action);
        }
    }
    public void RemoveEvent(Enum eventName)
    {
        if (_eventListenDic.Remove(eventName, out IEvenData eventInfo))
            eventInfo.Destroy();
    }
    public void Clear()
    {
        foreach (Enum eventName in _eventListenDic.Keys)
            _eventListenDic[eventName].Destroy();
        _eventListenDic.Clear();
    }
}

public interface IEvenData
{
    public void Destroy();
}

/// <summary>
/// 无参
/// </summary>
public class EvenData : IEvenData
{
    public Action ActionValue;

    public void Destroy()
    {
        ActionValue = null;
        EventManager2.I.Push(this);
    }
}

/// <summary>
/// 多参Action事件信息
/// </summary>
/// <typeparam name="TAction"></typeparam>
public class MultipleParameterEventInfo<TAction> : IEvenData where TAction : MulticastDelegate
{
    public TAction ActionValue;

    public void Destroy()
    {
        ActionValue = null;
        EventManager2.I.Push(this);
    }
}

#endregion


/// <summary>
/// 全称EventExpand 事件拓展
/// </summary>
public static class EventExpand
{
    public static void Register(this Enum enumValue, OnEventAction action) => EventManager.Register(enumValue, action);
    public static void UnRegister(this Enum enumValue, OnEventAction action) => EventManager.UnRegister(enumValue, action);
    public static void Trigger(this Enum enumValue, params object[] data) => EventManager.Trigger(enumValue, data);
}