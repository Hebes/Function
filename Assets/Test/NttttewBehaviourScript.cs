using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyNamespace
{
    [Serializable]
    public class NttttewBehaviourScriptData
    {
        public List<T1> t1List ;

        public NttttewBehaviourScriptData()
        {
            t1List = new List<T1>();
        }
    }
    
    public class NttttewBehaviourScript : MonoBehaviour
    {
        

        private void Awake()
        {
            NttttewBehaviourScriptData nttttewBehaviourScriptData = new NttttewBehaviourScriptData();
            nttttewBehaviourScriptData. t1List.Add(new T2());
            nttttewBehaviourScriptData. t1List.Add(new T3());

            string count = JsonUtility.ToJson(nttttewBehaviourScriptData);
            Debug.Log(count);
        }
    }

    public abstract class T1
    {
        public int temp1;
    }

    public class T2 : T1
    {
        public int temp2;

        public T2()
        {
            temp1 = 1;
            temp2 = 2;
        }
    }

    public class T3 : T1
    {
        public int temp3;

        public T3()
        {
            temp1 = 1;
            temp3 = 3;
        }
    }
}