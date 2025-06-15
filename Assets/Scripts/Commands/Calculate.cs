using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;

public class Calculate
{
    [SerializeField] private TextMeshPro resultText; // Ссылка на текстовый компонент панели

    public void ProcessVoiceInput(string voiceInput)
    {
        if (string.IsNullOrEmpty(voiceInput))
        {
            Debug.LogError("Входная строка пуста");
            return;
        }

        Debug.Log($"Получен голосовой ввод: {voiceInput}");

        // Вычисляем результат
        string result = CalculateExpression(voiceInput);
        if (result != null)
        {
            resultText.text = result; // Обновляем текст на панели
            Debug.Log($"Результат: {result}");
        }
        else
        {
            Debug.LogWarning($"Не удалось вычислить выражение: {voiceInput}");
            resultText.text = "Ошибка вычисления!";
        }
    }

    public string CalculateExpression(string input)
    {
        try
        {
            if (string.IsNullOrEmpty(input)) return null;

            // Словари для перевода русских слов в числа
            var numberWords = new Dictionary<string, float>
            {
                { "ноль", 0f }, { "один", 1f }, { "два", 2f }, { "три", 3f },
                { "четыре", 4f }, { "пять", 5f }, { "шесть", 6f }, { "семь", 7f },
                { "восемь", 8f }, { "девять", 9f }, { "десять", 10f }
            };
            var scaleWords = new Dictionary<string, float>
            {
                { "десять", 10f }, { "двадцать", 20f }, { "тридцать", 30f },
                { "сорок", 40f }, { "пятьдесят", 50f }, { "шестьдесят", 60f },
                { "семьдесят", 70f }, { "восемьдесят", 80f }, { "девяносто", 90f },
                { "сто", 100f }, { "двести", 200f }, { "триста", 300f },
                { "четыреста", 400f }, { "пятьсот", 500f }, { "шестьсот", 600f },
                { "семьсот", 700f }, { "восемьсот", 800f }, { "девятьсот", 900f },
                { "тысяча", 1000f }, { "тысячи", 1000f }
            };

            // Разбиваем по операторам
            string[] operators = { "плюс", "минус", "умножить", "на", "разделить", "+", "-", "*", "/" };
            string cleanInput = input.ToLower().Replace(" ", "");
            int opIndex = -1;
            string op = null;

            // Ищем первый оператор
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

            // Разделяем на операнды
            string num1Str = input.Substring(0, input.IndexOf(op, StringComparison.CurrentCultureIgnoreCase)).Trim();
            string num2Str = input.Substring(input.IndexOf(op, StringComparison.CurrentCultureIgnoreCase) + op.Length).Trim();

            // Парсим первый операнд
            float a = ParseNumber(num1Str, numberWords, scaleWords);
            if (a == 0f && !float.TryParse(num1Str, out a)) return null;

            // Парсим второй операнд
            float b = ParseNumber(num2Str, numberWords, scaleWords);
            if (b == 0f && !float.TryParse(num2Str, out b)) return null;

            // Выполняем операцию
            switch (op)
            {
                case "плюс":
                case "+":
                    return $"{num1Str} + {num2Str} = {(a + b)}";
                case "минус":
                case "-":
                    return $"{num1Str} - {num2Str} = {(a - b)}";
                case "умножить":
                case "на":
                case "*":
                    return $"{num1Str} * {num2Str} = {(a * b)}";
                case "разделить":
                case "/":
                    if (b == 0) return "Деление на ноль!";
                    return $"{num1Str} / {num2Str} = {(a / b)}";
                default:
                    return null;
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Ошибка вычисления: {ex.Message}");
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
        if (lastScale > 0 && result < lastScale) result *= lastScale; // Корректировка для тысяч

        return result > 0 ? result : float.NaN;
    }
}
