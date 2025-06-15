using System.Collections.Generic;
using System.Windows.Input;
using System;
using UnityEngine;
using System.Reflection;
using System.Diagnostics;

public class CommandDispatcher : MonoBehaviour
{
    private OpServ opSer; // Экземпляр класса OpServ
    private Calculate calc;
    private DeviceController contr;
    private OpenApp oppapp;
    void Start()
    {
        // Инициализируем экземпляр OpServ
        opSer = new OpServ();
        calc = new Calculate();
        contr = new DeviceController();
        oppapp = new OpenApp();
    }
    public GameObject canv;
    public void ExecuteCommand(string intent, string fullText = "")
    {
        switch (intent)
        {
            case "open_browser":
                Application.OpenURL("https://google.com");
                break;
            case "take_screenshot":
                new TakeScreenShot().Execute();
                break;
            case "search_youtube":
                new YoutubeSearch(fullText).Execute();
                break;
            case "shutdown_pc":
                Process.Start("shutdown", "/s /t 0");
                break;
            case "restart_pc":
                Process.Start("shutdown", "/r /t 0");
                break;
            case "search_file":
                FindFile.FindAndRevealFile(fullText);
                break;
            case "open_folder":
                FindFolder.FindAndRevealFolder(fullText);
                break;
            case "delete_file":
                DeleteFile.DeleteFileByName(fullText);
                break;
            case "adjust_volume":
                new Volume().AdjustVolume(fullText);
                break;
            case "adjust_brightness":
                new Brightness().AdjustBrightness(fullText);
                break;
            case "find_answer":
                new FindAnswer().Execute(fullText);
                break;
            case "search_rutube":
                Application.OpenURL("https://rutube.ru/");
                break;
            case "open_bin":
                OpenBin.OpenRecycleBin();
                break;
            case "clean_bin":
                CleanBin.ClearBin();
                break;
            case "close_app":
                CloseApp.CloseLastOpenedApp();
                break;
            case "customize_assistant":
                canv.SetActive(true);
                break;
            case "open_service":
                opSer.OpS(fullText);
                break;
            case "calculate":
                calc.CalculateExpression(fullText);
                break;
            case "open_app":
                oppapp.ProcessVoiceCommand(fullText);
                break;
            case "goodbye":
                Application.Quit();
                break;
            default:
                UnityEngine.Debug.Log($"Неизвестная команда: {intent}");
                break;
        }
    }
}
