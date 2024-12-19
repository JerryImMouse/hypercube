﻿using Hypercube.Core.Graphics.Api.GlfwApi;
using Hypercube.Core.Graphics.Api.GlfwApi.Enums;

namespace Hypercube.Core.Graphics.Windowing.Api.GlfwWindowing;

public unsafe partial class GlfwWindowing
{   
    private void Raise(ICommand command)
    {
        _logger.Trace($"Handled command {command.GetType().Name} (multiThread: {_multiThread})");
        
        if (_multiThread)
        {
            _commandBridge.Raise(command);
            
            if (_waitEventsTimeout == 0)
                Glfw.PostEmptyEvent();
            
            return;
        }
        
        Process(command);
    }

    private void ProcessCommands(bool single = false)
    {
        while (_commandBridge.TryRead(out var command))
        {
            _logger.Trace($"Read command {command.GetType().Name} (multiThread: {_multiThread})");
            Process(command);
            
            if (single)
                break;
        }
    }
    
    private void Process(ICommand command)
    {
        _logger.Trace($"Processed command {command.GetType().Name} (multiThread: {_multiThread})");
        
        switch (command)
        {
            case CommandTerminate:
                _running = false;
                _eventBridge.CompleteWrite();
                
                Glfw.Terminate();
                
                Console.WriteLine("Terminate");
                break;
            
            case CommandWindowSetTitle commandWindowSetTitle:
                Glfw.SetWindowTitle(commandWindowSetTitle.Window, commandWindowSetTitle.Title);
                break;
            
            case CommandCreateWindow commandCreateWindow:
                var size = commandCreateWindow.Settings.Size;
                var title = commandCreateWindow.Settings.Title;
                var monitor = commandCreateWindow.Settings.MonitorShare;
                var share = commandCreateWindow.Settings.ContextShare;
                
                Glfw.WindowHint(WindowHintClientApi.ClientApi, ClientApi.OpenglApi);
                
                var window = Glfw.CreateWindow(size.X, size.Y, title, monitor is not null ? monitor.Handle : null, share is not null ? share.Handle : null);
                if (window == null)
                {
                    Raise(new EventWindowCreated(nint.Zero, commandCreateWindow.TaskCompletionSource));
                    throw new InvalidOperationException($"Failed to create window '{title}' with size {size.X}x{size.Y}. Ensure that the system supports OpenGL and that a valid window context is provided.");
                }
                
                Glfw.MakeContextCurrent(window);
                
                Glfw.SetWindowPositionCallback(window, _windowPositionCallback);
                Glfw.SetWindowSizeCallback(window, _windowSizeCallback);
                Glfw.SetWindowCloseCallback(window, _windowCloseCallback);
                Glfw.SetWindowRefreshCallback(window, _windowRefreshCallback);
                Glfw.SetWindowFocusCallback(window, _windowFocusCallback);
                Glfw.SetWindowIconifyCallback(window, _windowIconifyCallback);
                Glfw.SetWindowMaximizeCallback(window, _windowMaximizeCallback);
                Glfw.SetFramebufferSizeCallback(window, _frameBufferSizeCallback);
                Glfw.SetWindowContentScaleCallback(window, _windowContentScaleCallback);
                Glfw.SetMouseButtonCallback(window, _mouseButtonCallback);
                Glfw.SetCursorPositionCallback(window, _cursorPositionCallback);
                Glfw.SetCursorEnterCallback(window, _cursorEnterCallback);
                Glfw.SetScrollCallback(window, _scrollCallback);
                Glfw.SetKeyCallback(window, _keyCallback);
                Glfw.SetCharCallback(window, _charCallback);
                Glfw.SetCharModsCallback(window, _charModificationCallback);
                Glfw.SetDropCallback(window, _dropCallback);
                
                Raise(new EventWindowCreated((nint) window, commandCreateWindow.TaskCompletionSource));
                break;
        }
    }

    public nint WindowCreate()
    {
        return WindowCreate(new WindowCreateSettings());
    }
    
    public nint WindowCreate(WindowCreateSettings settings)
    {
        var task = WindowCreateAsync(settings);
        
        // Since we are blocking the event stream,
        // we need to process the events manually
        while (!task.IsCompleted)
        {
            // For future me, probably doesn't work due to calling in the wrong thread KEKW
            // Yes, it is
            // WaitEvents();
            
            ProcessCommands(single: true);
            
            _eventBridge.Wait();
            
            ProcessEvents(single: true);
        }

        return task.Result;
    }

    public Task<nint> WindowCreateAsync()
    {
        return WindowCreateAsync(new WindowCreateSettings());
    }

    // TODO: It's fucking shit don't update and broke evrything
    public Task<nint> WindowCreateAsync(WindowCreateSettings settings)
    {
        var taskCompletionSource = new TaskCompletionSource<nint>();
        Raise(new CommandCreateWindow(settings, taskCompletionSource));

        return taskCompletionSource.Task;
    }
    
    public void WindowSetTitle(nint window, string title)
    {
        Raise(new CommandWindowSetTitle(window, title));
    }

    private interface ICommand;
    
    private record struct CommandTerminate : ICommand;
    private record struct CommandWindowSetTitle(nint Window, string Title) : ICommand;
    private record struct CommandCreateWindow(WindowCreateSettings Settings, TaskCompletionSource<nint>? TaskCompletionSource = null) : ICommand;
}
