﻿namespace Hypercube.Core.Rendering.Graphics.Window;

public sealed unsafe class WindowHandle
{
    public readonly nint* Handle;
    
    public WindowHandle(nint* handle)
    {
        Handle = handle;
    }

    public static explicit operator nint*(WindowHandle windowHandle)
    {
        return windowHandle.Handle;
    }
}