using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using LitJson;
using UnityEngine;

public class JsonSave
{
    public string Serialize(object obj)
    {
        if (obj == null) return "null";
        Type type = obj.GetType();

        // 检查是否有自定义序列化器
        if (_serializers.TryGetValue(type, out Func<object, string> serializer))
            return serializer(obj);
        
        // 处理数组
        if (type.IsArray)
            return Array_Serialize(obj);
        
        // 处理列表
        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
            return List_Serialize(obj);
        
        // 处理字典（简化版）
        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Dictionary<,>))
            return Dictionary_Serialize(obj);
        
        // 处理普通对象
        return Object_Serialize(obj);
    }
    public T Deserialize<T>(string json)
    {
        return (T)Deserialize(json, typeof(T));
    }
    public object Deserialize(string json, Type type)
    {
        if (string.IsNullOrEmpty(json) || json == "null")
            return GetDefaultValue(type);
        json = json.Trim();
        
        try
        {
            // 检查是否有自定义反序列化器
            if (_deserializers.TryGetValue(type, out Func<string, object> deserializer))
                return deserializer(json);
            
            // 处理数组
            if (type.IsArray)
                return DeserializeArray(json, type);
            
            // 处理列表
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
                return DeserializeList(json, type);
            
            // 处理字典
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Dictionary<,>))
                return DeserializeDictionary(json, type);
            
            // 处理普通对象
            return DeserializeObject(json, type);
        }
        catch (Exception ex)
        {
            Debug.LogError($"反序列化失败: {ex.Message}\nJSON: {json}\nType: {type}");
            return GetDefaultValue(type);
        }
        
        // 简化的反序列化 - 实际项目需要完整实现 JSON 解析
        // Debug.LogWarning("自定义反序列化需要完整实现 JSON 解析器");
        // return JsonUtility.FromJson(json, type);
    }


    
    private readonly Dictionary<Type, Func<object, string>> _serializers = new();
    private readonly Dictionary<Type, Func<string, object>> _deserializers = new();
    public void InitMethod()
    {
        _serializers.Add(typeof(string), String_Serialize);
        _serializers.Add(typeof(bool), Bool_Serialize);
        _serializers.Add(typeof(int), Int_Serialize);
        _serializers.Add(typeof(float), Float_Serialize);
        _serializers.Add(typeof(double), DoubleSerialize);
        _serializers.Add(typeof(Vector2), Vector2_Serialize);
        _serializers.Add(typeof(Vector3), Vector3_Serialize);
        _serializers.Add(typeof(Vector4), Vector4_Serialize);
        _serializers.Add(typeof(Quaternion), Quaternion_Serialize);
        _serializers.Add(typeof(Color), Color_Serialize);
        _serializers.Add(typeof(Color32), Color32_Serialize);

        //_deserializers.Add();
    }


    #region 序列化
    private string Object_Serialize(object obj)
    {
        var type = obj.GetType();
        FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance);
        StringBuilder sb = new StringBuilder();
        sb.Append("{");

        for (int i = 0; i < fields.Length; i++)
        {
            FieldInfo field = fields[i];


            // 检查 JsonIgnore 特性
            if (Attribute.IsDefined(field, typeof(JsonIgnoreAttribute)))
                continue;

            // 获取字段名（支持 JsonField 特性）
            string fieldName = field.Name;
            var jsonFieldAttr = field.GetCustomAttribute<JsonFieldAttribute>();
            if (jsonFieldAttr != null && !string.IsNullOrEmpty(jsonFieldAttr.Name))
                fieldName = jsonFieldAttr.Name;

            if (i != 0) sb.Append(",");
            object value = field.GetValue(obj);
            sb.Append($"\"{fieldName}\":{Serialize(value)}");
        }

        sb.Append("}");
        return sb.ToString();
    }
    private string String_Serialize(object str)
    {
        string value = (string)str;
        if (string.IsNullOrEmpty(value)) return "";

        return value.Replace("\\", "\\\\")
            .Replace("\"", "\\\"")
            .Replace("\n", "\\n")
            .Replace("\r", "\\r")
            .Replace("\t", "\\t")
            .Replace("\b", "\\b")
            .Replace("\f", "\\f");
    }
    private string Bool_Serialize(object obj)
    {
        return (bool)obj ? "true" : "false";
    }
    private string Int_Serialize(object obj)
    {
        return obj.ToString();
    }
    private string Float_Serialize(object obj)
    {
        return ((float)obj).ToString("F6").Replace(",", ".");
    }
    private string DoubleSerialize(object obj)
    {
        return ((double)obj).ToString("F10").Replace(",", ".");
    }
    private string Vector2_Serialize(object obj)
    {
        var v = (Vector2)obj;
        return $"{{\"x\":{v.x},\"y\":{v.y}}}";
    }
    private string Vector3_Serialize(object obj)
    {
        var v = (Vector3)obj;
        return $"{{\"x\":{v.x},\"y\":{v.y},\"z\":{v.z}}}";
    }
    private string Vector4_Serialize(object obj)
    {
        var v = (Vector4)obj;
        return $"{{\"x\":{v.x},\"y\":{v.y},\"z\":{v.z},\"w\":{v.w}}}";
    }
    private string Quaternion_Serialize(object obj)
    {
        var q = (Quaternion)obj;
        return $"{{\"x\":{q.x},\"y\":{q.y},\"z\":{q.z},\"w\":{q.w}}}";
    }
    private string Color_Serialize(object obj)
    {
        var c = (Color)obj;
        return $"{{\"r\":{c.r},\"g\":{c.g},\"b\":{c.b},\"a\":{c.a}}}";
    }
    private string Color32_Serialize(object obj)
    {
        var c = (Color32)obj;
        return $"{{\"r\":{c.r},\"g\":{c.g},\"b\":{c.b},\"a\":{c.a}}}";
    }
    private string Array_Serialize(object array)
    {
        var arrayObj = array as Array;
        var sb = new StringBuilder();
        sb.Append("[");
        
        for (int i = 0; i < arrayObj.Length; i++)
        {
            if (i > 0) sb.Append(",");
            sb.Append(Serialize(arrayObj.GetValue(i)));
        }
        
        sb.Append("]");
        return sb.ToString();
    }
    private string List_Serialize(object list)
    {
        var count = (int)list.GetType().GetProperty("Count").GetValue(list);
        var sb = new StringBuilder();
        sb.Append("[");
        
        for (int i = 0; i < count; i++)
        {
            if (i > 0) sb.Append(",");
            var item = list.GetType().GetMethod("get_Item").Invoke(list, new object[] { i });
            sb.Append(Serialize(item));
        }
        
        sb.Append("]");
        return sb.ToString();
    }
    private string Dictionary_Serialize(object dict)
    {
        var keys = dict.GetType().GetProperty("Keys").GetValue(dict) as System.Collections.ICollection;
        var sb = new StringBuilder();
        sb.Append("{");
        
        bool first = true;
        foreach (var key in keys)
        {
            if (!first) sb.Append(",");
            first = false;
            
            var value = dict.GetType().GetMethod("get_Item").Invoke(dict, new object[] { key });
            sb.Append($"\"{key}\":{Serialize(value)}");
        }
        
        sb.Append("}");
        return sb.ToString();
    }
    #endregion



    #region 反序列化
    private object GetDefaultValue(Type type)
    {
        return type.IsValueType ? Activator.CreateInstance(type) : null;
    }
    private string DeserializeString(string json)
    {
        if (json.StartsWith("\"") && json.EndsWith("\""))
            return UnescapeString(json.Substring(1, json.Length - 2));
        return json;
    }
    private bool DeserializeBool(string json)
    {
        return json.ToLower() == "true";
    }
    private int DeserializeInt(string json)
    {
        if (int.TryParse(json, out int result))
            return result;
        return 0;
    }
    private float DeserializeFloat(string json)
    {
        if (float.TryParse(json, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out float result))
            return result;
        return 0f;
    }
    private double DeserializeDouble(string json)
    {
        if (double.TryParse(json, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out double result))
            return result;
        return 0.0;
    }
    private object DeserializeVector2(string json)
    {
        Dictionary<string, string> dict = ParseObject(json);
        return new Vector2(dict.ContainsKey("x") ? DeserializeFloat(dict["x"]) : 0, dict.ContainsKey("y") ? DeserializeFloat(dict["y"]) : 0);
    }
    private object DeserializeVector3(string json)
    {
        var dict = ParseObject(json);
        return new Vector3(
            dict.ContainsKey("x") ? DeserializeFloat(dict["x"]) : 0,
            dict.ContainsKey("y") ? DeserializeFloat(dict["y"]) : 0,
            dict.ContainsKey("z") ? DeserializeFloat(dict["z"]) : 0
        );
    }
    private object DeserializeVector4(string json)
    {
        var dict = ParseObject(json);
        return new Vector4(
            dict.ContainsKey("x") ? DeserializeFloat(dict["x"]) : 0,
            dict.ContainsKey("y") ? DeserializeFloat(dict["y"]) : 0,
            dict.ContainsKey("z") ? DeserializeFloat(dict["z"]) : 0,
            dict.ContainsKey("w") ? DeserializeFloat(dict["w"]) : 0
        );
    }
    private object DeserializeQuaternion(string json)
    {
        var dict = ParseObject(json);
        return new Quaternion(
            dict.ContainsKey("x") ? DeserializeFloat(dict["x"]) : 0,
            dict.ContainsKey("y") ? DeserializeFloat(dict["y"]) : 0,
            dict.ContainsKey("z") ? DeserializeFloat(dict["z"]) : 0,
            dict.ContainsKey("w") ? DeserializeFloat(dict["w"]) : 1
        );
    }
    private object DeserializeColor(string json)
    {
        var dict = ParseObject(json);
        return new Color(
            dict.ContainsKey("r") ? DeserializeFloat(dict["r"]) : 0,
            dict.ContainsKey("g") ? DeserializeFloat(dict["g"]) : 0,
            dict.ContainsKey("b") ? DeserializeFloat(dict["b"]) : 0,
            dict.ContainsKey("a") ? DeserializeFloat(dict["a"]) : 1
        );
    }
    private object DeserializeColor32(string json)
    {
        var dict = ParseObject(json);
        return new Color32(
            dict.ContainsKey("r") ? (byte)DeserializeInt(dict["r"]) : (byte)0,
            dict.ContainsKey("g") ? (byte)DeserializeInt(dict["g"]) : (byte)0,
            dict.ContainsKey("b") ? (byte)DeserializeInt(dict["b"]) : (byte)0,
            dict.ContainsKey("a") ? (byte)DeserializeInt(dict["a"]) : (byte)255
        );
    }
    private Dictionary<string, string> ParseObject(string json)
    {
        var result = new Dictionary<string, string>();
        if (json.Length <= 2) return result;

        json = json.Substring(1, json.Length - 2).Trim();
        int index = 0;
        
        while (index < json.Length)
        {
            // 解析键
            var key = ParseString(json, ref index);
            if (key == null) break;

            // 跳过冒号
            index = SkipWhitespace(json, index);
            if (index >= json.Length || json[index] != ':') break;
            index++;

            // 解析值
            index = SkipWhitespace(json, index);
            var value = ParseValue(json, ref index);
            result[key] = value;

            // 跳过逗号
            index = SkipWhitespace(json, index);
            if (index < json.Length && json[index] == ',')
                index++;
        }

        return result;
    }
    private object DeserializeArray(string json, Type arrayType)
    {
        if (!json.StartsWith("[") || !json.EndsWith("]"))
            return GetDefaultValue(arrayType);

        var elementType = arrayType.GetElementType();
        var elements = ParseArray(json);
        var array = Array.CreateInstance(elementType, elements.Count);

        for (int i = 0; i < elements.Count; i++)
        {
            array.SetValue(Deserialize(elements[i], elementType), i);
        }

        return array;
    }

    private object DeserializeList(string json, Type listType)
    {
        if (!json.StartsWith("[") || !json.EndsWith("]"))
            return GetDefaultValue(listType);

        var elementType = listType.GetGenericArguments()[0];
        var elements = ParseArray(json);
        var list = Activator.CreateInstance(listType) as IList;

        if (list == null)
            return GetDefaultValue(listType);

        foreach (var element in elements)
        {
            list.Add(Deserialize(element, elementType));
        }

        return list;
    }

    private object DeserializeDictionary(string json, Type dictType)
    {
        if (!json.StartsWith("{") || !json.EndsWith("}"))
            return GetDefaultValue(dictType);

        var keyType = dictType.GetGenericArguments()[0];
        var valueType = dictType.GetGenericArguments()[1];
        var dict = Activator.CreateInstance(dictType) as IDictionary;

        if (dict == null)
            return GetDefaultValue(dictType);

        var parsedDict = ParseObject(json);
        foreach (var kvp in parsedDict)
        {
            var key = Convert.ChangeType(DeserializeString(kvp.Key), keyType);
            var value = Deserialize(kvp.Value, valueType);
            dict.Add(key, value);
        }

        return dict;
    }

    private object DeserializeObject(string json, Type objectType)
    {
        if (!json.StartsWith("{") || !json.EndsWith("}"))
            return GetDefaultValue(objectType);

        var instance = Activator.CreateInstance(objectType);
        var fields = objectType.GetFields(BindingFlags.Public | BindingFlags.Instance);
        var parsedObject = ParseObject(json);

        foreach (var field in fields)
        {
            // 检查 JsonIgnore 特性
            if (Attribute.IsDefined(field, typeof(JsonIgnoreAttribute)))
                continue;

            // 获取字段名（支持 JsonField 特性）
            string fieldName = field.Name;
            var jsonFieldAttr = field.GetCustomAttribute<JsonFieldAttribute>();
            if (jsonFieldAttr != null && !string.IsNullOrEmpty(jsonFieldAttr.Name))
                fieldName = jsonFieldAttr.Name;

            if (parsedObject.ContainsKey(fieldName))
            {
                try
                {
                    var value = Deserialize(parsedObject[fieldName], field.FieldType);
                    field.SetValue(instance, value);
                }
                catch (Exception ex)
                {
                    Debug.LogWarning($"设置字段 {fieldName} 失败: {ex.Message}");
                }
            }
        }

        return instance;
    }
    #endregion
    
    
    
    #region 解析器
    private List<string> ParseArray(string json)
    {
        var result = new List<string>();
        if (json.Length <= 2) return result;

        json = json.Substring(1, json.Length - 2).Trim();
        int index = 0;
        
        while (index < json.Length)
        {
            index = SkipWhitespace(json, index);
            if (index >= json.Length) break;

            var value = ParseValue(json, ref index);
            result.Add(value);

            index = SkipWhitespace(json, index);
            if (index < json.Length && json[index] == ',')
                index++;
        }

        return result;
    }
    private string ParseValue(string json, ref int index)
    {
        index = SkipWhitespace(json, index);
        if (index >= json.Length) return "";

        char startChar = json[index];
        
        if (startChar == '"') // 字符串
        {
            return ParseString(json, ref index);
        }
        else if (startChar == '{') // 对象
        {
            return ParseBracketed(json, ref index, '{', '}');
        }
        else if (startChar == '[') // 数组
        {
            return ParseBracketed(json, ref index, '[', ']');
        }
        else // 基础类型（数字、布尔值、null）
        {
            return ParsePrimitive(json, ref index);
        }
    }
    private string ParseString(string json, ref int index)
    {
        if (json[index] != '"') return null;
        
        index++; // 跳过开头的 "
        var sb = new StringBuilder();
        
        while (index < json.Length && json[index] != '"')
        {
            if (json[index] == '\\')
            {
                index++;
                if (index < json.Length)
                {
                    switch (json[index])
                    {
                        case '"': sb.Append('"'); break;
                        case '\\': sb.Append('\\'); break;
                        case '/': sb.Append('/'); break;
                        case 'b': sb.Append('\b'); break;
                        case 'f': sb.Append('\f'); break;
                        case 'n': sb.Append('\n'); break;
                        case 'r': sb.Append('\r'); break;
                        case 't': sb.Append('\t'); break;
                        case 'u': // Unicode 转义序列
                            if (index + 4 < json.Length)
                            {
                                var hex = json.Substring(index + 1, 4);
                                sb.Append((char)Convert.ToInt32(hex, 16));
                                index += 4;
                            }
                            break;
                    }
                }
            }
            else
            {
                sb.Append(json[index]);
            }
            index++;
        }
        
        if (index < json.Length && json[index] == '"')
            index++; // 跳过结尾的 "
            
        return sb.ToString();
    }
    private string ParseBracketed(string json, ref int index, char open, char close)
    {
        int depth = 0;
        int start = index;
        
        do
        {
            if (json[index] == open) depth++;
            else if (json[index] == close) depth--;
            index++;
        }
        while (index < json.Length && depth > 0);
        
        return json.Substring(start, index - start);
    }
    private string ParsePrimitive(string json, ref int index)
    {
        int start = index;
        
        while (index < json.Length && !IsWhitespace(json[index]) && json[index] != ',' && json[index] != '}' && json[index] != ']')
        {
            index++;
        }
        
        return json.Substring(start, index - start).Trim();
    }
    private int SkipWhitespace(string json, int index)
    {
        while (index < json.Length && IsWhitespace(json[index]))
            index++;
        return index;
    }
    private bool IsWhitespace(char c)
    {
        return c == ' ' || c == '\t' || c == '\n' || c == '\r';
    }
    private string UnescapeString(string str)
    {
        return str.Replace("\\\"", "\"")
            .Replace("\\\\", "\\")
            .Replace("\\n", "\n")
            .Replace("\\r", "\r")
            .Replace("\\t", "\t")
            .Replace("\\b", "\b")
            .Replace("\\f", "\f");
    }
    #endregion
}


