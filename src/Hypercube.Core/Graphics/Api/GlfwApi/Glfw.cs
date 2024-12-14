﻿// ------------------------------------------------------------------------------
// This code was generated by a Hypercube.Generators
// File: glfw3.h
// Path: Glfw.g.cs
// ------------------------------------------------------------------------------

using System.Runtime.InteropServices;
using Hypercube.Core.Graphics.Api.GlfwApi.Enums;
using Hypercube.Mathematics.Vectors;

namespace Hypercube.Core.Graphics.Api.GlfwApi;

public static unsafe class Glfw
{
    public static bool Init()
    {
        return GlfwNative.glfwInit() == GlfwNative.True;
    }

    public static void Terminate()
    {
        GlfwNative.glfwTerminate();
    }

    public static void InitHint(int hint, int value)
    {
        GlfwNative.glfwInitHint(hint, value);
    }

    public static void InitAllocator(nint* allocator)
    {
        GlfwNative.glfwInitAllocator(allocator);
    }

    public static void InitVulkanLoader(nint loader)
    {
        GlfwNative.glfwInitVulkanLoader(loader);
    }

    public static Version GetVersion()
    {
        var major = 0;
        var minor = 0;
        var revision = 0;
        
        GlfwNative.glfwGetVersion(&major, &minor, &revision);
        
        return new Version(minor, minor, 0, revision);
    }

    public static byte* GetVersionString()
    {
        return GlfwNative.glfwGetVersionString();
    }

    public static ErrorCode GetError(out string description)
    {
        byte* pointer;
        var code =  GlfwNative.glfwGetError(&pointer);
        description = Marshal.PtrToStringUTF8((nint) pointer) ?? string.Empty;
        return (ErrorCode) code;
    }

    public static nint SetErrorCallback(GlfwCallbacks.Error callback)
    {
        return GlfwNative.glfwSetErrorCallback(Marshal.GetFunctionPointerForDelegate(callback));
    }

    public static int GetPlatform()
    {
        return GlfwNative.glfwGetPlatform();
    }

    public static int PlatformSupported(int platform)
    {
        return GlfwNative.glfwPlatformSupported(platform);
    }

    public static nint** GetMonitors(int* count)
    {
        return GlfwNative.glfwGetMonitors(count);
    }

    public static nint* GetPrimaryMonitor()
    {
        return GlfwNative.glfwGetPrimaryMonitor();
    }

    public static Vector2i GetMonitorPos(nint monitor)
    {
        var pointer = &monitor;
        var x = 0;
        var y = 0;
        
        GlfwNative.glfwGetMonitorPos(pointer, &x, &y);

        return new Vector2i(x, y);
    }

    public static void GetMonitorWorkarea(nint* monitor, int* xpos, int* ypos, int* width, int* height)
    {
        GlfwNative.glfwGetMonitorWorkarea(monitor, xpos, ypos, width, height);
    }

    public static void GetMonitorPhysicalSize(nint* monitor, int* widthMM, int* heightMM)
    {
        GlfwNative.glfwGetMonitorPhysicalSize(monitor, widthMM, heightMM);
    }

    public static void GetMonitorContentScale(nint* monitor, float* xscale, float* yscale)
    {
        GlfwNative.glfwGetMonitorContentScale(monitor, xscale, yscale);
    }

    public static byte* GetMonitorName(nint* monitor)
    {
        return GlfwNative.glfwGetMonitorName(monitor);
    }

    public static void SetMonitorUserPointer(nint* monitor, void* pointer)
    {
        GlfwNative.glfwSetMonitorUserPointer(monitor, pointer);
    }

    public static void* GetMonitorUserPointer(nint* monitor)
    {
        return GlfwNative.glfwGetMonitorUserPointer(monitor);
    }

    public static nint SetMonitorCallback(GlfwCallbacks.Monitor callback)
    {
        return GlfwNative.glfwSetMonitorCallback(Marshal.GetFunctionPointerForDelegate(callback));
    }

    public static nint* GetVideoModes(nint* monitor, int* count)
    {
        return GlfwNative.glfwGetVideoModes(monitor, count);
    }

    public static nint* GetVideoMode(nint* monitor)
    {
        return GlfwNative.glfwGetVideoMode(monitor);
    }

