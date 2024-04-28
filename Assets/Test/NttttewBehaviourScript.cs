using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.UI;

namespace MyNamespace
{
    public class NttttewBehaviourScript : MonoBehaviour
    {

        public Text txt;
        public StringBuilder sb;
        public Collider collider;

        private void Awake()
        {
            //sb= new StringBuilder();
        }

        private void Update()
        {
            
            Debug.Log(collider.bounds.max);
            // Profiler.BeginSample($"StringRun");
            // if (Input.GetKeyDown(KeyCode.A))
            // {
            //     for (var i = 0; i < 100; i++)
            //         sb.Append("你好");
            //     txt.text = sb.ToString();
            // }
            //
            // if (Input.GetKeyDown(KeyCode.B))
            // {
            //     var sb1 = "你好";
            //     for (var i = 0; i < 100; i++)
            //         sb1 += sb1;
            //     txt.text = sb1;
            // }
            // Profiler.EndSample();
        }
    }
}