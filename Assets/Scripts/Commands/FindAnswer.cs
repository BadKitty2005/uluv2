using System.Diagnostics;
using System;
using UnityEngine;

public class FindAnswer
{
    public void Execute(string userQuestion)
    {
        if (string.IsNullOrWhiteSpace(userQuestion)) return;

        string encodedQuery = Uri.EscapeDataString(userQuestion);
        string url = "https://www.google.com/search?q=" + encodedQuery;
        Process.Start(url);
    }
}
