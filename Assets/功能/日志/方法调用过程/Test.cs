using UnityEngine;

namespace 日志.方法调用过程
{
    public class Test : MonoBehaviour
    {
        private void Awake()
        {
            T1();
        }

        private void T1()
        {
            T2();
        }

        private void T2()
        {
            var showStack = new ShowStack("测试");
            Debug.Log(showStack.message);
        }
    }
}