using System.Collections.Generic;
using UnityEngine;

public class OpenService
{
    private Dictionary<string, string> services = new()
    {
        { "����", "https://youtube.com" },
        { "�����", "https://mail.google.com" },
        { "����", "https://twitch.tv" },
        { "������", "https://github.com" },
        { "��������", "https://web.telegram.org" },
        { "��������", "https://www.spotify.com" },
        { "��", "https://www.spotify.com" },


    };
    public void OpS(string userText)
    {
         string lower = userText.ToLower();
        foreach (var kv in services)
        {
            if (lower.Contains(kv.Key))
            {
                Application.OpenURL(kv.Value);
                Debug.Log($"�������� ������: {kv.Value}");
                return;
            }
        }

        Debug.LogWarning($"[OpenService] �� ������� ���������� ������ �� �����: {userText}");
    }
}
