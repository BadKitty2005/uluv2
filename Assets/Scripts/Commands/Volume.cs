using System;
using System.Diagnostics;
using System.Net;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;

public class Volume
{// Импорт необходимых функций Windows API
    [DllImport("user32.dll")]
    private static extern IntPtr SendMessageW(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

    private const int APPCOMMAND_VOLUME_UP = 0xA0000;
    private const int APPCOMMAND_VOLUME_DOWN = 0x90000;
    private const int APPCOMMAND_VOLUME_MUTE = 0x80000;
    private const int WM_APPCOMMAND = 0x319;

    public void AdjustVolume(string userText)
    {
        userText = userText.ToLower().Trim();

        try
        {
            if (userText.Contains("минимум") || userText.Contains("выключи звук"))
            {
                SetVolume(0);
            }
            else if (userText.Contains("максимум") || userText.Contains("полная громкость"))
            {
                SetVolume(100);
            }
            else if (userText.Contains("убав") || userText.Contains("уменьш") || userText.Contains("тише"))
            {
                ChangeVolume(-10);
            }
            else if (userText.Contains("добав") || userText.Contains("увелич") || userText.Contains("громче"))
            {
                ChangeVolume(10);
            }
            else if (userText.Contains("безмолв") || userText.Contains("отключи звук"))
            {
                ToggleMute();
            }
            else
            {
                UnityEngine.Debug.Log("Команда не распознана");
            }
        }
        catch (Exception ex)
        {
            UnityEngine.Debug.LogError($"Ошибка изменения громкости: {ex.Message}");
        }
    }

    private void SetVolume(int percent)
    {
        percent = Mathf.Clamp(percent, 0, 100);

        if (percent == 0)
        {
            // Сначала установим минимальную громкость
            for (int i = 0; i < 50; i++) SendVolumeCommand(APPCOMMAND_VOLUME_DOWN);
            // Затем выключим звук
            SendVolumeCommand(APPCOMMAND_VOLUME_MUTE);
        }
        else
        {
            // Сначала установим минимальную громкость
            for (int i = 0; i < 50; i++) SendVolumeCommand(APPCOMMAND_VOLUME_DOWN);
            // Затем установим нужный уровень
            for (int i = 0; i < percent / 2; i++) SendVolumeCommand(APPCOMMAND_VOLUME_UP);
        }

        UnityEngine.Debug.Log($"Громкость установлена на {percent}%");
    }

    private void ChangeVolume(int delta)
    {
        int steps = Mathf.Abs(delta) / 2;
        int command = delta > 0 ? APPCOMMAND_VOLUME_UP : APPCOMMAND_VOLUME_DOWN;

        for (int i = 0; i < steps; i++)
        {
            SendVolumeCommand(command);
        }

        UnityEngine.Debug.Log($"Громкость изменена на {delta}%");
    }

    private void ToggleMute()
    {
        SendVolumeCommand(APPCOMMAND_VOLUME_MUTE);
        UnityEngine.Debug.Log("Режим беззвука переключен");
    }

    private void SendVolumeCommand(int command)
    {
        try
        {
            IntPtr handle = Process.GetCurrentProcess().MainWindowHandle;
            SendMessageW(handle, WM_APPCOMMAND, handle, (IntPtr)command);
        }
        catch (Exception ex)
        {
            UnityEngine.Debug.LogError($"Ошибка отправки команды: {ex.Message}");
        }
    }
}
