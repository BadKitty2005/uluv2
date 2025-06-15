using System.IO;
using UnityEngine;

public class TakeScreenShot
{
    public void Execute()
    {

        string picturesPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyPictures);
        string screenshotName = "Screenshot_" + System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".png";
        string screenshotPath = Path.Combine(picturesPath, screenshotName);

        // Делаем скриншот
        ScreenCapture.CaptureScreenshot(screenshotPath);
        Debug.Log($"Скриншот сохранён: {screenshotPath}");
    }
}
