using System.IO;
using UnityEngine;

public class DeleteFile
{
    public static void DeleteFileByName(string fileName)
    {
        string searchDirectory = @"C:\Users\Public";
        string[] foundFiles = Directory.GetFiles(searchDirectory, "*", SearchOption.AllDirectories);

        foreach (string filePath in foundFiles)
        {
            if (Path.GetFileNameWithoutExtension(filePath).ToLower().Contains(fileName.ToLower()))
            {
                File.Delete(filePath);
                Debug.Log("Файл удалён: " + filePath);
                return;
            }
        }

        Debug.LogWarning("Файл не найден для удаления: " + fileName);
    }
}
