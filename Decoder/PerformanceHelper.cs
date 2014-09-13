using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime;

namespace Observer.Decode
{
    /// <summary>
    /// Provides methods used for tuning performance.
    /// </summary>
    internal sealed class PerformanceHelper
    {
        [DllImport("kernel32.dll")]
        private static extern IntPtr GetCurrentThread();
        [DllImport("kernel32.dll")]
        private static extern IntPtr SetThreadAffinityMask(IntPtr hThread, IntPtr dwThreadAffinityMask);

        /// <summary>
        /// Provides methods used for tuning performance.
        /// </summary>
        public PerformanceHelper()
        {
        }

        private static void PegToProcessor(int processorNumber)
        {
            // Peg to processor #0, if processorNumber is 0, for example.
            if (Environment.ProcessorCount > 1)
            {
                SetThreadAffinityMask(GetCurrentThread(), new IntPtr(1 << processorNumber));
            }
        }

        private static void SetProcessPriority(ProcessPriorityClass priority)
        {
            Process proc = Process.GetCurrentProcess();
            proc.PriorityClass = priority;
        }

        /// <summary>
        /// Sets process priority, sets GC latency mode, and pegs the decoder thread to a given processor.
        /// </summary>
        public static void ApplyPerformanceSettings()
        {
            PegToProcessor(0);
            SetProcessPriority(ProcessPriorityClass.RealTime);
            GCSettings.LatencyMode = GCLatencyMode.SustainedLowLatency;
        }
    }
}
