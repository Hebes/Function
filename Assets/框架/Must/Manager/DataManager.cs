using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

/// <summary>
/// 数据句柄
/// </summary>
public interface IDataHandle
{
    public abstract void Save(object obj, string fileName);

    public abstract T Load<T>(string fileName) where T : class;
}

public class DataManager
{
}

/// <summary>
/// 二进制
/// </summary>
public class Binary : IDataHandle
{
    public void Save(object obj, string fileName)
    {
        throw new System.NotImplementedException();
    }

    public T Load<T>(string path) where T : class
    {
        string filePath = $"{path}.bytes";
        if (File.Exists(filePath))
        {
            using FileStream fs = File.Open(filePath, FileMode.Open, FileAccess.Read);
            BinaryFormatter bf = new BinaryFormatter();
            return bf.Deserialize(fs) as T;
        }

        return default(T);
    }
}