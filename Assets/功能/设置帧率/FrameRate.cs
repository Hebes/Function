using UnityEngine;

namespace 功能.设置帧率
{
    public class FrameRate : UnityEngine.MonoBehaviour
    {
        private void Awake()
        {
            Application.targetFrameRate = 60;
        }
    }
}