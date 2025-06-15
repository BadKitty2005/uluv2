using System.Collections.Generic;
using System.Text.RegularExpressions;
using System;
using UnityEngine;
using System.Linq;

public class Vectorizer
{
    private Dictionary<string, int> ngramIndex = new();
    private Dictionary<string, int> df = new();
    private int totalExamples = 0;
    private List<IntentRecognizer.IntentData> intents;

    public void LoadData(List<IntentRecognizer.IntentData> intentsData)
    {
        intents = intentsData;
        totalExamples = 0;

        foreach (var intent in intents)
        {
            intent.tokenizedExamples = new();
            foreach (var example in intent.examples)
            {
                var tokens = Tokenize(example.ToLower());
                intent.tokenizedExamples.Add(tokens);
                totalExamples++;
            }
        }

        BuildNgramIndex();
        CalculateDF();
        foreach (var intent in intents)
        {
            intent.tfidfVectors = intent.tokenizedExamples.Select(CalculateTfidfVector).ToList();
        }
    }

    public Dictionary<string, float> Vectorize(string input)
    {
        return CalculateTfidfVector(Tokenize(input.ToLower()));
    }

    public List<string> Tokenize(string input)
    {
        string clean = Regex.Replace(input, @"[^\w\s]", "");
        return clean.Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();
    }

    private void BuildNgramIndex()
    {
        int index = 0;
        foreach (var intent in intents)
        {
            foreach (var tokens in intent.tokenizedExamples)
            {
                foreach (var token in tokens)
                    if (!ngramIndex.ContainsKey(token))
                        ngramIndex[token] = index++;

                for (int i = 0; i < tokens.Count - 1; i++)
                {
                    string bigram = $"{tokens[i]}_{tokens[i + 1]}";
                    if (!ngramIndex.ContainsKey(bigram))
                        ngramIndex[bigram] = index++;
                }
            }
        }
    }

    private void CalculateDF()
    {
        foreach (var ngram in ngramIndex.Keys)
        {
            int count = 0;
            foreach (var intent in intents)
            {
                foreach (var tokens in intent.tokenizedExamples)
                {
                    if (ngram.Contains("_"))
                    {
                        var parts = ngram.Split('_');
                        for (int i = 0; i < tokens.Count - 1; i++)
                        {
                            if (tokens[i] == parts[0] && tokens[i + 1] == parts[1])
                            {
                                count++;
                                break;
                            }
                        }
                    }
                    else if (tokens.Contains(ngram))
                    {
                        count++;
                        break;
                    }
                }
            }

            df[ngram] = count;
        }
    }

    private Dictionary<string, float> CalculateTfidfVector(List<string> tokens)
    {
        var vector = new Dictionary<string, float>();

        foreach (var ngram in ngramIndex.Keys)
        {
            float tf = 0f;

            if (ngram.Contains("_"))
            {
                var parts = ngram.Split('_');
                for (int i = 0; i < tokens.Count - 1; i++)
                {
                    if (tokens[i] == parts[0] && tokens[i + 1] == parts[1])
                        tf++;
                }
            }
            else
            {
                tf = tokens.Count(t => t == ngram);
            }

            if (tf > 0 && df.ContainsKey(ngram))
            {
                float idf = Mathf.Log(totalExamples / (float)df[ngram]);
                vector[ngram] = tf * idf;
            }
        }

        return vector;
    }
}
