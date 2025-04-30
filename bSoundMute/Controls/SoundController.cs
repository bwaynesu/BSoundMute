using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace BSoundMute.Controls
{
    internal enum EDataFlow
    {
        eRender,
        eCapture,
        eAll,
        EDataFlow_enum_count
    }

    internal enum ERole
    {
        eConsole,
        eMultimedia,
        eCommunications,
        ERole_enum_count
    }

    [Guid("87CE5498-68D6-44E5-9215-6DA47EF883D8"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface ISimpleAudioVolume
    {
        [PreserveSig]
        int SetMasterVolume(float fLevel, ref Guid EventContext);

        [PreserveSig]
        int GetMasterVolume(out float pfLevel);

        [PreserveSig]
        int SetMute(bool bMute, ref Guid EventContext);

        [PreserveSig]
        int GetMute(out bool pbMute);
    }

    [Guid("A95664D2-9614-4F35-A746-DE8DB63617E6"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IMMDeviceEnumerator
    {
        int NotImpl1();

        [PreserveSig]
        int GetDefaultAudioEndpoint(EDataFlow dataFlow, ERole role, out IMMDevice ppDevice);

        // the rest is not implemented
    }

    [Guid("D666063F-1587-4E43-81F1-B948E807363F"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IMMDevice
    {
        [PreserveSig]
        int Activate(ref Guid iid, int dwClsCtx, IntPtr pActivationParams, [MarshalAs(UnmanagedType.IUnknown)] out object ppInterface);

        // the rest is not implemented
    }

    [ComImport]
    [Guid("BCDE0395-E52F-467C-8E3D-C4579291692E")]
    internal class MMDeviceEnumerator
    { }

    [Guid("77AA99A0-1BD6-484F-8BC7-2C654C9A9B6F"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IAudioSessionManager2
    {
        int NotImpl1();

        int NotImpl2();

        [PreserveSig]
        int GetSessionEnumerator(out IAudioSessionEnumerator SessionEnum);

        // the rest is not implemented
    }

    [Guid("E2F5BB11-0570-40CA-ACDD-3AA01277DEE8"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IAudioSessionEnumerator
    {
        [PreserveSig]
        int GetCount(out int SessionCount);

        [PreserveSig]
        int GetSession(int SessionCount, out IAudioSessionControl2 Session);
    }

    [Guid("bfb7ff88-7239-4fc9-8fa2-07c950be9c6d"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IAudioSessionControl2
    {
        // IAudioSessionControl
        [PreserveSig]
        int NotImpl0();

        [PreserveSig]
        int GetDisplayName([MarshalAs(UnmanagedType.LPWStr)] out string pRetVal);

        [PreserveSig]
        int SetDisplayName([MarshalAs(UnmanagedType.LPWStr)] string Value, [MarshalAs(UnmanagedType.LPStruct)] Guid EventContext);

        [PreserveSig]
        int GetIconPath([MarshalAs(UnmanagedType.LPWStr)] out string pRetVal);

        [PreserveSig]
        int SetIconPath([MarshalAs(UnmanagedType.LPWStr)] string Value, [MarshalAs(UnmanagedType.LPStruct)] Guid EventContext);

        [PreserveSig]
        int GetGroupingParam(out Guid pRetVal);

        [PreserveSig]
        int SetGroupingParam([MarshalAs(UnmanagedType.LPStruct)] Guid Override, [MarshalAs(UnmanagedType.LPStruct)] Guid EventContext);

        [PreserveSig]
        int NotImpl1();

        [PreserveSig]
        int NotImpl2();

        // IAudioSessionControl2
        [PreserveSig]
        int GetSessionIdentifier([MarshalAs(UnmanagedType.LPWStr)] out string pRetVal);

        [PreserveSig]
        int GetSessionInstanceIdentifier([MarshalAs(UnmanagedType.LPWStr)] out string pRetVal);

        [PreserveSig]
        int GetProcessId(out int pRetVal);

        [PreserveSig]
        int IsSystemSoundsSession();

        [PreserveSig]
        int SetDuckingPreference(bool optOut);
    }

    public class SoundController
    {
        public static float? GetApplicationVolume(int pid)
        {
            if (pid == 0)
                return null;

            ISimpleAudioVolume volume = GetVolumeObject(pid);
            if (volume == null)
                return null;

            try
            {
                float level;
                volume.GetMasterVolume(out level);
                return level * 100;
            }
            finally
            {
                Marshal.ReleaseComObject(volume);
            }
        }

        public static void SetApplicationVolume(int pid, float level)
        {
            if (pid == 0)
                return;

            ISimpleAudioVolume volume = GetVolumeObject(pid);
            if (volume == null)
                return;

            try
            {
                Guid guid = Guid.Empty;
                volume.SetMasterVolume(level / 100, ref guid);
            }
            finally
            {
                Marshal.ReleaseComObject(volume);
            }
        }

        public static bool? GetApplicationMute(int pid)
        {
            if (pid == 0)
                return null;

            ISimpleAudioVolume volume = GetVolumeObject(pid);
            if (volume == null)
                return null;

            try
            {
                bool mute = false;
                volume.GetMute(out mute);
                return mute;
            }
            finally
            {
                Marshal.ReleaseComObject(volume);
            }
        }

        public static void SetApplicationMute(int pid, bool mute)
        {
            if (pid == 0)
                return;

            ISimpleAudioVolume volume = GetVolumeObject(pid);
            if (volume == null)
                return;

            try
            {
                Guid guid = Guid.Empty;
                volume.SetMute(mute, ref guid);
            }
            finally
            {
                Marshal.ReleaseComObject(volume);
            }
        }

        private static ISimpleAudioVolume GetVolumeObject(int pid)
        {
            // Skip invalid process IDs
            if (pid == 0)
                return null;

            IMMDeviceEnumerator deviceEnumerator = null;
            IMMDevice speakers = null;
            IAudioSessionManager2 mgr = null;
            IAudioSessionEnumerator sessionEnumerator = null;
            ISimpleAudioVolume volumeControl = null;

            try
            {
                // Try to get the root process ID for better matching with audio sessions
                var rootPid = Utils.ProcessHelper.GetRootProcessId(pid);

                // get the speakers (1st render + multimedia) device
                deviceEnumerator = (IMMDeviceEnumerator)(new MMDeviceEnumerator());
                if (deviceEnumerator == null)
                    return null;

                deviceEnumerator.GetDefaultAudioEndpoint(EDataFlow.eRender, ERole.eMultimedia, out speakers);
                if (speakers == null)
                    return null;

                // activate the session manager. we need the enumerator
                var IID_IAudioSessionManager2 = typeof(IAudioSessionManager2).GUID;
                speakers.Activate(ref IID_IAudioSessionManager2, 0, IntPtr.Zero, out var o);
                mgr = (IAudioSessionManager2)o;
                if (o == null || mgr == null)
                    return null;

                // enumerate sessions for on this device
                mgr.GetSessionEnumerator(out sessionEnumerator);
                if (sessionEnumerator == null)
                    return null;

                sessionEnumerator.GetCount(out var count);

                // First pass: try to match exactly with process ID or root process ID
                for (var i = 0; i < count; ++i)
                {
                    IAudioSessionControl2 ctl = null;

                    try
                    {
                        sessionEnumerator.GetSession(i, out ctl);

                        var cpid = int.MaxValue;
                        var cpRootId = int.MaxValue;

                        if (ctl != null)
                        {
                            ctl.GetProcessId(out cpid);
                            cpRootId = Utils.ProcessHelper.GetRootProcessId(cpid);
                        }

                        // Check for exact match with original PID or root PID
                        if (cpid == pid || 
                            (rootPid != 0 && cpid == rootPid) ||
                            (cpRootId != 0 && cpRootId == rootPid))
                        {
                            volumeControl = ctl as ISimpleAudioVolume;
                            return volumeControl; // Don't release ctl here as it's returned as volumeControl
                        }
                    }
                    finally
                    {
                        // Only release if this is not the control we're returning
                        if (ctl != null && ctl != volumeControl)
                            Marshal.ReleaseComObject(ctl);
                    }
                }

                return null;
            }
            catch (Exception e)
            {
#if DEBUG
                MessageBox.Show(e.Message);
#endif
                return null;
            }
            finally
            {
                // Release all COM objects except volumeControl which is returned to caller
                if (sessionEnumerator != null)
                    Marshal.ReleaseComObject(sessionEnumerator);
                if (mgr != null)
                    Marshal.ReleaseComObject(mgr);
                if (speakers != null)
                    Marshal.ReleaseComObject(speakers);
                if (deviceEnumerator != null)
                    Marshal.ReleaseComObject(deviceEnumerator);
            }
        }
    }
}