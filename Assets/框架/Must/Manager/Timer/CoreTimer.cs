using System;
using System.Collections;
using System.Collections.Generic;

namespace Framework.Core
{
    public class CoreTimer : ICore
    {
        public void Init()
        {
        }

        public IEnumerator AsyncEnter()
        {
            yield return null;
        }

        public IEnumerator Exit()
        {
            yield break;
        }

        public  List<TimerData> TaskList = new List<TimerData>();
        public  Queue<IPool> RecycleDataQueue = new Queue<IPool>(); //简单对象池

        public void OnUpdate()
        {
            for (var i = TaskList.Count - 1; i >= 0; i--)
                TaskList[i].OnUpdate();
        }

        public void AddTask(float totalTime, Action<TimerData> action, bool autoRecycle = true)
        {
            var timerData = RecycleDataQueue.Count > 0 ? RecycleDataQueue.Dequeue() : new TimerData(this);
            //todo 
            //timerData.Get((totalTime, action, autoRecycle));
            TaskList.Add(timerData as TimerData);
        }

        public void UnAddTack(Action<TimerData> actionValue)
        {
            for (var i = TaskList.Count - 1; i >= 0; i--)
            {
                var timerData = TaskList[i];
                if (timerData.Action == actionValue)
                    timerData.Push();
            }
        }
    }

    /// <summary>
    /// 计时器数据
    /// </summary>
    public class TimerData : IPool
    {
        public CoreTimer CoreTimer;
        public float TotalTime;//一共时间
        public float CurTimer;//当前时间
        public Action<TimerData> Action;//执行的方法
        public bool AutoRecycle;//是否自动回收
        
        public float DesMilliseconds { get; }

        public TimerData(CoreTimer coreTimer)
        {
            CoreTimer = coreTimer;
        }

        public void Get(object valueTuple)
        {
            var convertedTuple = ((float, Action<TimerData>,bool))valueTuple;
            TotalTime = convertedTuple.Item1;
            Action = convertedTuple.Item2;
            AutoRecycle = convertedTuple.Item3;
        }

        public void OnUpdate()
        {
            CurTimer += UnityEngine.Time.deltaTime;
            if (CurTimer <= TotalTime) return;
            Action(this);
            if (!AutoRecycle)return;
            if (CurTimer <= TotalTime) return;//Action里面改时间了就不会回收
            Push();
        }

        public void Get()
        {
            
        }

        /// <summary>
        /// 需要自己在Action调用回收
        /// </summary>
        public void Push()
        {
            TotalTime = 0;
            CurTimer = 0;
            Action = null;
            CoreTimer.TaskList.Remove(this);
            CoreTimer.RecycleDataQueue.Enqueue(this);
        }
    }
}