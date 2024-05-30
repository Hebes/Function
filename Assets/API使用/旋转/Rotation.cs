using System;
using UnityEngine;

namespace 旋转
{
    /// <summary>
    /// 旋转
    /// https://blog.csdn.net/qq_42489774/article/details/97016477
    /// https://blog.csdn.net/m1234567q/article/details/130851985
    /// </summary>
    public class Rotation : MonoBehaviour
    {
        private void Awake()
        {
            //第一种：Rotate(vector,中心点);有两个参数：
            // transform.Rotate(x,y,z)：以自身坐标系为参考，而不是世界坐标系，分别以x度y度z度绕X轴、Y轴、Z轴匀速旋转
            // transform.Rotate(vector3，Space.Self):以自身坐标系为参考
            // Transform.Rotate(vector3，Space.World):以世界坐标系为参考

            //第二种：rotation = rotation;看等号就知道是用来赋值的
            //transform.rotation = Quaternion.Euler(new Vector3(x, y, z));
            
            //第三种:transform.localEulerAngles = new Vector3(x, y, z);
            
            //第四种：transform.eulerAngles = new Vector3(x,y,z)
            
            //第五种：transform.localRotation=Quaternion.Euler(x,y,z)
            
            //第六种：transform.RotateAround(Vector3 point, Vector3 axis, float angle); 
        }
    }
}