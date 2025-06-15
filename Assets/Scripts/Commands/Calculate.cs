using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;

public class Calculate
{
    [SerializeField] private TextMeshPro resultText; // ������ �� ��������� ��������� ������

    public void ProcessVoiceInput(string voiceInput)
    {
        if (string.IsNullOrEmpty(voiceInput))
        {
            Debug.LogError("������� ������ �����");
            return;
        }

        Debug.Log($"������� ��������� ����: {voiceInput}");

        // ��������� ���������
        string result = CalculateExpression(voiceInput);
        if (result != null)
        {
            resultText.text = result; // ��������� ����� �� ������
            Debug.Log($"���������: {result}");
        }
        else
        {
            Debug.LogWarning($"�� ������� ��������� ���������: {voiceInput}");
            resultText.text = "������ ����������!";
        }
    }

    public string CalculateExpression(string input)
    {
        try
        {
            if (string.IsNullOrEmpty(input)) return null;

            // ������� ��� �������� ������� ���� � �����
            var numberWords = new Dictionary<string, float>
            {
                { "����", 0f }, { "����", 1f }, { "���", 2f }, { "���", 3f },
                { "������", 4f }, { "����", 5f }, { "�����", 6f }, { "����", 7f },
                { "������", 8f }, { "������", 9f }, { "������", 10f }
            };
            var scaleWords = new Dictionary<string, float>
            {
                { "������", 10f }, { "��������", 20f }, { "��������", 30f },
                { "�����", 40f }, { "���������", 50f }, { "����������", 60f },
                { "���������", 70f }, { "�����������", 80f }, { "���������", 90f },
                { "���", 100f }, { "������", 200f }, { "������", 300f },
                { "���������", 400f }, { "�������", 500f }, { "��������", 600f },
                { "�������", 700f }, { "���������", 800f }, { "���������", 900f },
                { "������", 1000f }, { "������", 1000f }
            };

            // ��������� �� ����������
            string[] operators = { "����", "�����", "��������", "��", "���������", "+", "-", "*", "/" };
            string cleanInput = input.ToLower().Replace(" ", "");
            int opIndex = -1;
            string op = null;

            // ���� ������ ��������
            foreach (var oper in operators)
            {
                opIndex = cleanInput.IndexOf(oper);
                if (opIndex >= 0)
                {
                    op = oper;
                    break;
                }
            }

            if (opIndex < 0 || op == null) return null;

            // ��������� �� ��������
            string num1Str = input.Substring(0, input.IndexOf(op, StringComparison.CurrentCultureIgnoreCase)).Trim();
            string num2Str = input.Substring(input.IndexOf(op, StringComparison.CurrentCultureIgnoreCase) + op.Length).Trim();

            // ������ ������ �������
            float a = ParseNumber(num1Str, numberWords, scaleWords);
            if (a == 0f && !float.TryParse(num1Str, out a)) return null;

            // ������ ������ �������
            float b = ParseNumber(num2Str, numberWords, scaleWords);
            if (b == 0f && !float.TryParse(num2Str, out b)) return null;

            // ��������� ��������
            switch (op)
            {
                case "����":
                case "+":
                    return $"{num1Str} + {num2Str} = {(a + b)}";
                case "�����":
                case "-":
                    return $"{num1Str} - {num2Str} = {(a - b)}";
                case "��������":
                case "��":
                case "*":
                    return $"{num1Str} * {num2Str} = {(a * b)}";
                case "���������":
                case "/":
                    if (b == 0) return "������� �� ����!";
                    return $"{num1Str} / {num2Str} = {(a / b)}";
                default:
                    return null;
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"������ ����������: {ex.Message}");
            return null;
        }
    }

    private float ParseNumber(string input, Dictionary<string, float> numberWords, Dictionary<string, float> scaleWords)
    {
        float result = 0f;
        float current = 0f;
        float lastScale = 0f;

        string[] words = input.ToLower().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

        foreach (string word in words)
        {
            if (numberWords.TryGetValue(word, out float value))
            {
                current = value;
            }
            else if (scaleWords.TryGetValue(word, out float scale))
            {
                if (scale >= 100f)
                {
                    result += current * scale;
                    current = 0f;
                    lastScale = scale;
                }
                else
                {
                    current += scale;
                }
            }
        }

        result += current;
        if (lastScale > 0 && result < lastScale) result *= lastScale; // ������������� ��� �����

        return result > 0 ? result : float.NaN;
    }
}
