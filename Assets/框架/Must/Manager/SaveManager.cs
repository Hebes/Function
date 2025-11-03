using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

/// <summary>
/// 存档系统
/// </summary>
public class SaveManager
{
    private static SaveManager _i;
    public static SaveManager I => _i ??= new SaveManager();


    private static SaveSystemData _saveSystemData;
    private static string saveDirPath = $"{Application.persistentDataPath}/saveData";
    private static string settingDirPath = $"{Application.persistentDataPath}/setting";
    public ESaveSystemType SaveSystemType = ESaveSystemType.Binary;
    private static IBinarySerializer binarySerializer;

    public enum ESaveSystemType
    {
        Binary,
        Json
    }

    /// <summary>
    /// 存档系统数据类
    /// </summary>
    [Serializable]
    private class SaveSystemData
    {
        // 当前的存档ID
        public int currID = 0;

        // 所有存档的列表
        public List<SaveData> SaveItemList = new List<SaveData>();
    }

    public void Init()
    {
        CheckAndCreateDir(); // 检查路径
        InitSaveSystemData(); // 初始化SaveSystemData
    }
    private void InitSaveSystemData()
    {
        _saveSystemData = LoadFile<SaveSystemData>($"{saveDirPath}/SaveSystemData");
        if (_saveSystemData != null) return;
        _saveSystemData = new SaveSystemData();
        UpdateSaveSystemData();
    }



    /// <summary>
    /// 添加一个存档
    /// </summary>
    /// <returns></returns>
    public SaveData CreateSaveItem()
    {
        SaveData saveItem = new SaveData(_saveSystemData.currID, DateTime.Now);
        _saveSystemData.SaveItemList.Add(saveItem);
        _saveSystemData.currID += 1;
        // 更新SaveSystemData 写入磁盘
        UpdateSaveSystemData();
        return saveItem;
    }
    /// <summary>
    /// 删除存档
    /// </summary>
    /// <param name="saveID">存档的ID</param>
    public void DeleteSaveItem(int saveID)
    {
        string itemDir = GetSavePath(saveID, false);
        // 如果路径存在 且 有效
        if (!string.IsNullOrEmpty(itemDir))
            Directory.Delete(itemDir, true); // 把这个存档下的文件递归删除
        _saveSystemData.SaveItemList.Remove(GetSaveItem(saveID));
        UpdateSaveSystemData(); // 更新SaveSystemData 写入磁盘
    }
    /// <summary>
    /// 删除所有数据
    /// </summary>
    public void DeleteAllSaveItem()
    {
        if (Directory.Exists(saveDirPath))
        {
            Directory.Delete(saveDirPath, true); // 直接删除目录
        }

        CheckAndCreateDir();
        InitSaveSystemData();
    }
    /// <summary>
    /// 删除某个存档中的某个对象
    /// </summary>
    /// <param name="saveID">存档的ID</param>
    public void DeleteObject<T>(string saveFileName, int saveID) where T : class
    {
        string dirPath = GetSavePath(saveID);// 存档对象所在的文件路径
        string savePath = $"{dirPath}/{saveFileName}";
        File.Delete(savePath); //删除对应的文件
    }
    /// <summary>
    /// 保存对象至某个存档中
    /// </summary>
    /// <param name="saveObject">要保存的对象</param>
    /// <param name="saveFileName">保存的文件名称</param>
    /// <param name="saveID">存档的ID</param>
    public void SaveObject(object saveObject, string saveFileName, int saveID = 0)
    {
        // 存档所在的文件夹路径
        string dirPath = GetSavePath(saveID, true);
        // 具体的对象要保存的路径
        string savePath = dirPath + "/" + saveFileName;
        SaveFile(saveObject, savePath); // 具体的保存
        GetSaveItem(saveID).UpdateTime(DateTime.Now); // 更新存档时间
        UpdateSaveSystemData(); // 更新SaveSystemData 写入磁盘
    }
    /// <summary>
    /// 从某个具体的存档中加载某个对象
    /// </summary>
    /// <typeparam name="T">要返回的实际类型</typeparam>
    /// <param name="saveFileName">文件名称</param>
    /// <param name="id">存档ID</param>
    public T LoadObject<T>(string saveFileName, int saveID = 0) where T : class
    {
        string dirPath = GetSavePath(saveID); // 存档所在的文件夹路径
        if (dirPath == null) return null;
        string savePath = $"{dirPath}/{saveFileName}"; // 具体的对象要保存的路径
        T obj = LoadFile<T>(savePath);
        return obj;
    }
    /// <summary>
    /// 获取所有存档,最新的在最后面
    /// </summary>
    /// <returns></returns>
    public static List<SaveData> GetAllSaveItem() => _saveSystemData.SaveItemList;
    /// <summary>
    /// 获取SaveItem
    /// </summary>
    public SaveData GetSaveItem(int id)
    {
        foreach (SaveData savaData in _saveSystemData.SaveItemList)
        {
            if (savaData.SaveID != id) continue;
            return savaData;
        }

        return null;
    }

    

    /// <summary>
    /// 加载设置，全局生效，不关乎任何一个存档
    /// </summary>
    public T LoadSetting<T>(string fileName) where T : class
    {
        return LoadFile<T>($"{settingDirPath}/{fileName}");
    }
    /// <summary>
    /// 保存设置，全局生效，不关乎任何一个存档
    /// </summary>
    public void SaveSetting(object saveObject, string fileName)
    {
        SaveFile(saveObject, settingDirPath + "/" + fileName);
    }
    /// <summary>
    /// 删除全局设置
    /// </summary>
    public void DeleteAllSetting()
    {
        if (Directory.Exists(settingDirPath))
            Directory.Delete(settingDirPath, true);// 直接删除目录
        CheckAndCreateDir();
    }
    
    
    
