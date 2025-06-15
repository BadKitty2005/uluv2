using System.Collections.Generic;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using UnityEngine;

public class FindFolder
{
    //public static void Execute(string folderName)
    //{
    //    string[] drives = Directory.GetLogicalDrives();
    //    foreach (string drive in drives)
    //    {
    //        try
    //        {
    //            var dirs = Directory.GetDirectories(drive, folderName, SearchOption.AllDirectories);
    //            if (dirs.Length > 0)
    //            {
    //                // Открыть проводник и выделить папку
    //                Process.Start("explorer.exe", "/select,\"" + dirs[0] + "\"");
    //                return;
    //            }
    //        }
    //        catch { }
    //    }
    //}
    private static readonly string[] stopWords = {
        "найди", "папку", "открой папку", "покажи папку", "мне", "нужна папка", "пожалуйста", "по имени", "с названием", "найти"
    };

    public static void FindAndRevealFolder(string userText)
    {
        // Проверяем наличие слова "папка" (с учётом регистра)
        if (!userText.ToLower().Contains("папку"))
        {
            UnityEngine.Debug.LogWarning("Команда не содержит слово 'папка', выполнение прервано.");
            return;
        }

        string cleanedFolderName = ExtractFolderName(userText);
        if (string.IsNullOrEmpty(cleanedFolderName))
        {
            UnityEngine.Debug.LogWarning("Имя папки не найдено в фразе пользователя.");
            return;
        }

        try
        {
            // Поиск по всем доступным дискам
            var allFolders = DriveInfo.GetDrives()
                .Where(drive => drive.IsReady)
                .SelectMany(drive =>
                {
                    UnityEngine.Debug.Log($"Сканирование диска: {drive.Name}");
                    return GetAllAccessibleFolders(drive.RootDirectory.FullName);
                })
                .ToList();

            string matchedFolder = allFolders
                .FirstOrDefault(path => Path.GetFileName(path)
                    .ToLower().Contains(cleanedFolderName.ToLower()));

            if (!string.IsNullOrEmpty(matchedFolder))
            {
                // Открываем проводник и показываем папку
                System.Diagnostics.Process.Start("explorer.exe", $"\"{matchedFolder}\"");
                UnityEngine.Debug.Log($"Папка найдена и открыта: {matchedFolder}");
            }
            else
            {
                UnityEngine.Debug.LogWarning($"Папка с именем '{cleanedFolderName}' не найдена.");
            }
        }
        catch (Exception e)
        {
            UnityEngine.Debug.LogError("Общая ошибка при поиске папки: " + e.Message);
        }
    }

    private static string[] GetAllAccessibleFolders(string rootPath)
    {
        var allFolders = new List<string>();

        try
        {
            // Рекурсивный перебор папок
            var directories = new Queue<string>();
            directories.Enqueue(rootPath);

            while (directories.Count > 0)
            {
                string currentDir = directories.Dequeue();

                try
                {
                    string[] subDirs = Directory.GetDirectories(currentDir);
                    allFolders.AddRange(subDirs.Select(d => d)); // Добавляем текущую папку

                    foreach (string subDir in subDirs)
                    {
                        directories.Enqueue(subDir);
                    }
                }
                catch (UnauthorizedAccessException)
                {
                    UnityEngine.Debug.LogWarning($"Нет доступа к папке: {currentDir}, пропускаю...");
                    continue; // Пропускаем папку и продолжаем
                }
                catch (Exception ex)
                {
                    UnityEngine.Debug.LogWarning($"Ошибка при доступе к {currentDir}: {ex.Message}, пропускаю...");
                    continue;
                }
            }
        }
        catch (Exception ex)
        {
            UnityEngine.Debug.LogError($"Ошибка при сканировании: {ex.Message}");
        }

        return allFolders.ToArray();
    }

    private static string ExtractFolderName(string input)
    {
        input = input.ToLower();

        // Сначала убираем слово "папка" независимо от позиции
        input = input.Replace("папка", "").Trim();

        var words = input.Split(' ')
            .Where(word => !string.IsNullOrWhiteSpace(word))
            .Where(word => !stopWords.Contains(word))
            .ToArray();

        return string.Join(" ", words).Trim();
    }
}