    public static void SetGamma(nint* monitor, float gamma)
    {
        GlfwNative.glfwSetGamma(monitor, gamma);
    }

    public static nint* GetGammaRamp(nint* monitor)
    {
        return GlfwNative.glfwGetGammaRamp(monitor);
    }

    public static void SetGammaRamp(nint* monitor, nint* ramp)
    {
        GlfwNative.glfwSetGammaRamp(monitor, ramp);
    }

    public static void DefaultWindowHints()
    {
        GlfwNative.glfwDefaultWindowHints();
    }

    public static void WindowHint(int hint, int value)
    {
        GlfwNative.glfwWindowHint(hint, value);
    }
    
    public static void WindowHint(WindowHintClientApi hint, ClientApi value)
    {
        GlfwNative.glfwWindowHint((int) hint, (int) value);
    }

    public static void WindowHintString(int hint, byte* value)
    {
        GlfwNative.glfwWindowHintString(hint, value);
    }

    public static nint* CreateWindow(int width, int height, string title, nint* monitor, nint* share)
    {
        var byteString = Marshal.StringToCoTaskMemUTF8(title);
        
        try
        {
            return GlfwNative.glfwCreateWindow(width, height, (byte*)byteString, monitor, share);
        }
        finally
        {
            if (byteString != nint.Zero)
                Marshal.FreeCoTaskMem(byteString);
        }
    }

    public static void DestroyWindow(nint* window)
    {
        GlfwNative.glfwDestroyWindow(window);
    }

    public static int WindowShouldClose(nint* window)
    {
        return GlfwNative.glfwWindowShouldClose(window);
    }

    public static void SetWindowShouldClose(nint* window, int value)
    {
        GlfwNative.glfwSetWindowShouldClose(window, value);
    }

    public static byte* GetWindowTitle(nint* window)
    {
        return GlfwNative.glfwGetWindowTitle(window);
    }

    public static void SetWindowTitle(nint window, string title)
    {
        SetWindowTitle((nint*) window, title);
    }

    public static void SetWindowTitle(nint* window, string title)
    {
        var byteString = Marshal.StringToCoTaskMemUTF8(title);
        
        try
        {
            GlfwNative.glfwSetWindowTitle(window, (byte*) byteString);
        }
        finally
        {
            if (byteString != nint.Zero)
                Marshal.FreeCoTaskMem(byteString);
        }
    }

    public static void SetWindowIcon(nint* window, int count, nint* images)
    {
        GlfwNative.glfwSetWindowIcon(window, count, images);
    }

    public static void GetWindowPos(nint* window, int* xpos, int* ypos)
    {
        GlfwNative.glfwGetWindowPos(window, xpos, ypos);
    }

    public static void SetWindowPos(nint* window, int xpos, int ypos)
    {
        GlfwNative.glfwSetWindowPos(window, xpos, ypos);
    }

    public static void GetWindowSize(nint* window, int* width, int* height)
    {
        GlfwNative.glfwGetWindowSize(window, width, height);
    }

    public static void SetWindowSizeLimits(nint* window, int minwidth, int minheight, int maxwidth, int maxheight)
    {
        GlfwNative.glfwSetWindowSizeLimits(window, minwidth, minheight, maxwidth, maxheight);
    }

    public static void SetWindowAspectRatio(nint* window, int numer, int denom)
    {
        GlfwNative.glfwSetWindowAspectRatio(window, numer, denom);
    }

    public static void SetWindowSize(nint* window, int width, int height)
    {
        GlfwNative.glfwSetWindowSize(window, width, height);
    }

    public static void GetFramebufferSize(nint* window, int* width, int* height)
    {
        GlfwNative.glfwGetFramebufferSize(window, width, height);
    }

    public static void GetWindowFrameSize(nint* window, int* left, int* top, int* right, int* bottom)
    {
        GlfwNative.glfwGetWindowFrameSize(window, left, top, right, bottom);
    }

    public static void GetWindowContentScale(nint* window, float* xscale, float* yscale)
    {
        GlfwNative.glfwGetWindowContentScale(window, xscale, yscale);
    }

