using System;
using UnityEngine;
using UnityEngine.Profiling;
using XFramework.Collections;

namespace MyNamespace
{
    public class TestGC : MonoBehaviour
    {
        private List<DialogData> DialogDataDataList;
        
        private DialogData DialogData1;
        private DialogData1 DialogData2;
        private void Awake()
        {
            //A组
            DialogData1 = new DialogData();
            DialogData1.dialogDataData = new DialogDataData();
            
            
            //B组
            DialogData2 = new DialogData1();
            DialogDataDataList = new List<DialogData>();
            DialogDataDataList.Add(new DialogData());
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                Profiler.BeginSample("AGCShow");
                var temp = DialogData1.GetNextDialog();
                Profiler.EndSample();
            }
            
            if (Input.GetKeyDown(KeyCode.B))
            {
                Profiler.BeginSample("BGCShow");
                var temp = DialogData2.GetNextDialog();
                var temp1 = DialogDataDataList[temp];
                Profiler.EndSample();
            }
        }
    }

    public class DialogData
    {
        public DialogDataData dialogDataData;
        
        public DialogDataData GetNextDialog()
        {
            return dialogDataData;
        }
    }
    
    public class DialogData1
    {
        public int dialogDataData;
        
        public int GetNextDialog()
        {
            return dialogDataData;
        }
    }

    public class DialogDataData
    {
        public string value = "你好";
    }
    
}