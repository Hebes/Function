using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace Framework.Core
{
    [Serializable]
    public class CallStack
    {
        [Header("基本说明")]
        public string baseInfo;

        [Header("调用时间")]
        public string callTime;

        public TimeSpan CallTime { get; }

        [Header("详细")]
        public List<string> call;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseInfo">基本说明</param>
        public CallStack(string baseInfo)
        {
            this.baseInfo = baseInfo;
            call = new List<string>();
            var stackTrace = new StackTrace();  
            for (var i = 2; i < stackTrace.FrameCount; i++)
            {
                //堆栈帧
                var stackFrame = stackTrace.GetFrame(i);
                call.Add($"类:{stackFrame.GetMethod().DeclaringType.Name}, 方法:{stackFrame.GetMethod().Name}");
            }


            CallTime = DateTime.Now.TimeOfDay;
            callTime = CallTime.ToString();
        }

        public override string ToString() => $"Details={baseInfo},Time={callTime}";

        //用法介绍
        //public void Add()
        //{
        //    //下面的代码才是真的
        //    List<CallStack> callStacks = new List<CallStack>();
        //    callStacks.Add(new CallStack("测试"));
        //}
    }
}