    /// <summary>
    /// 更新存档系统数据
    /// </summary>
    private void UpdateSaveSystemData()
    {
        SaveFile(_saveSystemData, $"{saveDirPath}/SaveSystemData");
    }
    /// <summary>
    /// 获取某个存档的路径
    /// </summary>
    /// <param name="saveID">存档ID</param>
    /// <param name="createDir">如果不存在这个路径，是否需要创建</param>
    /// <returns></returns>
    private string GetSavePath(int saveID, bool createDir = true)
    {
        // 验证是否有某个存档
        if (GetSaveItem(saveID) == null) throw new Exception("JK:saveID 存档不存在！");

        string saveDir = saveDirPath + "/" + saveID;
        // 确定文件夹是否存在
        if (Directory.Exists(saveDir)) return saveDir;
        if (createDir)
        {
            Directory.CreateDirectory(saveDir);
        }
        else
        {
            return null;
        }

        return saveDir;
    }
    /// <summary>
    /// 检查路径并创建目录
    /// </summary>
    private void CheckAndCreateDir()
    {
        // 确保路径的存在
        if (Directory.Exists(saveDirPath) == false)
            Directory.CreateDirectory(saveDirPath);
        if (Directory.Exists(settingDirPath) == false)
            Directory.CreateDirectory(settingDirPath);
    }
    /// <summary>
    /// 加载文件
    /// </summary>
    /// <param name="path"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    private T LoadFile<T>(string path) where T : class
    {
        switch (SaveSystemType)
        {
            case ESaveSystemType.Binary:
                // 避免框架内部的数据类型也使用外部序列化工具序列化，这一般都会出现问题
                if (binarySerializer == null || typeof(T) == typeof(SaveSystemData)) return IOTool.LoadFile<T>(path);
                else
                {
                    FileStream file = new FileStream(path, FileMode.Open);
                    byte[] bytes = new byte[file.Length];
                    file.Read(bytes, 0, bytes.Length);
                    file.Close();
                    return binarySerializer.Deserialize<T>(bytes);
                }
            case ESaveSystemType.Json:
                return IOTool.LoadJson<T>(path);
            default:
                throw new Exception("未实现加载方式");
        }
    }
    /// <summary>
    /// 保存文件
    /// </summary>
    /// <param name="saveObject">保存的对象</param>
    /// <param name="path">保存的路径</param>
    private void SaveFile(object saveObject, string path)
    {
        switch (SaveSystemType)
        {
            case ESaveSystemType.Binary:
                if (binarySerializer == null || saveObject.GetType() == typeof(SaveSystemData)) IOTool.SaveFile(saveObject, path);
                else
                {
                    byte[] bytes = binarySerializer.Serialize(saveObject);
                    File.WriteAllBytes(path, bytes);
                }

                break;
            case ESaveSystemType.Json:
                string jsonData = JsonUtility.ToJson(saveObject);
                IOTool.SaveJson(jsonData, path);
                break;
        }
    }
}

/// <summary>
/// 一个存档的数据
/// </summary>
public class SaveData
{
    public int SaveID;
    private DateTime _lastSaveTime;
    [SerializeField] private string _lastSaveTimeString; // Json不支持DateTime，用来持久化的

    public DateTime LastSaveTime
    {
        get
        {
            if (_lastSaveTime == default(DateTime))
            {
                DateTime.TryParse(_lastSaveTimeString, out _lastSaveTime);
            }

            return _lastSaveTime;
        }
    }

    public SaveData(int saveID, DateTime lastSaveTime)
    {
        SaveID = saveID;
        _lastSaveTime = lastSaveTime;
        _lastSaveTimeString = lastSaveTime.ToString();
    }

    public void UpdateTime(DateTime lastSaveTime)
    {
        _lastSaveTime = lastSaveTime;
        _lastSaveTimeString = lastSaveTime.ToString();
    }
}

public interface IBinarySerializer
{
    public byte[] Serialize<T>(T obj) where T : class;
    public T Deserialize<T>(byte[] bytes) where T : class;
}

public static class IOTool
{
    private static BinaryFormatter binaryFormatter = new BinaryFormatter();

    /// <summary>
    /// 保存Json
    /// </summary>
    /// <param name="jsonString">Json的字符串</param>
    /// <param name="path">路径</param>
    public static void SaveJson(string jsonString, string path)
    {
        File.WriteAllText(path, jsonString);
    }

    /// <summary>
    /// 读取Json为指定的类型对象
    /// </summary>
    public static T LoadJson<T>(string path) where T : class
    {
        return File.Exists(path) ? JsonUtility.FromJson<T>(File.ReadAllText(path)) : null;
    }

    /// <summary>
    /// 保存文件
    /// </summary>
    /// <param name="saveObject">保存的对象</param>
    /// <param name="path">保存的路径</param>
    public static void SaveFile(object saveObject, string path)
    {
        FileStream f = new FileStream(path, FileMode.OpenOrCreate);
        // 二进制的方式把对象写进文件
        binaryFormatter.Serialize(f, saveObject);
        f.Dispose();
    }

    /// <summary>
    /// 加载文件
    /// </summary>
    /// <typeparam name="T">加载后要转为的类型</typeparam>
    /// <param name="path">加载路径</param>
    public static T LoadFile<T>(string path) where T : class
    {
        if (!File.Exists(path))
        {
            return null;
        }

        FileStream file = new FileStream(path, FileMode.Open);
        // 将内容解码成对象
        T obj = (T)binaryFormatter.Deserialize(file);
        file.Dispose();
        return obj;
    }
}