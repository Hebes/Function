using System;
using System.Collections;
using UnityEngine;
using Object = UnityEngine.Object;

//资源加载接口
public interface IResLoad
{
    /// <summary>
    /// 同步加载资源对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path"></param>
    /// <returns></returns>
    public T Load<T>(string path) where T : Object;

    /// <summary>
    /// 异步加载资源对象
    /// </summary>
    /// <param name="path"></param>
    /// <param name="action"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public IEnumerator LoadAsync<T>(string path, Action<T> action) where T : Object;


    /// <summary>
    /// 资源释放
    /// 可以在切换场景之后调用资源释放方法或者写定时器间隔时间去释放。
    /// 注意：只有调用资源释放方法，资源对象才会在内存里被移除。
    /// </summary>
    public void UnloadAsset(Object assetToUnload);
}

public class ResManager
{
    private static ResManager _i;
    public static ResManager I => _i ??= new ResManager();

    private readonly IResLoad _iLoad; //= 
}


public class UnityResLoad : IResLoad
{
    public T Load<T>(string path) where T : Object
    {
        return (T)Resources.Load(path, typeof(T));
    }

    public IEnumerator LoadAsync<T>(string path, Action<T> action) where T : Object
    {
        ResourceRequest request = Resources.LoadAsync<T>(path);
        yield return request;
        if (request.asset != null)
        {
            T loadedResource = request.asset as T;
            action?.Invoke(loadedResource);
        }
        else
        {
            Debug.LogError("资源加载失败: " + request);
        }
    }

    public void UnloadAsset(Object assetToUnload)
    {
        Resources.UnloadAsset(assetToUnload);
    }
}