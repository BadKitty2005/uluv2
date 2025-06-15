using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;

public class OpenApp
{
    // Расширенный словарь приложений
    private readonly Dictionary<string, string> appTranslations = new Dictionary<string, string>
    {
        { "блокнот", "notepad" },
        { "блендер", "blender" },
        { "ворд", "winword" },
        { "эксель", "excel" },
        { "хром", "chrome" },
        { "браузер", "chrome" },
        { "проводник", "explorer" }
    };

    // Ключевые команды для запуска
    private readonly string[] launchCommands = { "открой", "запусти", "включи", "открыть", "запустить" };

    public void ProcessVoiceCommand(string voiceInput)
    {
        if (string.IsNullOrEmpty(voiceInput))
        {
            UnityEngine.Debug.LogError("Входная строка пуста");
            return;
        }

        string input = voiceInput.Trim().ToLower();
        UnityEngine.Debug.Log($"Обрабатываем команду: {input}");

        // Проверяем содержит ли команда ключевое слово запуска
        bool shouldLaunch = false;
        foreach (string command in launchCommands)
        {
            if (input.Contains(command))
            {
                shouldLaunch = true;
                break;
            }
        }

        if (!shouldLaunch)
        {
            UnityEngine.Debug.Log("Команда не содержит ключевых слов для запуска");
            return;
        }

        // Ищем совпадение с названием приложения
        string appToLaunch = null;
        foreach (var pair in appTranslations)
        {
            if (input.Contains(pair.Key))
            {
                appToLaunch = pair.Value;
                UnityEngine.Debug.Log($"Найдено совпадение: {pair.Key} -> {pair.Value}");
                break;
            }
        }

        if (appToLaunch == null)
        {
            UnityEngine.Debug.Log("Не найдено подходящего приложения для запуска");
            return;
        }

        // Запускаем приложение
        LaunchApplication(appToLaunch);
    }

    private void LaunchApplication(string appName)
    {
        string exePath = FindAppExecutable(appName);
        if (!string.IsNullOrEmpty(exePath))
        {
            try
            {
                Process.Start(exePath);
                UnityEngine.Debug.Log($"Успешно запущено: {appName} ({exePath})");
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.LogError($"Ошибка запуска {appName}: {ex.Message}");
                OpenDownloadSearch(appName);
            }
        }
        else
        {
            OpenDownloadSearch(appName);
        }
    }

    private string FindAppExecutable(string appName)
    {
        // Проверка системных переменных PATH
        string pathEnv = Environment.GetEnvironmentVariable("PATH");
        if (!string.IsNullOrEmpty(pathEnv))
        {
            string[] paths = pathEnv.Split(';');
            foreach (string path in paths)
            {
                try
                {
                    if (Directory.Exists(path))
                    {
                        string[] exes = Directory.GetFiles(path, "*.exe", SearchOption.AllDirectories);
                        foreach (string exe in exes)
                        {
                            string fileName = Path.GetFileNameWithoutExtension(exe).ToLower();
                            if (fileName.Contains(appName))
                            {
                                return exe;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    UnityEngine.Debug.LogWarning($"Ошибка поиска в {path}: {ex.Message}");
                }
            }
        }

        // Проверка стандартных директорий
        string[] commonDirs = {
            @"C:\Program Files",
            @"C:\Program Files (x86)",
            Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
            Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86),
            Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
            Environment.GetFolderPath(Environment.SpecialFolder.StartMenu)
        };

        foreach (string dir in commonDirs)
        {
            try
            {
                if (Directory.Exists(dir))
                {
                    string[] exes = Directory.GetFiles(dir, "*.exe", SearchOption.AllDirectories);
                    foreach (string exe in exes)
                    {
                        string fileName = Path.GetFileNameWithoutExtension(exe).ToLower();
                        if (fileName.Contains(appName))
                        {
                            return exe;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.LogWarning($"Ошибка поиска в {dir}: {ex.Message}");
            }
        }

        return null;
    }

    private void OpenDownloadSearch(string appName)
    {
        string url = $"https://www.google.com/search?q=скачать+{Uri.EscapeDataString(appName)}+для+windows";
        Application.OpenURL(url);
        UnityEngine.Debug.Log($"Открываю страницу поиска: {url}");
    }
}
