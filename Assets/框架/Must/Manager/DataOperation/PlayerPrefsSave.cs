using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

/// <summary>
/// PlayerPrefs存储数据
/// </summary>
public class PlayerPrefsSave
{
    public static void MainSave(object data, string keyName)
    {
        //1.获取传入数据对象的所有字段
        Type dataType = data.GetType();
        FieldInfo[] infos = dataType.GetFields();
        //2.遍历这些字段 进行数据存储
        foreach (FieldInfo fieldInfo in infos)
        {
            string saveKeyName = $"{keyName}_{dataType.Name}_{fieldInfo.FieldType.Name}_{fieldInfo.Name}";
            SaveValue(saveKeyName, fieldInfo.GetValue(data));
        }

        PlayerPrefs.Save();
    }
    public static object MainLoad(Type type, string keyName)
    {
        //不用object对象传入 而使用 Type传入，有Type的 你只用传入 一个Type typeof(Player)
        object data = Activator.CreateInstance(type); //根据传入的Type 创建一个对象 用于存储数据       
        FieldInfo[] infos = type.GetFields(); //得到所有字段
        //用于拼接key的字符串
        //用于存储 单个字段信息的 对象
        for (int i = 0; i < infos.Length; i++)
        {
            FieldInfo info = infos[i];
            string loadKeyName = $"{keyName}_{type.Name}_{info.FieldType.Name}_{info.Name}"; //key的拼接规则 一定是和存储时一模一样 这样才能找到对应数据

            object value = LoadValue(loadKeyName, info.FieldType);
            info.SetValue(data, value); //填充数据到data中 
        }

        return data;
    }
    public void DeleteAll()
    {
        PlayerPrefs.DeleteAll();
    }
    public static void SaveValue(string saveKeyName, object value)
    {
        SafetyCheck();
        Type fieldType = value.GetType();
        foreach (IPlayerPrefs typeCheck in CheckList)
        {
            if (typeCheck.Check(fieldType))
            {
                typeCheck.Save(saveKeyName, value);
            }
        }
    }
    public static object LoadValue(string keyName, Type fieldType)
    {
        SafetyCheck();
        foreach (IPlayerPrefs typeCheck in CheckList)
        {
            if (typeCheck.Check(fieldType))
            {
                return typeCheck.Load(fieldType, keyName);
            }
        }

        return default;
    }

    
    
    private static readonly List<IPlayerPrefs> CheckList = new();
    public static void AddCheckType(params IPlayerPrefs[] playerPrefsSaveTypeCheck)
    {
        foreach (IPlayerPrefs value in playerPrefsSaveTypeCheck)
        {
            if (CheckList.Contains(value)) continue;
            CheckList.Add(value);
        }
    }
    private static void SafetyCheck()
    {
        if (CheckList.Count == 0)
            throw new Exception("请先调用AddCheckType方法");
    }
}

public interface IPlayerPrefs
{
    public void Save(string keyName, object value);

    public object Load(Type fieldType, string keyName);

    public bool Check(Type fieldType);
}
public class IntCheck : IPlayerPrefs
{
    public void Save(string keyName, object value)
    {
        Debug.Log($"存储int{keyName},值是{value}");
        PlayerPrefs.SetInt(keyName, (int)value);
    }

    public object Load(Type fieldType, string keyName)
    {
        return PlayerPrefs.GetInt(keyName, 0);
    }

    public bool Check(Type fieldType)
    {
        return fieldType == typeof(int);
    }
}
public class StringCheck : IPlayerPrefs
{
    public void Save(string keyName, object value)
    {
        Debug.Log($"存储string{keyName},值是{value}");
        PlayerPrefs.SetString(keyName, value.ToString());
    }

    public object Load(Type fieldType, string keyName)
    {
        return Check(fieldType) ? PlayerPrefs.GetString(keyName, "") : default(object);
    }

