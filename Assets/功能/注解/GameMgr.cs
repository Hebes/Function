using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

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

// 注解 + 装饰器;
// 注解：給一个类型做一些注释对象，到时候我们可以读取得到;
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
        Type t = typeof(GameMgr); // 类型实例

        // 调用系统API全域类扫描你所有得类;
        Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
        foreach (Assembly assembly in assemblies) {
            Type[] types = assembly.GetTypes(); // 扫描你所有得当前程序集类型
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
