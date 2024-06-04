using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;


/// <summary>
/// 临时变量使用
/// </summary>
[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class TempAttribute : Attribute
{
    

    public TempAttribute()
    {
        
    }
}

class EntityObject : Attribute {
    public EntityObject(string name) { 
    }
}

class NewEntityObject : Attribute
{
    public object instace;
    public NewEntityObject(Type t)
    {
        Debug.Log("NewEntityObject ^^^^");
        instace = Activator.CreateInstance(t);
    }
}

// ע�� + װ����;
// ע�⣺�oһ��������һЩע�Ͷ��󣬵�ʱ�����ǿ��Զ�ȡ�õ�;
[EntityObject("GameEntiyObject")]
public class GameEntiyObject { 
}

[EntityObject("MyEntiyObject")]
[NewEntityObject(typeof(MyEntiyObject))]
public class MyEntiyObject
{
    
    public MyEntiyObject() {
        Debug.Log("##########MyEntiyObject!!!!!");
    }
}


public class GameMgr : MonoBehaviour
{
    void Start()
    {
        Type t = typeof(GameMgr); // ����ʵ��

        // ����ϵͳAPIȫ����ɨ�������е���;
        Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
        foreach (Assembly assembly in assemblies) {
            Type[] types = assembly.GetTypes(); // ɨ�������еõ�ǰ��������
            for (int i = 0; i < types.Length; i++) {
                NewEntityObject obj = types[i].GetCustomAttribute<NewEntityObject>();
                if (obj != null) {
                    Debug.Log(types[i].Name);
                }

            }
        }
        // end
    }
}
