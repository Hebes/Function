using System.Text;
using UnityEngine;

namespace String优化
{
    public class Test: MonoBehaviour
    {
        void Start()
        {
            //用法1
            Debug.Log(StringEx.Concat("Afff", "Bfff"));
            Debug.Log(StringEx.Concat("C", ""));
            //用法2
            StringBuilder stringBuilder = StringEx.GetShareStringBuilder();
            for (int i = 0; i < 100; i++)
            {
                stringBuilder.Append(i.ToString());
            }
            Debug.Log(stringBuilder.ToString());
        }
    }
}