﻿using Hypercube.Client.Graphics.Events;
using Hypercube.Graphics;
using Hypercube.Graphics.Windowing;

namespace Hypercube.Client.Graphics.Realisation.OpenGL.Rendering;

public sealed partial class Renderer
{
    public WindowHandle MainWindow => _windows[_mainWindowId];
    public IReadOnlyDictionary<WindowId, WindowHandle> Windows => _windows; 
    
    private readonly Dictionary<WindowId, WindowHandle> _windows = new();
    private WindowId _mainWindowId = WindowId.Invalid;
    
    public void EnterWindowLoop()
    {
        _windowing.EnterWindowLoop();
    }

    public void TerminateWindowLoop()
    {
        _windowing.Terminate();
    }

    public WindowHandle CreateWindow(WindowCreateSettings settings)
    {
        var (registration, error) = CreateWindow(_context, settings, MainWindow);
        if (registration is null)
            throw new Exception(error);

        return registration;
    }
    
    public void DestroyWindow(WindowHandle handle)
    {
        _windowing.WindowDestroy(handle);
        _windows.Remove(handle.Id);
    }

    public void CloseWindow(WindowHandle handle)
    {
        _eventBus.Raise(new WindowClosedEvent(handle));
        
        if (handle.Id == _mainWindowId)
        {
            _eventBus.Raise(new MainWindowClosedEvent(handle));
            return;
        }
        
        if (!handle.DisposeOnClose)
            return;
        
        DestroyWindow(handle);
    }
    
    private bool InitMainWindow(IContextInfo? context, WindowCreateSettings settings)
    {
        var (registration, error) = CreateWindow(context, settings, null);
        if (registration is null)
        {
            _logger.Error($"Initializing main window failed, error:\n{error}");
            return false;
        }
        
        _mainWindowId = registration.Id;
        return true;
    }

    private (WindowHandle? registration, string? error) CreateWindow(IContextInfo? context, WindowCreateSettings settings, WindowHandle? share)
    {
        var result = _windowing.WindowCreate(context, settings, share);
        if (result.Failed)
            return (null, result.Error);

        var handle = result.Registration;
        if (handle is null)
            return (null, "registration is null");
        
        _windowing.MakeContextCurrent(handle);
        _windows.Add(handle.Id, handle);
        
        _logger.EngineInfo($"Created new {handle}");
        
        return (handle, null);
    }

    public void OnFocusChanged(WindowHandle window, bool focused)
    {
        _eventBus.Raise(new WindowFocusChangedEvent(window, focused));
    }
}