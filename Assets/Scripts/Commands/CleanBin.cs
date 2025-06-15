using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using UnityEngine;

public class CleanBin
{
    [DllImport("Shell32.dll", CharSet = CharSet.Unicode)]
    static extern uint SHEmptyRecycleBin(IntPtr hwnd, string pszRootPath, uint dwFlags);

    public static void ClearBin()
    {
        // 0x00000001 = no confirmation
        // 0x00000002 = no progress UI
        // 0x00000004 = no sound
        SHEmptyRecycleBin(System.IntPtr.Zero, null, 0x00000007);
    }
}
