using System.Diagnostics;
using System.IO;
using UnityEngine;

public class OpenFile
{
    public static void OpenFileByName(string fileName)
    {
        string searchDirectory = @"C:\Users\Public";
        string[] foundFiles = Directory.GetFiles(searchDirectory, "*", SearchOption.AllDirectories);

        foreach (string filePath in foundFiles)
        {
            if (Path.GetFileNameWithoutExtension(filePath).ToLower().Contains(fileName.ToLower()))
            {
                Process.Start(filePath);
                UnityEngine.Debug.Log("Файл открыт: " + filePath);
                return;
            }
        }

        UnityEngine.Debug.LogWarning("Файл не найден для открытия: " + fileName);
    }
}
