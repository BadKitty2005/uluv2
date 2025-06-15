using System.Diagnostics;
using UnityEngine;

public class OpenBin
{
    public static void OpenRecycleBin()
    {
        Process.Start("explorer.exe", "shell:RecycleBinFolder");
    }
}
