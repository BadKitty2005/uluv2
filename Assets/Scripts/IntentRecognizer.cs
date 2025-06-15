using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Timeline;
using Vosk;
using Newtonsoft.Json;
using Unity.Barracuda;
using System;

public class IntentRecognizer : MonoBehaviour
{
    public class IntentData
    {
        public string intent;
        public List<string> examples;
        [NonSerialized] public List<List<string>> tokenizedExamples;
        [NonSerialized] public List<Dictionary<string, float>> tfidfVectors;
    }

    public List<IntentData> intents = new();
    private Vectorizer vectorizer = new();

    void Start()
    {
        LoadIntentsFromJson();
        vectorizer.LoadData(intents);
    }

    void LoadIntentsFromJson()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "intents.json");
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            intents = JsonConvert.DeserializeObject<List<IntentData>>(json);
        }
        else
        {
            Debug.LogError("Файл intents.json не найден в StreamingAssets.");
        }
    }

    public string RecognizeIntent(string input)
    {
        var inputVec = vectorizer.Vectorize(input);
        string bestIntent = null;
        float bestScore = 0f;
        const float threshold = 0.4f;

        foreach (var intent in intents)
        {
            foreach (var vec in intent.tfidfVectors)
            {
                float score = CosineSimilarity(inputVec, vec);
                if (score > bestScore)
                {
                    bestScore = score;
                    bestIntent = intent.intent;
                }
            }
        }
        if (bestScore < threshold)
        {
            Debug.Log($"Интент не найден, лучшая оценка {bestScore} меньше порога {threshold}");
            return "unknown";
        }
        Debug.Log($"Распознанный интент: {bestIntent} (score: {bestScore})");
        return bestIntent;
    }

    float CosineSimilarity(Dictionary<string, float> v1, Dictionary<string, float> v2)
    {
        float dot = 0f, mag1 = 0f, mag2 = 0f;
        foreach (var kv in v1)
        {
            if (v2.TryGetValue(kv.Key, out float val2))
                dot += kv.Value * val2;
            mag1 += kv.Value * kv.Value;
        }

        foreach (var val in v2.Values)
            mag2 += val * val;

        return (mag1 > 0 && mag2 > 0) ? dot / (Mathf.Sqrt(mag1) * Mathf.Sqrt(mag2)) : 0f;
    }
}
