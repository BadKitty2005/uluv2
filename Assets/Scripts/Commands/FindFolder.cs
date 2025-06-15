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
    //                // ������� ��������� � �������� �����
    //                Process.Start("explorer.exe", "/select,\"" + dirs[0] + "\"");
    //                return;
    //            }
    //        }
    //        catch { }
    //    }
    //}
    private static readonly string[] stopWords = {
        "�����", "�����", "������ �����", "������ �����", "���", "����� �����", "����������", "�� �����", "� ���������", "�����"
    };

    public static void FindAndRevealFolder(string userText)
    {
        // ��������� ������� ����� "�����" (� ������ ��������)
        if (!userText.ToLower().Contains("�����"))
        {
            UnityEngine.Debug.LogWarning("������� �� �������� ����� '�����', ���������� ��������.");
            return;
        }

        string cleanedFolderName = ExtractFolderName(userText);
        if (string.IsNullOrEmpty(cleanedFolderName))
        {
            UnityEngine.Debug.LogWarning("��� ����� �� ������� � ����� ������������.");
            return;
        }

        try
        {
            // ����� �� ���� ��������� ������
            var allFolders = DriveInfo.GetDrives()
                .Where(drive => drive.IsReady)
                .SelectMany(drive =>
                {
                    UnityEngine.Debug.Log($"������������ �����: {drive.Name}");
                    return GetAllAccessibleFolders(drive.RootDirectory.FullName);
                })
                .ToList();

            string matchedFolder = allFolders
                .FirstOrDefault(path => Path.GetFileName(path)
                    .ToLower().Contains(cleanedFolderName.ToLower()));

            if (!string.IsNullOrEmpty(matchedFolder))
            {
                // ��������� ��������� � ���������� �����
                System.Diagnostics.Process.Start("explorer.exe", $"\"{matchedFolder}\"");
                UnityEngine.Debug.Log($"����� ������� � �������: {matchedFolder}");
            }
            else
            {
                UnityEngine.Debug.LogWarning($"����� � ������ '{cleanedFolderName}' �� �������.");
            }
        }
        catch (Exception e)
        {
            UnityEngine.Debug.LogError("����� ������ ��� ������ �����: " + e.Message);
        }
    }

    private static string[] GetAllAccessibleFolders(string rootPath)
    {
        var allFolders = new List<string>();

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
                    string[] subDirs = Directory.GetDirectories(currentDir);
                    allFolders.AddRange(subDirs.Select(d => d)); // ��������� ������� �����

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

        return allFolders.ToArray();
    }

    private static string ExtractFolderName(string input)
    {
        input = input.ToLower();

        // ������� ������� ����� "�����" ���������� �� �������
        input = input.Replace("�����", "").Trim();

        var words = input.Split(' ')
            .Where(word => !string.IsNullOrWhiteSpace(word))
            .Where(word => !stopWords.Contains(word))
            .ToArray();

        return string.Join(" ", words).Trim();
    }
}