    public static float GetWindowOpacity(nint* window)
    {
        return GlfwNative.glfwGetWindowOpacity(window);
    }

    public static void SetWindowOpacity(nint* window, float opacity)
    {
        GlfwNative.glfwSetWindowOpacity(window, opacity);
    }

    public static void IconifyWindow(nint* window)
    {
        GlfwNative.glfwIconifyWindow(window);
    }

    public static void RestoreWindow(nint* window)
    {
        GlfwNative.glfwRestoreWindow(window);
    }

    public static void MaximizeWindow(nint* window)
    {
        GlfwNative.glfwMaximizeWindow(window);
    }

    public static void ShowWindow(nint* window)
    {
        GlfwNative.glfwShowWindow(window);
    }

    public static void HideWindow(nint* window)
    {
        GlfwNative.glfwHideWindow(window);
    }

    public static void FocusWindow(nint* window)
    {
        GlfwNative.glfwFocusWindow(window);
    }

    public static void RequestWindowAttention(nint* window)
    {
        GlfwNative.glfwRequestWindowAttention(window);
    }

    public static nint* GetWindowMonitor(nint* window)
    {
        return GlfwNative.glfwGetWindowMonitor(window);
    }

    public static void SetWindowMonitor(nint* window, nint* monitor, int xpos, int ypos, int width, int height, int refreshRate)
    {
        GlfwNative.glfwSetWindowMonitor(window, monitor, xpos, ypos, width, height, refreshRate);
    }

    public static int GetWindowAttrib(nint* window, int attrib)
    {
        return GlfwNative.glfwGetWindowAttrib(window, attrib);
    }

    public static void SetWindowAttrib(nint* window, int attrib, int value)
    {
        GlfwNative.glfwSetWindowAttrib(window, attrib, value);
    }

    public static void SetWindowUserPointer(nint* window, void* pointer)
    {
        GlfwNative.glfwSetWindowUserPointer(window, pointer);
    }

    public static void* GetWindowUserPointer(nint* window)
    {
        return GlfwNative.glfwGetWindowUserPointer(window);
    }

     public static nint SetWindowPositionCallback(nint* window, GlfwCallbacks.WindowPosition callback)
    {
        return GlfwNative.glfwSetWindowPosCallback(window, Marshal.GetFunctionPointerForDelegate(callback));
    }

    public static nint SetWindowSizeCallback(nint* window, GlfwCallbacks.WindowSize callback)
    {
        return GlfwNative.glfwSetWindowSizeCallback(window, Marshal.GetFunctionPointerForDelegate(callback));
    }

    public static nint SetWindowCloseCallback(nint* window, GlfwCallbacks.WindowClose callback)
    {
        return GlfwNative.glfwSetWindowCloseCallback(window, Marshal.GetFunctionPointerForDelegate(callback));
    }

    public static nint SetWindowRefreshCallback(nint* window, GlfwCallbacks.WindowRefresh callback)
    {
        return GlfwNative.glfwSetWindowRefreshCallback(window, Marshal.GetFunctionPointerForDelegate(callback));
    }

    public static nint SetWindowFocusCallback(nint* window, GlfwCallbacks.WindowFocus callback)
    {
        return GlfwNative.glfwSetWindowFocusCallback(window, Marshal.GetFunctionPointerForDelegate(callback));
    }

    public static nint SetWindowIconifyCallback(nint* window, GlfwCallbacks.WindowIconify callback)
    {
        return GlfwNative.glfwSetWindowIconifyCallback(window, Marshal.GetFunctionPointerForDelegate(callback));
    }

    public static nint SetWindowMaximizeCallback(nint* window, GlfwCallbacks.WindowMaximize callback)
    {
        return GlfwNative.glfwSetWindowMaximizeCallback(window, Marshal.GetFunctionPointerForDelegate(callback));
    }

    public static nint SetFramebufferSizeCallback(nint* window, GlfwCallbacks.FrameBufferSize callback)
    {
        return GlfwNative.glfwSetFramebufferSizeCallback(window, Marshal.GetFunctionPointerForDelegate(callback));
    }

    public static nint SetWindowContentScaleCallback(nint* window, GlfwCallbacks.WindowContentScale callback)
    {
        return GlfwNative.glfwSetWindowContentScaleCallback(window, Marshal.GetFunctionPointerForDelegate(callback));
    }

