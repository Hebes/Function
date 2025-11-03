using UnityEngine.Profiling;

namespace ExpansionUnity
{
    /// <summary>
    /// 性能断点使用
    /// </summary>
    public class ExpansionProfiler
    {
        /// <summary>
        /// 需要成对出现ProfilerEndSample
        /// </summary>
        /// <param name="str"></param>
        public static void ProfilerBeginSample(string str)
        {
            Profiler.BeginSample(str);
        }

        public static void ProfilerEndSample()
        {
            Profiler.EndSample();
        }
    }
}