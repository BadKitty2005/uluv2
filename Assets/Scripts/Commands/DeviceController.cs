using System.Diagnostics;
using System;
using UnityEngine;

public class DeviceController
{
    //private string command; // ���������� ��� ������� ������������

    //public void SetCommand(string userCommand)
    //{
    //    command = userCommand?.ToLower().Trim();
    //    ExecuteCommand();
    //}

    //private void ExecuteCommand()
    //{
    //    if (string.IsNullOrEmpty(command))
    //    {
    //        UnityEngine.Debug.LogError("������� �� ������");
    //        return;
    //    }

    //    // ��������� �������� � ����������
    //    string action = command.Contains("������") ? "on" : command.Contains("�������") ? "off" : null;
    //    if (action == null)
    //    {
    //        UnityEngine.Debug.LogWarning($"����������� �������� � �������: {command}");
    //        return;
    //    }

    //    if (command.Contains("wi-fi") || command.Contains("������"))
    //    {
    //        ToggleWiFi(action);
    //    }
    //    else if (command.Contains("bluetooth") || command.Contains("������"))
    //    {
    //        ToggleBluetooth(action);
    //    }
    //    else if (command.Contains("����� �����"))
    //    {
    //        ToggleAirplaneMode(action);
    //    }
    //    else if (command.Contains("����� ����������������"))
    //    {
    //        TogglePowerSaving(action);
    //    }
    //    else
    //    {
    //        UnityEngine.Debug.LogWarning($"����������� ���������� ��� ����� � �������: {command}");
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
    //            Verb = "runas", // ������ ���� ��������������
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
    //        UnityEngine.Debug.LogError($"������ ���������� Wi-Fi: {ex.Message}");
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
    //            Verb = "runas", // ������ ���� ��������������
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
    //        UnityEngine.Debug.LogError($"������ ���������� Bluetooth: {ex.Message}");
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
    //            Verb = "runas", // ������ ���� ��������������
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
    //        UnityEngine.Debug.LogError($"������ ���������� ������� �����: {ex.Message}");
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
    //            Verb = "runas", // ������ ���� ��������������
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
    //        UnityEngine.Debug.LogError($"������ ���������� �����������������: {ex.Message}");
    //    }
    //}
}
