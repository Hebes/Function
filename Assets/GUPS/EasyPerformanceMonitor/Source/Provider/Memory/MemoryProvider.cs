// Microsoft
using System;

namespace GUPS.EasyPerformanceMonitor.Provider
{
    /// <summary>
    /// Fetches the allocated memory.
    /// </summary>
    /// <remarks>
    /// The <see cref="MemoryProvider"/> class is responsible for fetching and providing information about the allocated memory used by the
    /// application. This component is supported on all platforms and uses the System.GC.GetTotalMemory method to calculate
    /// the total memory used by the application in bytes.
    /// <br>-> Recommended Desktop memory - Thresholds above: Warning 8GB Critical 12GB</br>
    /// <br>-> Recommended Mobile memory - Thresholds above: Warning 200MB Critical 500MB</br>
    /// <br>-> Recommended Console memory - Thresholds above: Warning 4GB Critical 6GB</br>
    /// </remarks>
    public class MemoryProvider : APerformanceProvider
    {
        /// <summary>
        /// The performance component name.
        /// </summary>
        public const String CName = "Memory";

        /// <summary>
        /// Returns the performance component name.
        /// </summary>
        public override string Name
        {
            get
            {
                return CName;
            }
        }

        /// <summary>
        /// Returns true if the performance component is supported on the current platform.
        /// The Memory component is supported on all platforms.
        /// </summary>
        public override bool IsSupported
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// The default unit is bytes.
        /// </summary>
        public override String Unit
        {
            get
            {
                return "B";
            }
        }

        /// <summary>
        /// Calculates the current memory usage and adds it to the graph.
        /// </summary>
        /// <returns></returns>
        protected override float GetNextValue()
        { 
            // Get the total memory used by the application in bytes.
            long var_CurrentMemory = System.GC.GetTotalMemory(false);

            // Add value.
            return var_CurrentMemory;
        }
    }
}
