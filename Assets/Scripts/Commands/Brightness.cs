using System.Diagnostics;
using UnityEngine;

public class Brightness
{
    public void AdjustBrightness(string userText)
    {
        userText = userText.ToLower();

        int current = GetCurrentBrightness();
        int newValue = current;

        if (userText.Contains("минимум"))
            newValue = 0;
        else if (userText.Contains("максимум"))
            newValue = 100;
        else if (userText.Contains("убав") || userText.Contains("уменьш"))
            newValue = Mathf.Max(0, current - 10);
        else if (userText.Contains("добав") || userText.Contains("увелич"))
            newValue = Mathf.Min(100, current + 10);
        else
            UnityEngine.Debug.Log("‘раза не распознана дл€ управлени€ €ркостью");

        SetBrightness(newValue);
        UnityEngine.Debug.Log($"яркость установлена на {newValue}");
    }

    private int GetCurrentBrightness()
    {
        try
        {
            var psi = new ProcessStartInfo("powershell", "(Get-CimInstance -Namespace root/wmi -ClassName WmiMonitorBrightness).CurrentBrightness")
            {
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            var process = Process.Start(psi);
            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            if (int.TryParse(output.Trim(), out int brightness))
                return brightness;
        }
        catch
        {
            UnityEngine.Debug.LogWarning("Ќе удалось получить текущую €ркость");
        }

        return 50; // значение по умолчанию
    }

    private void SetBrightness(int brightness)
    {
        string command = $"(Get-WmiObject -Namespace root/wmi -Class WmiMonitorBrightnessMethods).WmiSetBrightness(1,{brightness})";
        var psi = new ProcessStartInfo("powershell", command)
        {
            UseShellExecute = false,
            CreateNoWindow = true
        };
        Process.Start(psi);
    }
}
