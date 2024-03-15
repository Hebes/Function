using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
[System.Serializable]
public class UKeyframe
{
	/// <summary>
	///  Describes the tangent when approaching this point from the previous point in the curve.
	/// 描述从曲线上的前一点接近该点时的切线。
	/// </summary>
	public float inTangent;
	
	/// <summary>
	/// The out tangent.
	/// 出切线
	/// </summary>
	public float outTangent;
 
	/// <summary>
	///  The time of the keyframe.
	/// 关键帧的时间。
	/// </summary>
	public float time;
 
	/// <summary>
	/// The value of the curve at keyframe.
	/// 曲线在关键帧处的值。
	/// </summary>
	public float value;
 
	public static UKeyframe GetUkeyframe (Keyframe kf)
	{
		UKeyframe ukf = new UKeyframe ();
		ukf.time = kf.time;
		ukf.value = kf.value;
		ukf.inTangent = kf.inTangent;
		ukf.outTangent = kf.outTangent;
		return ukf;
	}
 
}
 
public class UAnimationCurve : MonoBehaviour
{
 
	public UKeyframe[] keys;
 
	public void SetKeys (UKeyframe[] keys)
	{
		this.keys = keys;
	}
 
	public float Evaluate (float x)
	{
		if (this.keys == null || this.keys.Length == 0) {
			return 0;
		}
		return UAnimationCurve.Evaluate (this.keys, x);
	}
 
	/// <summary>
	///  分段三次Hermite样条曲线 P(t) = B1 + B2 * t + B3 * t2 + B4 * t3  
	///  t 后数字是指数
	///  已知每个节点的x,y 和节点的切线值（导数，微商值） ， 可根据相邻2点和微商值确定一条三次Hermite样条曲线
	///  注意，当节点数多于2个时，就是分段三次Hermite样条曲线，每一段的x,y的起点终点 与上一个节点的终点做个偏移（矫正），最后再加上偏移量即可
	/// </summary>
	public static float Evaluate (UKeyframe[] keys, float x)
	{
		var index = 0;
		// 找出当前节点
		for (int i = 0; i < keys.Length; i++) {
            if (i == 0 && x < keys[i].time)
            {
                return keys[0].value;
            }
            if (x <= keys [i].time) {
				index = i;
				if (i == 0) {
					index = 1;
				}
				break;
			}
		}
		if (index == 0) {
//			index = keys.Length - 1;
			return keys [keys.Length - 1].value;
		}
		// 前一个节点
		var startIndex = index - 1;
		// 后一个节点
		var endIndex = index;
		// 当前时间（当前曲线的时间点）
		var t = x - keys [startIndex].time;
		// 当前曲线偏移的时间点
		float off_t = keys [startIndex].time;
		// 当前曲线偏移的值
		float off_p = keys [startIndex].value;
		// 当前曲线起点
		var t0 = keys [startIndex].time - off_t;
		// 当前曲线终点
		var t1 = keys [endIndex].time - off_t;
 
		// 求参数时用到的是一些表达式
		var A = t1 - t0;
		var B = t1 * t1 - t0 * t0;
		var C = t1 * t1 * t1 - t0 * t0 * t0;
		// 起点值（矫正当前曲线的值）
		var p0 = keys [startIndex].value - off_p;
		// 终点值（矫正当前曲线的值）
		var p1 = keys [endIndex].value - off_p;
		// 起点切线值
		var p0_d = keys [startIndex].outTangent;
		// 终点切线值
		var p1_d = keys [endIndex].inTangent;
 
		// 求当前曲线参数
		var b4 = ((p1 - p0 - p0_d * A) / (B - 2 * A * t0 * t0) - (p1_d - p0_d) / (2 * A)) / ((C - 3 * A * t0 * t0) / (B - 2 * A * t0) - (3 * B / (2 * A)));
		var b3 = (p1_d - p0_d) / (2 * A) - 3 * B / (2 * A) * b4;
		var b2 = p0_d - (b3 * 2 * t0 + b4 * 3 * t0 * t0);
		var b1 = p0 - (b2 * t0 + b3 * t0 * t0 + b4 * t0 * t0 * t0);
		// 求当前曲线值
		var pt = b1 + b2 * t + b3 * t * t + b4 * t * t * t;
 
		return pt + off_p;
	}
}