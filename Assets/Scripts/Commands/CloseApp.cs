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
                !string.IsNullOrEmpty(p.MainWindowTitle) && // У него есть окно
                !p.ProcessName.ToLower().Contains("system") &&
                !p.ProcessName.ToLower().Contains("idle") &&
                p.SessionId == Process.GetCurrentProcess().SessionId) // Текущая сессия пользователя
            .ToList();

        if (allProcesses.Count == 0)
        {
            UnityEngine.Debug.LogWarning("Нет активных пользовательских приложений для закрытия.");
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
                UnityEngine.Debug.Log("Закрываю последнее открытое приложение: " + newestProcess.ProcessName);
                newestProcess.Kill();
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogError("Не удалось закрыть приложение: " + e.Message);
            }
        }
    }
}
