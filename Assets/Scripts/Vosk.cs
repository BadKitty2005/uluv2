using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

namespace Vosk
{
    //    public class Model : IDisposable
    //    {
    //        private IntPtr handle;

    //        [DllImport("libvosk", CallingConvention = CallingConvention.Cdecl)]
    //        private static extern IntPtr vosk_model_new(IntPtr path);

    //        [DllImport("libvosk", CallingConvention = CallingConvention.Cdecl)]
    //        private static extern void vosk_model_free(IntPtr model);

    //        public Model(string path)
    //        {
    //            IntPtr pathPtr = Marshal.StringToHGlobalAnsi(path); // Конвертируем в ANSI
    //            try
    //            {
    //                handle = vosk_model_new(pathPtr);
    //                if (handle == IntPtr.Zero)
    //                {
    //                    UnityEngine.Debug.LogError("Failed to create Vosk model at path: " + path);
    //                    throw new Exception("Vosk model initialization failed");
    //                }
    //                UnityEngine.Debug.Log("Vosk model created successfully at: " + path);
    //            }
    //            finally
    //            {
    //                Marshal.FreeHGlobal(pathPtr); // Освобождаем память
    //            }
    //        }

    //        public IntPtr GetHandle()
    //        {
    //            return handle;
    //        }

    //        public void Dispose()
    //        {
    //            if (handle != IntPtr.Zero)
    //                vosk_model_free(handle);
    //        }
    //    }

    //    public class VoskRecognizer : IDisposable
    //    {
    //        private IntPtr handle;

    //        [DllImport("libvosk", CallingConvention = CallingConvention.Cdecl)]
    //        private static extern IntPtr vosk_recognizer_new(IntPtr model, float sample_rate);

    //        [DllImport("libvosk", CallingConvention = CallingConvention.Cdecl)]
    //        private static extern int vosk_recognizer_accept_waveform(IntPtr recognizer, byte[] data, int length);

    //        [DllImport("libvosk", CallingConvention = CallingConvention.Cdecl)]
    //        private static extern IntPtr vosk_recognizer_result(IntPtr recognizer);

    //        [DllImport("libvosk", CallingConvention = CallingConvention.Cdecl)]
    //        private static extern IntPtr vosk_recognizer_partial_result(IntPtr recognizer);

    //        [DllImport("libvosk", CallingConvention = CallingConvention.Cdecl)]
    //        private static extern IntPtr vosk_recognizer_final_result(IntPtr recognizer);

    //        [DllImport("libvosk", CallingConvention = CallingConvention.Cdecl)]
    //        private static extern void vosk_recognizer_free(IntPtr recognizer);

    //        public VoskRecognizer(Model model, float sampleRate)
    //        {
    //            handle = vosk_recognizer_new(model.GetHandle(), sampleRate);
    //        }

    //        public bool AcceptWaveform(byte[] data, int length)
    //        {
    //            return vosk_recognizer_accept_waveform(handle, data, length) != 0;
    //        }

    //        public string Result()
    //        {
    //            return Marshal.PtrToStringAnsi(vosk_recognizer_result(handle));
    //        }

    //        public string PartialResult()
    //        {
    //            return Marshal.PtrToStringAnsi(vosk_recognizer_partial_result(handle));
    //        }

    //        public string FinalResult()
    //        {
    //            return Marshal.PtrToStringAnsi(vosk_recognizer_final_result(handle));
    //        }

    //        public void Dispose()
    //        {
    //            vosk_recognizer_free(handle);
    //        }
    //    }

    //    public static class Vosk
    //    {
    //        [DllImport("libvosk", CallingConvention = CallingConvention.Cdecl)]
    //        private static extern void vosk_set_log_level(int level);

    //        public static void SetLogLevel(int level)
    //        {
    //            vosk_set_log_level(level); //ВОТ ЗДЕСЬ DllNotFoundException: libvosk assembly:<unknown assembly> type:<unknown type> member:(null)
    //            //Vosk.Vosk.SetLogLevel(System.Int32 level)(at Assets / Scripts / Vosk.cs:93)
    //        }
    //    }


    public class VoskModel : IDisposable
    {
        private IntPtr handle;

        [DllImport("libvosk", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr vosk_model_new(IntPtr path);

        [DllImport("libvosk", CallingConvention = CallingConvention.Cdecl)]
        private static extern void vosk_model_free(IntPtr model);

        public VoskModel(string path)
        {
            IntPtr pathPtr = Marshal.StringToHGlobalAnsi(path);
            try
            {
                handle = vosk_model_new(pathPtr);
                if (handle == IntPtr.Zero)
                {
                    UnityEngine.Debug.LogError("Failed to create Vosk model at path: " + path);
                    throw new Exception("Vosk model initialization failed");
                }
                UnityEngine.Debug.Log("Vosk model created successfully at: " + path);
            }
            finally
            {
                Marshal.FreeHGlobal(pathPtr);
            }
        }

        public IntPtr GetHandle()
        {
            return handle;
        }

        public void Dispose()
        {
            if (handle != IntPtr.Zero)
            {
                vosk_model_free(handle);
                handle = IntPtr.Zero;
            }
        }
    }

    public class VoskRecognizer : IDisposable
    {
        private IntPtr handle;

        [DllImport("libvosk", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr vosk_recognizer_new(IntPtr model, float sample_rate);

        [DllImport("libvosk", CallingConvention = CallingConvention.Cdecl)]
        private static extern int vosk_recognizer_accept_waveform(IntPtr recognizer, byte[] data, int length);

        [DllImport("libvosk", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr vosk_recognizer_result(IntPtr recognizer);

        [DllImport("libvosk", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr vosk_recognizer_partial_result(IntPtr recognizer);

        [DllImport("libvosk", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr vosk_recognizer_final_result(IntPtr recognizer);

        [DllImport("libvosk", CallingConvention = CallingConvention.Cdecl)]
        private static extern void vosk_recognizer_free(IntPtr recognizer);

        public VoskRecognizer(VoskModel model, float sampleRate)
        {
            handle = vosk_recognizer_new(model.GetHandle(), sampleRate);
            if (handle == IntPtr.Zero)
            {
                UnityEngine.Debug.LogError("Failed to create Vosk recognizer");
                throw new Exception("Vosk recognizer initialization failed");
            }
        }

        public bool AcceptWaveform(byte[] data, int length)
        {
            return vosk_recognizer_accept_waveform(handle, data, length) != 0;
        }

        public string Result()
        {
            return Marshal.PtrToStringAnsi(vosk_recognizer_result(handle));
        }

        public string PartialResult()
        {
            return Marshal.PtrToStringAnsi(vosk_recognizer_partial_result(handle));
        }

        public string FinalResult()
        {
            return Marshal.PtrToStringAnsi(vosk_recognizer_final_result(handle));
        }

        public void Dispose()
        {
            if (handle != IntPtr.Zero)
            {
                vosk_recognizer_free(handle);
                handle = IntPtr.Zero;
            }
        }
    }

    public static class Vosk
    {
        [DllImport("libvosk", CallingConvention = CallingConvention.Cdecl)]
        private static extern void vosk_set_log_level(int level);

        public static void SetLogLevel(int level)
        {
            vosk_set_log_level(level);
        }
    }
}
