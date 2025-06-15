
#if UNITY_STANDALONE_WIN
using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class TransparentWindow : MonoBehaviour
{
    [DllImport("user32.dll")]
    private static extern IntPtr GetActiveWindow();

    [DllImport("user32.dll")]
    private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

    [DllImport("user32.dll")]
    private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter,
        int X, int Y, int cx, int cy, uint uFlags);

    [DllImport("user32.dll")]
    private static extern bool SetLayeredWindowAttributes(IntPtr hwnd, uint crKey, byte bAlpha, uint dwFlags);

    // Стили окна
    private const int GWL_STYLE = -16;
    private const int GWL_EXSTYLE = -20;
    private const int WS_POPUP = unchecked((int)0x80000000);
    private const int WS_VISIBLE = 0x10000000;
    private const int WS_EX_LAYERED = 0x80000;
    private const int WS_EX_TRANSPARENT = 0x20;
    private const int LWA_COLORKEY = 0x1;

    // Флаги для SetWindowPos
    private const uint SWP_NOSIZE = 0x0001;
    private const uint SWP_NOMOVE = 0x0002;
    private const uint SWP_NOZORDER = 0x0004;
    private const uint SWP_FRAMECHANGED = 0x0020;

    void Start()
    {
#if !UNITY_EDITOR
        IntPtr hwnd = GetActiveWindow();
        
        // Полностью убираем стандартные рамки
        SetWindowLong(hwnd, GWL_STYLE, WS_POPUP | WS_VISIBLE);
       SetWindowLong(hwnd, GWL_EXSTYLE, WS_EX_LAYERED);
        
        // Ключевой цвет прозрачности
        SetLayeredWindowAttributes(hwnd, 0x00FF00FF, 0, LWA_COLORKEY);
        
        // Принудительное обновление окна
        SetWindowPos(hwnd, IntPtr.Zero, 0, 0, 0, 0, 
            SWP_NOMOVE | SWP_NOSIZE | SWP_NOZORDER | SWP_FRAMECHANGED);
        
        // Позиционирование
        MoveWindowToBottomRight();
#endif
    }

    void MoveWindowToBottomRight()
    {
        IntPtr hwnd = GetActiveWindow();
        int screenWidth = Screen.currentResolution.width;
        int screenHeight = Screen.currentResolution.height;

        int windowWidth = Screen.width;
        int windowHeight = Screen.height;

        SetWindowPos(hwnd, IntPtr.Zero,
            screenWidth - windowWidth - 10,
            screenHeight - windowHeight - 10,
            0, 0, SWP_NOSIZE | SWP_NOZORDER);
    }
}
#endif