    public static void PollEvents()
    {
        GlfwNative.glfwPollEvents();
    }

    public static void WaitEvents()
    {
        GlfwNative.glfwWaitEvents();
    }

    public static void WaitEventsTimeout(double timeout)
    {
        GlfwNative.glfwWaitEventsTimeout(timeout);
    }

    public static void PostEmptyEvent()
    {
        GlfwNative.glfwPostEmptyEvent();
    }

    public static int GetInputMode(nint* window, int mode)
    {
        return GlfwNative.glfwGetInputMode(window, mode);
    }

    public static void SetInputMode(nint* window, int mode, int value)
    {
        GlfwNative.glfwSetInputMode(window, mode, value);
    }

    public static int RawMouseMotionSupported()
    {
        return GlfwNative.glfwRawMouseMotionSupported();
    }

    public static byte* GetKeyName(int key, int scancode)
    {
        return GlfwNative.glfwGetKeyName(key, scancode);
    }

    public static int GetKeyScancode(int key)
    {
        return GlfwNative.glfwGetKeyScancode(key);
    }

    public static int GetKey(nint* window, int key)
    {
        return GlfwNative.glfwGetKey(window, key);
    }

    public static int GetMouseButton(nint* window, int button)
    {
        return GlfwNative.glfwGetMouseButton(window, button);
    }

    public static void GetCursorPos(nint* window, double* xpos, double* ypos)
    {
        GlfwNative.glfwGetCursorPos(window, xpos, ypos);
    }

    public static void SetCursorPos(nint* window, double xpos, double ypos)
    {
        GlfwNative.glfwSetCursorPos(window, xpos, ypos);
    }

    public static nint* CreateCursor(nint* image, int xhot, int yhot)
    {
        return GlfwNative.glfwCreateCursor(image, xhot, yhot);
    }

    public static nint* CreateStandardCursor(int shape)
    {
        return GlfwNative.glfwCreateStandardCursor(shape);
    }

    public static void DestroyCursor(nint* cursor)
    {
        GlfwNative.glfwDestroyCursor(cursor);
    }

    public static void SetCursor(nint* window, nint* cursor)
    {
        GlfwNative.glfwSetCursor(window, cursor);
    }

    public static nint SetKeyCallback(nint* window, GlfwCallbacks.Key callback)
    {
        return GlfwNative.glfwSetKeyCallback(window, Marshal.GetFunctionPointerForDelegate(callback));
    }

    public static nint SetCharCallback(nint* window, GlfwCallbacks.Char callback)
    {
        return GlfwNative.glfwSetCharCallback(window, Marshal.GetFunctionPointerForDelegate(callback));
    }

    public static nint SetCharModsCallback(nint* window, GlfwCallbacks.CharModification callback)
    {
        return GlfwNative.glfwSetCharModsCallback(window, Marshal.GetFunctionPointerForDelegate(callback));
    }

    public static nint SetMouseButtonCallback(nint* window, GlfwCallbacks.MouseButton callback)
    {
        return GlfwNative.glfwSetMouseButtonCallback(window, Marshal.GetFunctionPointerForDelegate(callback));
    }

    public static nint SetCursorPositionCallback(nint* window, GlfwCallbacks.CursorPosition callback)
    {
        return GlfwNative.glfwSetCursorPosCallback(window, Marshal.GetFunctionPointerForDelegate(callback));
    }

    public static nint SetCursorEnterCallback(nint* window, GlfwCallbacks.CursorEnter callback)
    {
        return GlfwNative.glfwSetCursorEnterCallback(window, Marshal.GetFunctionPointerForDelegate(callback));
    }

    public static nint SetScrollCallback(nint* window, GlfwCallbacks.Scroll callback)
    {
        return GlfwNative.glfwSetScrollCallback(window, Marshal.GetFunctionPointerForDelegate(callback));
    }

    public static nint SetDropCallback(nint* window, GlfwCallbacks.Drop callback)
    {
        return GlfwNative.glfwSetDropCallback(window, Marshal.GetFunctionPointerForDelegate(callback));
    }
    
