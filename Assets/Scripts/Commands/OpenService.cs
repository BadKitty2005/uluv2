using System.Collections.Generic;
using UnityEngine;

public class OpenService
{
    private Dictionary<string, string> services = new()
    {
        { "ютуб", "https://youtube.com" },
        { "почта", "https://mail.google.com" },
        { "твич", "https://twitch.tv" },
        { "гитхаб", "https://github.com" },
        { "телеграм", "https://web.telegram.org" },
        { "спотифай", "https://www.spotify.com" },
        { "вк", "https://www.spotify.com" },


    };
    public void OpS(string userText)
    {
         string lower = userText.ToLower();
        foreach (var kv in services)
        {
            if (lower.Contains(kv.Key))
            {
                Application.OpenURL(kv.Value);
                Debug.Log($"Открываю сервис: {kv.Value}");
                return;
            }
        }

        Debug.LogWarning($"[OpenService] Не удалось распознать сервис во фразе: {userText}");
    }
}
