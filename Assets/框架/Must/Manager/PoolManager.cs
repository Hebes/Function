using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class PoolManager
{
    private static PoolManager _i;
    public static PoolManager I => _i ??= new PoolManager();


    private Dictionary<string, string> _objPathDic; //加载物体的路径字典
    private Dictionary<string, Queue<IPool>> _poolsDic; //回收池字典
    private GameObject _poolObjParent; //父物体


    public T GetMono<T>(Func<string, GameObject> loadRes) where T : Component, IPool
    {
        IPool value = TryGetValue(nameof(T), _poolsDic);

        if (value == null)
        {
            GameObject gameObject = loadRes?.Invoke(_objPathDic[nameof(T)]);
            value = Instantiate<T>(gameObject);
        }

        value.Get();
        return value as T;
    }

    public void GetMonoAsync<T>(Action loadAsync) where T : Component, IPool
    {
        string key = nameof(T);
        IPool value = TryGetValue(key, _poolsDic);
        value?.Get();
        if (value == null)
            loadAsync?.Invoke();
    }

    public void Push<T>(T t) where T : IPool
    {
        string name = nameof(T);
        if (_poolsDic.ContainsKey(name))
        {
            _poolsDic[name].Enqueue(t);
        }
        else
        {
            Queue<IPool> queue = new Queue<IPool>();
            queue.Enqueue(t);
            _poolsDic.Add(name, queue);
        }

        //if (typeof(T).IsSubclassOf(typeof(Component))){}
        t.Push();
    }

    public T GetClass<T>() where T : class, IPool, new()
    {
        IPool value = TryGetValue(nameof(T), _poolsDic) ?? new T();
        value.Get();
        return value as T;
    }

    public void Clear()
    {
        foreach (Queue<IPool> value in _poolsDic.Values)
            value.Clear();
    }

    public void CheckAndDestroy(string key)
    {
        //TODO 实际中在编写
    }


    public void SetParentMono<T>(T t) where T : Component, IPool
    {
        if (_poolObjParent == null)
        {
            _poolObjParent = new GameObject("PoolParent");
        }

        Transform transform = _poolObjParent.transform.Find(nameof(T));
        if (transform == null)
        {
            transform = new GameObject(nameof(T)).transform;
            transform.SetParent(_poolObjParent.transform, false);
        }

        t.transform.SetParent(transform, false);
    }

    public string GetPath(string key) => _objPathDic[key];
    public void SetPathDic(Dictionary<string, string> dic) => _objPathDic = dic;

    private T Instantiate<T>(GameObject obj) where T : Component, IPool
    {
        GameObject gameObjectTemp = Object.Instantiate(obj);
        T t = gameObjectTemp.GetComponent<T>();
        return t == null ? gameObjectTemp.AddComponent<T>() : t;
    }

    private IPool TryGetValue(string key, Dictionary<string, Queue<IPool>> poolDic)
    {
        //如果缓存池中有的话
        if (!poolDic.TryGetValue(key, out Queue<IPool> data)) return null;
        return data.Count > 0 ? data.Dequeue() : null;
    }
}

/// <summary>
/// 对象池接口
/// </summary>
public interface IPool
{
    /// <summary> 从对象池出来之后需要做的事 </summary>
    public void Get();

    /// <summary> 进对象池之前需要做的事 </summary>
    public void Push();
}