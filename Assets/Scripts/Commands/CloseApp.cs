using System.Diagnostics;
using System;
using UnityEngine;
using System.Linq;

public class CloseApp
{
    public static void CloseLastOpenedApp()
    {
        var allProcesses = Process.GetProcesses()
            .Where(p =>
                !string.IsNullOrEmpty(p.MainWindowTitle) && // � ���� ���� ����
                !p.ProcessName.ToLower().Contains("system") &&
                !p.ProcessName.ToLower().Contains("idle") &&
                p.SessionId == Process.GetCurrentProcess().SessionId) // ������� ������ ������������
            .ToList();

        if (allProcesses.Count == 0)
        {
            UnityEngine.Debug.LogWarning("��� �������� ���������������� ���������� ��� ��������.");
            return;
        }

        Process newestProcess = allProcesses
            .OrderByDescending(p =>
            {
                try
                {
                    return p.StartTime;
                }
                catch
                {
                    return DateTime.MinValue;
                }
            })
            .FirstOrDefault();

        if (newestProcess != null)
        {
            try
            {
                UnityEngine.Debug.Log("�������� ��������� �������� ����������: " + newestProcess.ProcessName);
                newestProcess.Kill();
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogError("�� ������� ������� ����������: " + e.Message);
            }
        }
    }
}
