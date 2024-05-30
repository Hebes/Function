using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnimationCurveView
{
    /// <summary>
    /// https://blog.csdn.net/fucun1984686003/article/details/81086630
    /// </summary>
    public class NewBehaviourScript : MonoBehaviour
    {
        /// <summary>
        /// unity 曲线
        /// </summary>
        public AnimationCurve cure = new AnimationCurve();

        void OnDrawGizmos()
        {
            if (cure.keys == null || cure.keys.Length == 0)
            {
                return;
            }

            UKeyframe[] keys = new UKeyframe[cure.keys.Length];
            for (int i = 0; i < keys.Length; i++)
            {
                keys[i] = UKeyframe.GetUkeyframe(cure.keys[i]);
            }

            for (int j = 0; j <= 100; j++)
            {
                float s = 1.5f;
                var pos = new Vector3(j * 0.01f, UAnimationCurve.Evaluate(keys, j * 0.01f), 0);
                var pos2 = new Vector3(j * 0.01f + s, cure.Evaluate(j * 0.01f) + s, 0);
                Gizmos.DrawCube(pos, Vector3.one * 0.03f);
                Gizmos.DrawCube(pos2, Vector3.one * 0.03f);
            }
        }
    }
}