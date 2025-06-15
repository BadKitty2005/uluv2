using System.Diagnostics;
using System;
using UnityEngine;

public class DeviceController
{
    //private string command; // Переменная для команды пользователя

    //public void SetCommand(string userCommand)
    //{
    //    command = userCommand?.ToLower().Trim();
    //    ExecuteCommand();
    //}

    //private void ExecuteCommand()
    //{
    //    if (string.IsNullOrEmpty(command))
    //    {
    //        UnityEngine.Debug.LogError("Команда не задана");
    //        return;
    //    }

    //    // Проверяем действие и устройство
    //    string action = command.Contains("включи") ? "on" : command.Contains("выключи") ? "off" : null;
    //    if (action == null)
    //    {
    //        UnityEngine.Debug.LogWarning($"Неизвестное действие в команде: {command}");
    //        return;
    //    }

    //    if (command.Contains("wi-fi") || command.Contains("вайфай"))
    //    {
    //        ToggleWiFi(action);
    //    }
    //    else if (command.Contains("bluetooth") || command.Contains("блютуз"))
    //    {
    //        ToggleBluetooth(action);
    //    }
    //    else if (command.Contains("режим полёта"))
    //    {
    //        ToggleAirplaneMode(action);
    //    }
    //    else if (command.Contains("режим энергосбережения"))
    //    {
    //        TogglePowerSaving(action);
    //    }
    //    else
    //    {
    //        UnityEngine.Debug.LogWarning($"Неизвестное устройство или режим в команде: {command}");
    //    }
    //}

    //private void ToggleWiFi(string action)
    //{
    //    try
    //    {
    //        ProcessStartInfo psi = new ProcessStartInfo
    //        {
    //            FileName = "cmd.exe",
    //            Arguments = $"/c netsh interface set interface \"Wi-Fi\" admin={action}",
    //            Verb = "runas", // Запрос прав администратора
    //            RedirectStandardOutput = true,
    //            UseShellExecute = false,
    //            CreateNoWindow = true
    //        };
    //        using (Process process = Process.Start(psi))
    //        {
    //            process.WaitForExit();
    //            string output = process.StandardOutput.ReadToEnd();
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        UnityEngine.Debug.LogError($"Ошибка управления Wi-Fi: {ex.Message}");
    //    }
    //}

    //private void ToggleBluetooth(string action)
    //{
    //    try
    //    {
    //        ProcessStartInfo psi = new ProcessStartInfo
    //        {
    //            FileName = "powershell.exe",
    //            Arguments = $"-Command \"Get-PnpDevice -Class Bluetooth | Where-Object {{ $_.Status -eq 'OK' }} | ForEach-Object {{ & 'pnputil.exe' /{action}-device $_.DeviceID }}\"",
    //            Verb = "runas", // Запрос прав администратора
    //            RedirectStandardOutput = true,
    //            UseShellExecute = false,
    //            CreateNoWindow = true   
    //        };
    //        using (Process process = Process.Start(psi))
    //        {
    //            process.WaitForExit();
    //            string output = process.StandardOutput.ReadToEnd();
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        UnityEngine.Debug.LogError($"Ошибка управления Bluetooth: {ex.Message}");
    //    }
    //}

    //private void ToggleAirplaneMode(string action)
    //{
    //    try
    //    {
    //        string key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\PolicyManager\default\WiFi\AllowWiFi";
    //        string value = action == "on" ? "1" : "0";
    //        ProcessStartInfo psi = new ProcessStartInfo
    //        {
    //            FileName = "reg",
    //            Arguments = $"add {key} /v Value /t REG_DWORD /d {value} /f",
    //            Verb = "runas", // Запрос прав администратора
    //            RedirectStandardOutput = true,
    //            UseShellExecute = false,
    //            CreateNoWindow = true
    //        };
    //        using (Process process = Process.Start(psi))
    //        {
    //            process.WaitForExit();
    //            string output = process.StandardOutput.ReadToEnd();
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        UnityEngine.Debug.LogError($"Ошибка управления режимом полёта: {ex.Message}");
    //    }
    //}

    //private void TogglePowerSaving(string action)
    //{
    //    try
    //    {
    //        string plan = action == "on" ? "powercfg /setactive 8c5e7fda-e8bf-4a96-9a85-a6e23a8c635c" : "powercfg /setactive 381b4222-f694-41f0-9685-ff5bb260df2e";
    //        ProcessStartInfo psi = new ProcessStartInfo
    //        {
    //            FileName = "powercfg",
    //            Arguments = plan,
    //            Verb = "runas", // Запрос прав администратора
    //            RedirectStandardOutput = true,
    //            UseShellExecute = false,
    //            CreateNoWindow = true
    //        };
    //        using (Process process = Process.Start(psi))
    //        {
    //            process.WaitForExit();
    //            string output = process.StandardOutput.ReadToEnd();
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        UnityEngine.Debug.LogError($"Ошибка управления энергосбережением: {ex.Message}");
    //    }
    //}
}
