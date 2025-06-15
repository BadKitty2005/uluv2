using System;
using System.Diagnostics;
using System.Net;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;

public class Volume
{// ������ ����������� ������� Windows API
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
            if (userText.Contains("�������") || userText.Contains("������� ����"))
            {
                SetVolume(0);
            }
            else if (userText.Contains("��������") || userText.Contains("������ ���������"))
            {
                SetVolume(100);
            }
            else if (userText.Contains("����") || userText.Contains("������") || userText.Contains("����"))
            {
                ChangeVolume(-10);
            }
            else if (userText.Contains("�����") || userText.Contains("������") || userText.Contains("������"))
            {
                ChangeVolume(10);
            }
            else if (userText.Contains("�������") || userText.Contains("������� ����"))
            {
                ToggleMute();
            }
            else
            {
                UnityEngine.Debug.Log("������� �� ����������");
            }
        }
        catch (Exception ex)
        {
            UnityEngine.Debug.LogError($"������ ��������� ���������: {ex.Message}");
        }
    }

    private void SetVolume(int percent)
    {
        percent = Mathf.Clamp(percent, 0, 100);

        if (percent == 0)
        {
            // ������� ��������� ����������� ���������
            for (int i = 0; i < 50; i++) SendVolumeCommand(APPCOMMAND_VOLUME_DOWN);
            // ����� �������� ����
            SendVolumeCommand(APPCOMMAND_VOLUME_MUTE);
        }
        else
        {
            // ������� ��������� ����������� ���������
            for (int i = 0; i < 50; i++) SendVolumeCommand(APPCOMMAND_VOLUME_DOWN);
            // ����� ��������� ������ �������
            for (int i = 0; i < percent / 2; i++) SendVolumeCommand(APPCOMMAND_VOLUME_UP);
        }

        UnityEngine.Debug.Log($"��������� ����������� �� {percent}%");
    }

    private void ChangeVolume(int delta)
    {
        int steps = Mathf.Abs(delta) / 2;
        int command = delta > 0 ? APPCOMMAND_VOLUME_UP : APPCOMMAND_VOLUME_DOWN;

        for (int i = 0; i < steps; i++)
        {
            SendVolumeCommand(command);
        }

        UnityEngine.Debug.Log($"��������� �������� �� {delta}%");
    }

    private void ToggleMute()
    {
        SendVolumeCommand(APPCOMMAND_VOLUME_MUTE);
        UnityEngine.Debug.Log("����� �������� ����������");
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
            UnityEngine.Debug.LogError($"������ �������� �������: {ex.Message}");
        }
    }
}
