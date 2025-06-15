using UnityEngine;
using System.IO;
using UnityEngine.Events;
using System;
using Vosk;
using Unity.Barracuda;
using UnityEngine.UIElements;
using Google.Protobuf;
using System.Threading;

public class SpeechRecognizer : MonoBehaviour
{
    //// публичное событие
    //public UnityEvent<string> OnTextRecognized = new(); // <- Это сигнал

    //private VoskRecognizer recognizer;
    //private AudioClip mic;
    //private const int SampleRate = 16000;
    //private int lastSamplePos;

    //void Start()
    //{
    //    Vosk.Vosk.SetLogLevel(0); 
    //    string modelPath = Application.streamingAssetsPath + "/vosk-model-small-ru-0.22";
    //    var model = new Vosk.Model(modelPath);
    //    recognizer = new VoskRecognizer(model, SampleRate);
    //    mic = Microphone.Start(null, true, 10, SampleRate);
    //}
    //void Update()
    //{
    //    if (mic == null || !Microphone.IsRecording(null)) return;

    //    int pos = Microphone.GetPosition(null);
    //    int samplesToRead = pos - lastSamplePos;
    //    if (samplesToRead < 0) samplesToRead += mic.samples;

    //    // Ничего не делать, если нечего читать
    //    if (samplesToRead == 0) return;

    //    // не выходим за пределы
    //    if (samplesToRead > mic.samples) samplesToRead = mic.samples;

    //    float[] samples = new float[samplesToRead];
    //    try
    //    {
    //        mic.GetData(samples, lastSamplePos);
    //    }
    //    catch (Exception e)
    //    {
    //        Debug.LogError($"Ошибка чтения микрофона: {e.Message}");
    //        return;
    //    }

    //    byte[] buffer = new byte[samples.Length * 2];
    //    for (int i = 0; i < samples.Length; i++)
    //    {
    //        short val = (short)(samples[i] * short.MaxValue);
    //        buffer[i * 2] = (byte)(val & 0xff);
    //        buffer[i * 2 + 1] = (byte)((val >> 8) & 0xff);
    //    }

    //    if (recognizer.AcceptWaveform(buffer, buffer.Length))
    //    {
    //        var result = JsonUtility.FromJson<VoskResult>(recognizer.Result());
    //        if (!string.IsNullOrEmpty(result.text))
    //        {
    //            OnTextRecognized.Invoke(result.text);
    //        }
    //    }

    //    lastSamplePos = pos;
    //}
    //[System.Serializable]
    //private class VoskResult { public string text; }

    //void OnDestroy()
    //{
    //    recognizer?.Dispose();
    //    Microphone.End(null);
    //}



    public UnityEvent<string> OnTextRecognized = new();

    private Vosk.VoskRecognizer recognizer;
    private AudioClip mic;
    private const int SampleRate = 16000;
    private int lastSamplePos;

    void Start()
    {
        Vosk.Vosk.SetLogLevel(0);

        string modelPath = Application.streamingAssetsPath + "/vosk-model-small-ru-0.22";
        var model = new Vosk.VoskModel(modelPath);
        recognizer = new Vosk.VoskRecognizer(model, SampleRate);

        mic = Microphone.Start(null, true, 10, SampleRate);
    }

    void Update()
    {
        if (mic == null || !Microphone.IsRecording(null)) return;

        int pos = Microphone.GetPosition(null);
        int samplesToRead = pos - lastSamplePos;
        if (samplesToRead < 0) samplesToRead += mic.samples;

        if (samplesToRead == 0) return;

        if (samplesToRead > mic.samples) samplesToRead = mic.samples;

        float[] samples = new float[samplesToRead];
        try
        {
            mic.GetData(samples, lastSamplePos);
        }
        catch (Exception e)
        {
            Debug.LogError($"Ошибка чтения микрофона: {e.Message}");
            return;
        }

        byte[] buffer = new byte[samples.Length * 2];
        for (int i = 0; i < samples.Length; i++)
        {
            short val = (short)(samples[i] * short.MaxValue);
            buffer[i * 2] = (byte)(val & 0xff);
            buffer[i * 2 + 1] = (byte)((val >> 8) & 0xff);
        }

        if (recognizer.AcceptWaveform(buffer, buffer.Length))
        {
            var result = JsonUtility.FromJson<VoskResult>(recognizer.Result());
            if (!string.IsNullOrEmpty(result.text))
            {
                OnTextRecognized.Invoke(result.text);
            }
        }

        lastSamplePos = pos;
    }

    [Serializable]
    private class VoskResult
    {
        public string text;
    }

    void OnDestroy()
    {
        recognizer?.Dispose();
        Microphone.End(null);
    }
}