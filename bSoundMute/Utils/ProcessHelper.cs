using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace BSoundMute.Utils
{
    /// <summary>
    /// Helper class for process related operations
    /// </summary>
    internal static class ProcessHelper
    {
        // Maximum size of the cache
        private static readonly int s_maxCacheSize = 500;

        // Cache for parent process IDs to improve performance
        private static Dictionary<int, int> s_parentProcessCache = [];

        // Cache for root process IDs to improve performance
        private static Dictionary<int, int> s_rootProcessCache = [];

        // Cache expiration time
        private static TimeSpan s_cacheExpiration = TimeSpan.FromSeconds(30);

        // Cache last update time
        private static Dictionary<int, DateTime> s_cacheUpdateTime = [];

        /// <summary>
        /// Gets the parent process ID for a given process ID
        /// </summary>
        /// <param name="processId">The process ID to get the parent for</param>
        /// <returns>Parent process ID, or 0 if not found</returns>
        public static int GetParentProcessId(int processId)
        {
            if (s_parentProcessCache.Count > s_maxCacheSize)
            {
                ClearCache();
            }

            // Check cache first
            if (s_parentProcessCache.TryGetValue(processId, out int parentId))
            {
                // Check if cache is still valid
                if (s_cacheUpdateTime.TryGetValue(processId, out var updateTime) &&
                    DateTime.Now - updateTime < s_cacheExpiration)
                {
                    return parentId;
                }
            }

            var result = 0;

            // Use Windows API to get parent process ID
            var processHandle = Win32.OpenProcess(Win32.PROCESS_QUERY_INFORMATION, false, processId);
            if (processHandle != IntPtr.Zero)
            {
                try
                {
                    var pbi = new Win32.PROCESS_BASIC_INFORMATION();
                    var status = Win32.NtQueryInformationProcess(
                        processHandle,
                        0, // ProcessBasicInformation
                        ref pbi,
                        Marshal.SizeOf(pbi),
                        out var returnLength);

                    if (status == 0) // STATUS_SUCCESS
                    {
                        result = pbi.InheritedFromUniqueProcessId.ToInt32();
                    }
                }
                finally
                {
                    Win32.CloseHandle(processHandle);
                }
            }

            // Update cache
            s_parentProcessCache[processId] = result;
            s_cacheUpdateTime[processId] = DateTime.Now;

            return result;
        }

        /// <summary>
        /// Get the root process ID for a given process
        /// The root process is either the first non-system process in the parent chain
        /// or the process itself if it has no parent or its parent is a system process
        /// </summary>
        /// <param name="processId">The process ID to get the root for</param>
        /// <returns>The root process ID</returns>
        public static int GetRootProcessId(int processId)
        {
            if (s_rootProcessCache.Count > s_maxCacheSize)
            {
                ClearCache();
            }

            // Check cache first
            if (s_rootProcessCache.TryGetValue(processId, out var rootId))
            {
                // Check if cache is still valid
                if (s_cacheUpdateTime.TryGetValue(processId, out var updateTime) &&
                    DateTime.Now - updateTime < s_cacheExpiration)
                {
                    return rootId;
                }
            }

            var currentPid = processId;
            var parentPid = GetParentProcessId(currentPid);

            // Traverse the process tree until we find the root
            // The root is either when we have no more parents or the parent is a system process
            while (parentPid > 0 && !IsSystemOrExplorerProcess(parentPid))
            {
                currentPid = parentPid;
                parentPid = GetParentProcessId(currentPid);

                // Avoid infinite loops if there's a circular reference
                if (parentPid == processId)
                    break;
            }

            // Update cache
            s_rootProcessCache[processId] = currentPid;
            s_cacheUpdateTime[processId] = DateTime.Now;

            return currentPid;
        }

        /// <summary>
        /// Clears the process cache to free memory and ensure fresh data
        /// </summary>
        public static void ClearCache()
        {
            s_parentProcessCache.Clear();
            s_rootProcessCache.Clear();
            s_cacheUpdateTime.Clear();
        }

        /// <summary>
        /// Determines if a process is a system process or the explorer process
        /// </summary>
        /// <param name="processId">The process ID to check</param>
        /// <returns>True if the process is a system process or explorer</returns>
        private static bool IsSystemOrExplorerProcess(int processId)
        {
            try
            {
                // System processes typically have IDs 0 and 4
                if (processId <= 4)
                    return true;

                var process = Process.GetProcessById(processId);
                var processName = process.ProcessName.ToLower();

                // Check if it's explorer or a known system process
                return processName == "explorer" ||
                       processName == "system" ||
                       processName == "smss" ||
                       processName == "csrss" ||
                       processName == "winlogon" ||
                       processName == "services" ||
                       processName == "lsass";
            }
            catch
            {
                // Process might have exited or we don't have permission to access it
                return false;
            }
        }
    }
}
