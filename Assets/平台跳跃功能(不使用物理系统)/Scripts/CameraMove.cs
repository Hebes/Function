using UnityEngine;

namespace 平台跳跃功能
{
    public class CameraMove : MonoBehaviour
    {
        public Transform target; //观察的目标
        public float moveSpeed = 8; //摄像移动速度
        public float offsetY = 5; //摄像机偏移的Y位置
        private Vector3 targetPos;//目标位置

        void Update()
        {
            targetPos = target.position;
            //对目标位置进行Y偏移
            targetPos.y += offsetY;
            //摄像机z不需要更新 主要更新x和y
            targetPos.z = this.transform.position.z;
            //让摄像机不停的向目标位置靠拢
            this.transform.position = Vector3.Lerp(this.transform.position, targetPos, moveSpeed * Time.deltaTime);
        }
    }
}