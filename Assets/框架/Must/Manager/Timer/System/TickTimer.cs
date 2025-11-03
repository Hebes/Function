using System;
using System.Collections.Concurrent;
using System.Threading;

/*--------脚本描述-----------

描述:
    毫秒级精确的定时器

-----------------------*/

namespace Framework.Core
{
    public class TickTimer : Timer
    {
        /// <summary>
        /// 起始时间
        /// </summary>
        private readonly DateTime startDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);

        /// <summary>
        /// 任务字典
        /// </summary>
        private readonly ConcurrentDictionary<int, TickTask> taskDic;

        /// <summary>
        /// 设置处理
        /// </summary>
        private readonly bool setHandle;

        /// <summary>
        /// 任务取消回调包任务，线程安全的队列数据结构
        /// </summary>
        private readonly ConcurrentQueue<TickTaskPack> packQue;

        /// <summary>
        /// 锁
        /// </summary>
        private const string tidLock = "TickTimer_tidLock";

        /// <summary>
        /// 时间线程
        /// </summary>
        private readonly Thread timerThread;

        /// <summary>
        /// 毫秒级计时任务
        /// </summary>
        /// <param name="interval">每次循环停顿的间隔</param>
        /// <param name="setHandle">设置内部循环</param>
        public TickTimer(int interval = 0, bool setHandle = true)
        {
            taskDic = new ConcurrentDictionary<int, TickTask>();    //初始化任务字典
            this.setHandle = setHandle;                             //设置处理

            if (setHandle)
                packQue = new ConcurrentQueue<TickTaskPack>();

            if (interval != 0)
            {
                timerThread = new Thread(StartTick);
                timerThread.Start();
            }

            //多线程任务
            void StartTick()
            {
                try
                {
                    while (true)
                    {
                        UpdateTask();
                        Thread.Sleep(interval);
                    }
                }
                catch (ThreadAbortException e)
                {
                    WarnFunc?.Invoke($"勾选线程中止:{e}.");
                }
            }
        }


        //实现抽象
        public override int AddTask(uint delay, Action<int> taskCB, Action<int> cancelCB, int count = 1)
        {
            int tid = GenerateTid();                    //任务编号
            double startTime = GetUTCMilliseconds();    //开始时间
            double destTime = startTime + delay;        //目标时间
            TickTask task = new TickTask(tid, delay, count, destTime, taskCB, cancelCB, startTime); //一个任务
            if (taskDic.TryAdd(tid, task))
            {
                return tid;
            }
            else
            {
                WarnFunc?.Invoke($"任务:{tid} 已经退出.");
                return -1;
            }
        }
        public override bool DeleteTask(int tid)
        {
            if (taskDic.TryRemove(tid, out TickTask task))//尝试从字典中移除指定的键值对
            {
                if (setHandle && task.cancelCB != null)
                    packQue.Enqueue(new TickTaskPack(tid, task.cancelCB));
                else
                    task.cancelCB?.Invoke(tid);
                return true;
            }
            else
            {
                WarnFunc?.Invoke($"任务:{tid} 删除失败,任务不存在.");
                return false;
            }
        }
        public override void Reset()
        {
            if (!packQue.IsEmpty)
                WarnFunc?.Invoke("回调队列不为空.");
            taskDic.Clear();
            if (timerThread != null)
                timerThread.Abort();    //终止线程
        }
        protected override int GenerateTid()
        {
            lock (tidLock)//上锁
            {
                do
                {
                    tid++;
                    if (tid == int.MaxValue)
                        tid = 0;
                }
                while (taskDic.ContainsKey(tid));
                return tid;
            }
        }


        //其他
        public void UpdateTask()
        {
            double nowTime = GetUTCMilliseconds();
            foreach (var item in taskDic)
            {
                TickTask task = item.Value;
                if (nowTime < task.destTime)
                    continue;

                ++task.loopIndex;
                if (task.count > 0)
                {
                    --task.count;
                    if (task.count == 0)
                    {
                        FinsisTask(task.tid);
                    }
                    else
                    {
                        task.destTime = task.startTime + task.delay * (task.loopIndex + 1);
                        CallTaskCB(task.tid, task.taskCB);
                    }
                }
                else
                {
                    task.destTime = task.startTime + task.delay * (task.loopIndex + 1);
                    CallTaskCB(task.tid, task.taskCB);
                }
            }
        }
        public void HandleTask()
        {
            while (packQue != null && packQue.Count > 0)
            {
                if (packQue.TryDequeue(out TickTaskPack pack))
                {
                    pack.cb.Invoke(pack.tid);
                }
                else
                {
                    ErrorFunc?.Invoke("packQue Dequeue Data Error.");
                }
            }
        }
        private double GetUTCMilliseconds()
        {
            TimeSpan ts = DateTime.UtcNow - startDateTime;
            return ts.TotalMilliseconds;
        }
        void FinsisTask(int tid)
        {
            //线程安全字典，遍历过程中删除无影响。
            if (taskDic.TryRemove(tid, out TickTask task))
            {
                CallTaskCB(tid, task.taskCB);
                task.taskCB = null;
            }
            else
            {
                WarnFunc?.Invoke($"删除 tid:{tid} Dic中的任务失败.");
            }
        }
        void CallTaskCB(int tid, Action<int> taskCB)
        {
            if (setHandle)
                packQue.Enqueue(new TickTaskPack(tid, taskCB));//用于将元素添加到队列的末尾
            else
                taskCB.Invoke(tid);
        }
    }

    /// <summary>
    /// 打勾任务包
    /// </summary>
    public class TickTaskPack
    {
        /// <summary>
        /// 任务编号
        /// </summary>
        public int tid;

        /// <summary>
        /// 取消任务回调
        /// </summary>
        public Action<int> cb;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tid">任务编号</param>
        /// <param name="cb">取消任务回调</param>
        public TickTaskPack(int tid, Action<int> cb)
        {
            this.tid = tid;
            this.cb = cb;
        }
    }


    /// <summary>
    /// 毫秒级的任务
    /// </summary>
    public class TickTask
    {
        public int tid;
        public uint delay;
        public int count;
        public double destTime;
        public Action<int> taskCB;
        public Action<int> cancelCB;

        public double startTime;
        public ulong loopIndex;

        /// <summary>
        /// 变量赋值
        /// </summary>
        /// <param name="tid">任务编号</param>
        /// <param name="delay">定时任务时间</param>
        /// <param name="count">任务重复计数</param>
        /// <param name="destTime">目标时间</param>
        /// <param name="taskCB">定时任务回调</param>
        /// <param name="cancelCB">取消任务回调</param>
        /// <param name="startTime">开始的时间</param>
        public TickTask(int tid, uint delay, int count, double destTime, Action<int> taskCB, Action<int> cancelCB, double startTime)
        {
            this.tid = tid;
            this.delay = delay;
            this.count = count;
            this.destTime = destTime;
            this.taskCB = taskCB;
            this.cancelCB = cancelCB;
            this.startTime = startTime;

            loopIndex = 0;
        }
    }
}
