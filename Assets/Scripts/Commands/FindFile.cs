using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using UnityEngine;

public class FindFile
{
    private static readonly string[] stopWords = {
        "найди", "открой файл", "покажи файл", "мне", "нужен файл", "пожалуйста", "по имени", "с названием", "найти"
    };

    public static void FindAndRevealFile(string userText)
    {
        // Проверяем наличие слова "файл" (с учётом регистра)
        if (!userText.ToLower().Contains("файл"))
        {
            UnityEngine.Debug.LogWarning("Команда не содержит слово 'файл', выполнение прервано.");
            return;
        }

        string cleanedFileName = ExtractFileName(userText);
        if (string.IsNullOrEmpty(cleanedFileName))
        {
            UnityEngine.Debug.LogWarning("Имя файла не найдено в фразе пользователя.");
            return;
        }

        try
        {
            // Поиск по всем папкам в Документах
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string[] allFiles = GetAllAccessibleFiles(documentsPath);

            //// Поиск по всем доступным дискам
            //DriveInfo[] allDrives = DriveInfo.GetDrives();
            //var allFiles = new List<string>();

            //foreach (var drive in allDrives)
            //{
            //    if (drive.IsReady) // Проверяем, готов ли диск
            //    {
            //        UnityEngine.Debug.Log($"Сканирование диска: {drive.Name}");
            //        string[] driveFiles = GetAllAccessibleFiles(drive.RootDirectory.FullName);
            //        allFiles.AddRange(driveFiles);
            //    }
            //    else
            //    {
            //        UnityEngine.Debug.LogWarning($"Диск не готов: {drive.Name}, пропускаю...");
            //    }
            //}

            string matchedFile = allFiles
                .FirstOrDefault(path => Path.GetFileNameWithoutExtension(path)
                    .ToLower().Contains(cleanedFileName.ToLower()));

            if (!string.IsNullOrEmpty(matchedFile))
            {
                // Открываем проводник и выделяем файл
                System.Diagnostics.Process.Start("explorer.exe", $"/select,\"{matchedFile}\"");
                UnityEngine.Debug.Log($"Файл найден и выделен: {matchedFile}");
            }
            else
            {
                UnityEngine.Debug.LogWarning($"Файл с именем '{cleanedFileName}' не найден.");
            }
        }
        catch (Exception e)
        {
            UnityEngine.Debug.LogError("Общая ошибка при поиске файла: " + e.Message);
        }
    }

    private static string[] GetAllAccessibleFiles(string rootPath)
    {
        var allFiles = new List<string>();

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
                    string[] files = Directory.GetFiles(currentDir);
                    allFiles.AddRange(files);

                    string[] subDirs = Directory.GetDirectories(currentDir);
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

        return allFiles.ToArray();
    }

    private static string ExtractFileName(string input)
    {
        input = input.ToLower();

        // Сначала убираем слово "файл" независимо от позиции
        input = input.Replace("файл", "").Trim();

        var words = input.Split(' ')
            .Where(word => !string.IsNullOrWhiteSpace(word))
            .Where(word => !stopWords.Contains(word))
            .ToArray();

        return string.Join(" ", words).Trim();
    }
}
