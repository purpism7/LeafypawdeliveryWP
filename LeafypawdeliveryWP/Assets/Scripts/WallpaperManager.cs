using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class WallpaperManager : MonoBehaviour
{
    #region WinAPI Imports
    // 윈도우 OS의 핵심 기능을 가져옵니다.
    [DllImport("user32.dll")]
    public static extern IntPtr FindWindow(string className, string windowName);

    [DllImport("user32.dll")]
    public static extern int SendMessage(IntPtr hWnd, int wMsg, int wParam, int lParam);

    [DllImport("user32.dll")]
    public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

    [DllImport("user32.dll")]
    public static extern IntPtr FindWindowEx(IntPtr parentHandle, IntPtr childAfter, string className, string windowTitle);

    [DllImport("user32.dll")]
    public static extern bool EnumWindows(EnumWindowsProc enumProc, IntPtr lParam);

    [DllImport("user32.dll")]
    public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

    [DllImport("user32.dll")]
    public static extern int GetWindowLong(IntPtr hWnd, int nIndex);

    [DllImport("user32.dll")]
    public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

    public delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);
    #endregion

    // 바탕화면 뒤로 보낼 때 필요한 매직 넘버들
    private const int GWL_STYLE = -16;
    private const int WS_BORDER = 0x00800000;
    private const int WS_CAPTION = 0x00C00000;
    private const int WS_SYSMENU = 0x00080000;
    private const int WS_MINIMIZEBOX = 0x00020000;

    private const uint SWP_SHOWWINDOW = 0x0040;

    [Header("사이드바 설정")]
    [Tooltip("사이드바의 가로 너비 (픽셀 단위)")]
    public int sidebarWidth = 500;

    [Tooltip("체크하면 오른쪽 배치, 끄면 왼쪽 배치")]
    public bool alignRight = true;

    void Start()
    {
        // 빌드된 게임에서만 실행 (에디터에서는 실행 방지)
#if !UNITY_EDITOR
        InitializeWallpaper();
#endif
    }

    void InitializeWallpaper()
    {
        // 1. 내 게임 창 찾기 (Product Name이 중요!)
        IntPtr unityWindow = FindWindow(null, Application.productName);

        if (unityWindow == IntPtr.Zero)
        {
            // 이름을 못 찾았을 경우를 대비해 클래스 이름으로 한 번 더 시도
            unityWindow = FindWindow("UnityWndClass", null);
        }

        // 2. 창 테두리 제거
        int style = GetWindowLong(unityWindow, GWL_STYLE);
        SetWindowLong(unityWindow, GWL_STYLE, style & ~(WS_BORDER | WS_CAPTION | WS_SYSMENU | WS_MINIMIZEBOX));

        // 3. 윈도우 바탕화면 관리자(Progman) 찾기
        IntPtr progman = FindWindow("Progman", null);

        // 4. 바탕화면 생성 메시지 전송 (0x052C)
        SendMessage(progman, 0x052C, 0, 0);

        // 5. WorkerW(진짜 배경화면 레이어) 찾기
        IntPtr workerw = IntPtr.Zero;
        EnumWindows((hwnd, lParam) =>
        {
            IntPtr shellDll = FindWindowEx(hwnd, IntPtr.Zero, "SHELLDLL_DefView", null);
            if (shellDll != IntPtr.Zero)
            {
                workerw = FindWindowEx(IntPtr.Zero, hwnd, "WorkerW", null);
            }
            return true;
        }, IntPtr.Zero);

        if (workerw == IntPtr.Zero) workerw = progman;

        // 6. 부모 변경 (입양)
        SetParent(unityWindow, workerw);

        // 7. 전체화면으로 크기 맞추기
        int screenWidth = Screen.currentResolution.width;
        int screenHeight = Screen.currentResolution.height;

        int finalX = 0;
        int finalY = 0; // 맨 위
        int finalWidth = sidebarWidth; // 설정한 너비
        int finalHeight = screenHeight; // 세로는 꽉 차게

        //if (alignRight)
        //{
        //    // 오른쪽 배치: (전체화면 너비 - 사이드바 너비)가 시작점 X
        //    finalX = screenWidth - sidebarWidth;
        //}
        //else
        //{
        //    // 왼쪽 배치: 시작점 X는 0
        //    finalX = 0;
        //}

        SetWindowPos(unityWindow, IntPtr.Zero, finalX, finalY, finalWidth, finalHeight, SWP_SHOWWINDOW);
    }
}