    public bool Check(Type fieldType)
    {
        return fieldType == typeof(string);
    }
}
public class FloatCheck : IPlayerPrefs
{
    public void Save(string keyName, object value)
    {
        Debug.Log($"存储float{keyName},值是{value}");
        PlayerPrefs.SetFloat(keyName, (float)value);
    }

    public object Load(Type fieldType, string keyName)
    {
        return Check(fieldType) ? PlayerPrefs.GetFloat(keyName, 0) : default(object);
    }

    public bool Check(Type fieldType)
    {
        return fieldType == typeof(float);
    }
}
public class BoolCheck : IPlayerPrefs
{
    public void Save(string keyName, object value)
    {
        Debug.Log($"存储bool{keyName},值是{value}");
        PlayerPrefs.SetInt(keyName, (bool)value ? 1 : 0); //自己定一个存储bool的规则
    }

    public object Load(Type fieldType, string keyName)
    {
        return PlayerPrefs.GetInt(keyName, 0) == 1;
    }

    public bool Check(Type fieldType)
    {
        return fieldType == typeof(bool);
    }
}
public class ListCheck : IPlayerPrefs
{
    public void Save(string keyName, object value)
    {
        Debug.Log("存储List" + keyName);
        IList list = value as IList; //父类装子类
        PlayerPrefs.SetInt(keyName, list.Count); //先存储 数量 
        int index = 0;
        for (int i = 0; i < list.Count; i++)
            PlayerPrefsSave.SaveValue($"{keyName}{i}", list[i]); //存储具体的值
    }

    public object Load(Type fieldType, string keyName)
    {
        int count = PlayerPrefs.GetInt(keyName, 0); //得到长度
        IList list = Activator.CreateInstance(fieldType) as IList; //实例化一个List对象 来进行赋值,用了反射中双A中 Activator进行快速实例化List对象
        for (int i = 0; i < count; i++)
        {
            //目的是要得到 List中泛型的类型 
            object obj = PlayerPrefsSave.LoadValue(keyName + i, fieldType.GetGenericArguments()[0]);
            list.Add(obj);
        }

        return list;
    }

    public bool Check(Type fieldType)
    {
        return typeof(IList).IsAssignableFrom(fieldType);
    }
}
public class DictionaryCheck : IPlayerPrefs
{
    public void Save(string keyName, object value)
    {
        Debug.Log("存储Dictionary" + keyName);
        IDictionary dic = value as IDictionary; //父类装自来
        PlayerPrefs.SetInt(keyName, dic.Count); //先存字典长度
        //遍历存储Dic里面的具体值
        //用于区分 表示的 区分 key
        int index = 0;
        foreach (object key in dic.Keys)
        {
            PlayerPrefsSave.SaveValue($"{keyName}_key_{index}", key);
            PlayerPrefsSave.SaveValue($"{keyName}_value_{index}", dic[key]);
            ++index;
        }
    }

    public object Load(Type fieldType, string keyName)
    {
        //得到字典的长度
        int count = PlayerPrefs.GetInt(keyName, 0);
        //实例化一个字典对象 用父类装子类
        IDictionary dic = Activator.CreateInstance(fieldType) as IDictionary;
        Type[] kvType = fieldType.GetGenericArguments();
        for (int i = 0; i < count; i++)
        {
            object o1 = PlayerPrefsSave.LoadValue($"{keyName}_key_{i}", kvType[0]);
            object o2 = PlayerPrefsSave.LoadValue($"{keyName}_value_{i}", kvType[1]);
            dic.Add(o1,o2);
        }

        return dic;
    }

    public bool Check(Type fieldType)
    {
        return typeof(IDictionary).IsAssignableFrom(fieldType);
    }
}
public class CommonCheck : IPlayerPrefs
{
    public void Save(string keyName, object value)
    {
        PlayerPrefsSave.MainSave(value, keyName);
    }

    public object Load(Type fieldType, string keyName)
    {
        return PlayerPrefsSave.MainLoad(fieldType, keyName);
    }

    public bool Check(Type fieldType)
    {
        return true;
    }
}