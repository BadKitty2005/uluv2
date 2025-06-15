using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using UnityEngine;

public class FindFile
{
    private static readonly string[] stopWords = {
        "�����", "������ ����", "������ ����", "���", "����� ����", "����������", "�� �����", "� ���������", "�����"
    };

    public static void FindAndRevealFile(string userText)
    {
        // ��������� ������� ����� "����" (� ������ ��������)
        if (!userText.ToLower().Contains("����"))
        {
            UnityEngine.Debug.LogWarning("������� �� �������� ����� '����', ���������� ��������.");
            return;
        }

        string cleanedFileName = ExtractFileName(userText);
        if (string.IsNullOrEmpty(cleanedFileName))
        {
            UnityEngine.Debug.LogWarning("��� ����� �� ������� � ����� ������������.");
            return;
        }

        try
        {
            // ����� �� ���� ������ � ����������
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string[] allFiles = GetAllAccessibleFiles(documentsPath);

            //// ����� �� ���� ��������� ������
            //DriveInfo[] allDrives = DriveInfo.GetDrives();
            //var allFiles = new List<string>();

            //foreach (var drive in allDrives)
            //{
            //    if (drive.IsReady) // ���������, ����� �� ����
            //    {
            //        UnityEngine.Debug.Log($"������������ �����: {drive.Name}");
            //        string[] driveFiles = GetAllAccessibleFiles(drive.RootDirectory.FullName);
            //        allFiles.AddRange(driveFiles);
            //    }
            //    else
            //    {
            //        UnityEngine.Debug.LogWarning($"���� �� �����: {drive.Name}, ���������...");
            //    }
            //}

            string matchedFile = allFiles
                .FirstOrDefault(path => Path.GetFileNameWithoutExtension(path)
                    .ToLower().Contains(cleanedFileName.ToLower()));

            if (!string.IsNullOrEmpty(matchedFile))
            {
                // ��������� ��������� � �������� ����
                System.Diagnostics.Process.Start("explorer.exe", $"/select,\"{matchedFile}\"");
                UnityEngine.Debug.Log($"���� ������ � �������: {matchedFile}");
            }
            else
            {
                UnityEngine.Debug.LogWarning($"���� � ������ '{cleanedFileName}' �� ������.");
            }
        }
        catch (Exception e)
        {
            UnityEngine.Debug.LogError("����� ������ ��� ������ �����: " + e.Message);
        }
    }

    private static string[] GetAllAccessibleFiles(string rootPath)
    {
        var allFiles = new List<string>();

        try
        {
            // ����������� ������� �����
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
                    UnityEngine.Debug.LogWarning($"��� ������� � �����: {currentDir}, ���������...");
                    continue; // ���������� ����� � ����������
                }
                catch (Exception ex)
                {
                    UnityEngine.Debug.LogWarning($"������ ��� ������� � {currentDir}: {ex.Message}, ���������...");
                    continue;
                }
            }
        }
        catch (Exception ex)
        {
            UnityEngine.Debug.LogError($"������ ��� ������������: {ex.Message}");
        }

        return allFiles.ToArray();
    }

    private static string ExtractFileName(string input)
    {
        input = input.ToLower();

        // ������� ������� ����� "����" ���������� �� �������
        input = input.Replace("����", "").Trim();

        var words = input.Split(' ')
            .Where(word => !string.IsNullOrWhiteSpace(word))
            .Where(word => !stopWords.Contains(word))
            .ToArray();

        return string.Join(" ", words).Trim();
    }
}
