using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;

public class OpenApp
{
    // ����������� ������� ����������
    private readonly Dictionary<string, string> appTranslations = new Dictionary<string, string>
    {
        { "�������", "notepad" },
        { "�������", "blender" },
        { "����", "winword" },
        { "������", "excel" },
        { "����", "chrome" },
        { "�������", "chrome" },
        { "���������", "explorer" }
    };

    // �������� ������� ��� �������
    private readonly string[] launchCommands = { "������", "�������", "������", "�������", "���������" };

    public void ProcessVoiceCommand(string voiceInput)
    {
        if (string.IsNullOrEmpty(voiceInput))
        {
            UnityEngine.Debug.LogError("������� ������ �����");
            return;
        }

        string input = voiceInput.Trim().ToLower();
        UnityEngine.Debug.Log($"������������ �������: {input}");

        // ��������� �������� �� ������� �������� ����� �������
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
            UnityEngine.Debug.Log("������� �� �������� �������� ���� ��� �������");
            return;
        }

        // ���� ���������� � ��������� ����������
        string appToLaunch = null;
        foreach (var pair in appTranslations)
        {
            if (input.Contains(pair.Key))
            {
                appToLaunch = pair.Value;
                UnityEngine.Debug.Log($"������� ����������: {pair.Key} -> {pair.Value}");
                break;
            }
        }

        if (appToLaunch == null)
        {
            UnityEngine.Debug.Log("�� ������� ����������� ���������� ��� �������");
            return;
        }

        // ��������� ����������
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
                UnityEngine.Debug.Log($"������� ��������: {appName} ({exePath})");
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.LogError($"������ ������� {appName}: {ex.Message}");
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
        // �������� ��������� ���������� PATH
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
                    UnityEngine.Debug.LogWarning($"������ ������ � {path}: {ex.Message}");
                }
            }
        }

        // �������� ����������� ����������
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
                UnityEngine.Debug.LogWarning($"������ ������ � {dir}: {ex.Message}");
            }
        }

        return null;
    }

    private void OpenDownloadSearch(string appName)
    {
        string url = $"https://www.google.com/search?q=�������+{Uri.EscapeDataString(appName)}+���+windows";
        Application.OpenURL(url);
        UnityEngine.Debug.Log($"�������� �������� ������: {url}");
    }
}