/// <summary>
/// 跳过的
/// </summary>
[AttributeUsage(AttributeTargets.Field)]
public class JsonIgnoreAttribute : Attribute { }

/// <summary>
/// 替换的
/// </summary>
[AttributeUsage(AttributeTargets.Field)]
public class JsonFieldAttribute : Attribute
{
    public string Name { get; }
    
    public JsonFieldAttribute(string name = null)
    {
        Name = name;
    }
}


public enum JsonType
{
    JsonUtlity,
    LitJson,
}

public class JsonOperation 
{
    private JsonType jsonType = JsonType.LitJson;
    public T Load<T>(string fileName) where T : class
    {
        //确定从哪个路径读取
        //首先先判断 默认数据文件夹中是否有我们想要的数据 如果有 就从中获取
        string path = $"{Application.streamingAssetsPath}/{fileName}.json";
        //先判断 是否存在这个文件
        //如果不存在默认文件 就从 读写文件夹中去寻找
        if (!File.Exists(path))
            path = $"{Application.persistentDataPath}/{fileName}.json";
        //如果读写文件夹中都还没有 那就返回一个默认对象
        if (!File.Exists(path))
            return null;

        //进行反序列化
        string jsonStr = File.ReadAllText(path);
        //数据对象
        T data = default(T);
        switch (jsonType)
        {
            case JsonType.JsonUtlity:
                data = JsonUtility.FromJson<T>(jsonStr);
                break;
            case JsonType.LitJson:
                data = JsonMapper.ToObject<T>(jsonStr);
                break;
        }

        //把对象返回出去
        return data;
    }

    public void Save(object data, string fileName)
    {
        //确定存储路径
        string path = Application.persistentDataPath + "/" + fileName + ".json";
        //序列化 得到Json字符串
        string jsonStr = "";
        switch (jsonType)
        {
            case JsonType.JsonUtlity: jsonStr = JsonUtility.ToJson(data); break;
            case JsonType.LitJson: jsonStr = JsonMapper.ToJson(data); break;
        }
        //把序列化的Json字符串 存储到指定路径的文件中
        File.WriteAllText(path, jsonStr);
    }
}