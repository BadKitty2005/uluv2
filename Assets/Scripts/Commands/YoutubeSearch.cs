using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

public class YoutubeSearch
{
    private string userText;

    private List<string> stopWords = new List<string>
    {
        "покажи", "включи", "на", "ютубе", "видео", "ролик",
        "про", "хочу", "посмотреть", "найди", "мне", "найти", "включи"
    };

    public YoutubeSearch(string fullText)
    {
        userText = fullText.ToLower();
    }

    public void Execute()
    {
        string query = ExtractSearchQuery(userText);
        if (string.IsNullOrEmpty(query))
        {
            Debug.LogWarning("Не удалось извлечь поисковый запрос из фразы: " + userText);
            return;
        }

        string encodedQuery = Uri.EscapeDataString(query);
        string url = $"https://www.youtube.com/results?search_query={encodedQuery}";
        Debug.Log($"Открываю YouTube с запросом: {query}");
        Application.OpenURL(url);
    }

    private string ExtractSearchQuery(string inputText)
    {
        var words = inputText.Split(' ')
            .Where(word => !string.IsNullOrWhiteSpace(word))
            .Where(word => !stopWords.Contains(word))
            .ToArray();

        return string.Join(" ", words).Trim();
    }
}
