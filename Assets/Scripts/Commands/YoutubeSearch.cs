using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

public class YoutubeSearch
{
    private string userText;

    private List<string> stopWords = new List<string>
    {
        "������", "������", "��", "�����", "�����", "�����",
        "���", "����", "����������", "�����", "���", "�����", "������"
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
            Debug.LogWarning("�� ������� ������� ��������� ������ �� �����: " + userText);
            return;
        }

        string encodedQuery = Uri.EscapeDataString(query);
        string url = $"https://www.youtube.com/results?search_query={encodedQuery}";
        Debug.Log($"�������� YouTube � ��������: {query}");
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
