using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Object = UnityEngine.Object;


[Serializable]
public class UIData
{
    public string key;
    public Object @object;

    public UIData(string keyValue,Object objectValue)
    {
        key = keyValue;
        @object = objectValue;
    }
}


public class UIComponent : MonoBehaviour
{
    public List<UIData> dataList = new List<UIData>();

    public GameObject Get(string key)
    {
        foreach (var data in dataList)
        {
            if (data.key != key) continue;
            return data.@object as GameObject;
        }

        return default;
    }

    public T GetComponent<T>(string key) where T : Component => Get(key).GetComponent<T>();
}