    public static int JoystickPresent(int jid)
    {
        return GlfwNative.glfwJoystickPresent(jid);
    }

    public static float* GetJoystickAxes(int jid, int* count)
    {
        return GlfwNative.glfwGetJoystickAxes(jid, count);
    }

    public static nint* GetJoystickButtons(int jid, int* count)
    {
        return GlfwNative.glfwGetJoystickButtons(jid, count);
    }

    public static nint* GetJoystickHats(int jid, int* count)
    {
        return GlfwNative.glfwGetJoystickHats(jid, count);
    }

    public static byte* GetJoystickName(int jid)
    {
        return GlfwNative.glfwGetJoystickName(jid);
    }

    public static byte* GetJoystickGUID(int jid)
    {
        return GlfwNative.glfwGetJoystickGUID(jid);
    }

    public static void SetJoystickUserPointer(int jid, void* pointer)
    {
        GlfwNative.glfwSetJoystickUserPointer(jid, pointer);
    }

    public static void* GetJoystickUserPointer(int jid)
    {
        return GlfwNative.glfwGetJoystickUserPointer(jid);
    }

    public static int JoystickIsGamepad(int jid)
    {
        return GlfwNative.glfwJoystickIsGamepad(jid);
    }

    public static nint SetJoystickCallback(GlfwCallbacks.Joystick callback)
    {
        return GlfwNative.glfwSetJoystickCallback(Marshal.GetFunctionPointerForDelegate(callback));
    }
    
    public static int UpdateGamepadMappings(byte* @string)
    {
        return GlfwNative.glfwUpdateGamepadMappings(@string);
    }

    public static byte* GetGamepadName(int jid)
    {
        return GlfwNative.glfwGetGamepadName(jid);
    }

    public static int GetGamepadState(int jid, nint* state)
    {
        return GlfwNative.glfwGetGamepadState(jid, state);
    }

    public static void SetClipboardString(nint* window, byte* @string)
    {
        GlfwNative.glfwSetClipboardString(window, @string);
    }

    public static byte* GetClipboardString(nint* window)
    {
        return GlfwNative.glfwGetClipboardString(window);
    }

    public static double GetTime()
    {
        return GlfwNative.glfwGetTime();
    }

    public static void SetTime(double time)
    {
        GlfwNative.glfwSetTime(time);
    }

    public static nint GetTimerValue()
    {
        return GlfwNative.glfwGetTimerValue();
    }

    public static nint GetTimerFrequency()
    {
        return GlfwNative.glfwGetTimerFrequency();
    }

    public static void MakeContextCurrent(nint* window)
    {
        GlfwNative.glfwMakeContextCurrent(window);
    }

    public static nint* GetCurrentContext()
    {
        return GlfwNative.glfwGetCurrentContext();
    }

    public static void SwapBuffers(nint* window)
    {
        GlfwNative.glfwSwapBuffers(window);
    }

    public static void SwapInterval(int interval)
    {
        GlfwNative.glfwSwapInterval(interval);
    }

    public static int ExtensionSupported(byte* extension)
    {
        return GlfwNative.glfwExtensionSupported(extension);
    }

    public static nint GetProcAddress(byte* procname)
    {
        return Marshal.GetDelegateForFunctionPointer<nint>(
            GlfwNative.glfwGetProcAddress(procname));
    }

    public static int VulkanSupported()
    {
        return GlfwNative.glfwVulkanSupported();
    }

    public static byte** GetRequiredInstanceExtensions(nint* count)
    {
        return GlfwNative.glfwGetRequiredInstanceExtensions(count);
    }

    public static nint GetInstanceProcAddress(nint instance, byte* procname)
    {
        return Marshal.GetDelegateForFunctionPointer<nint>(
            GlfwNative.glfwGetInstanceProcAddress(instance, procname));
    }

    public static int GetPhysicalDevicePresentationSupport(nint instance, nint device, nint queuefamily)
    {
        return GlfwNative.glfwGetPhysicalDevicePresentationSupport(instance, device, queuefamily);
    }

    public static int CreateWindowSurface(nint instance, nint* window, nint* allocator, nint* surface)
    {
        return GlfwNative.glfwCreateWindowSurface(instance, window, allocator, surface);
    }
}
