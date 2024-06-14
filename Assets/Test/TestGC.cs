using System;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.UI;
using System.Collections.Generic;

namespace MyNamespace
{
    public class TestGC : MonoBehaviour
    {
        private List<DialogData> DialogDataDataList;

        private DialogData DialogData1;
        private DialogData1 DialogData2;
        public Vector3 originalVector = new Vector3(0.2f, 0.1f, 0f);

        private Dictionary<int, int> valueDic = new Dictionary<int, int>();
        private List<(int, int)> valueList = new List<(int, int)>();
        
        int uid = 999999;
        private void Awake()
        {
            for (int i = 0; i < 1000000; i++)
            {
                valueDic.Add(i, i);
                valueList.Add((i, i));
            }

            
            

            

            //Vector3 normalizedVector = originalVector.normalized;
            //Debug.Log("原始向量：" + originalVector);
            //Debug.Log("归一化向量：" + normalizedVector);
            return;
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
                var startValue = DateTime.Now.TimeOfDay.TotalMilliseconds;
                int temp1 = valueDic[uid];
                var runTimeValue = DateTime.Now.TimeOfDay.TotalMilliseconds - startValue;

                int temp2 = 0;
                var startValue1 = DateTime.Now.TimeOfDay.TotalMilliseconds;
                foreach (var value in valueList)
                {
                    if (value.Item1==uid)
                        temp2 = value.Item1;
                }
                var runTimeValue1 = DateTime.Now.TimeOfDay.TotalMilliseconds - startValue1;
                
                Debug.LogError($"字典查找 {runTimeValue} \n 列表查找 {runTimeValue1}");
            }

            if (Input.GetKeyDown(KeyCode.B))
            {
                
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