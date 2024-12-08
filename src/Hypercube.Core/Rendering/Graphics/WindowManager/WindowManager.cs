﻿using Hypercube.Core.Debugging.Logger;
using Hypercube.Core.Dependencies;
using Hypercube.Core.Rendering.Graphics.WindowApi;
using Hypercube.Core.Rendering.Graphics.WindowApi.GlfwApi;

namespace Hypercube.Core.Rendering.Graphics.WindowManager;

public class WindowManager : IWindowManager
{
    [Dependency] private readonly DependenciesContainer _container = default!;
    [Dependency] private readonly ILogger _logger = default!;

    private const ThreadPriority ThreadPriority = System.Threading.ThreadPriority.AboveNormal;
    private const int ThreadStackSize = 8 * 1024 * 1024;
    private const string ThreadName = "Windowing thread";
    private const int ThreadReadySleepDelay = 10;

    private readonly ManualResetEvent _readyEvent = new(false);

    private IWindowingApi _windowApi = default!;
    private Thread? _thread;

    public void Init(bool multiThread = false)
    {
        _windowApi = new GlfwWindowingApi();
        _container.Inject(_windowApi);
        
        if (!multiThread)
        {
            OnThread();
        }
        
        _logger.Debug("Windowing multi thread enabled");
        
        _thread = new Thread(OnThread, ThreadStackSize)
        {
            IsBackground = false,
            Priority = ThreadPriority,
            Name = ThreadName
        };

        _thread.Start();
        
        _logger.Debug("Suspend main thread");
        
        _readyEvent.WaitOne();
        
        _logger.Debug("Unsuspend main thread");
    }

    public void WindowCreate()
    {
        _windowApi.WindowCreate();
    }

    public void Terminate()
    {
        _windowApi.Terminate();
    }

    public void PollEvents()
    {
        _windowApi.PollEvents();
    }

    private void OnThread()
    {
        _logger.Debug($"Started Window Thread, priority: {ThreadPriority}, Stack size: {ThreadStackSize}, Name: {ThreadName}, Ready sleep delay: {ThreadReadySleepDelay}");
        _logger.Debug("Initialization window api...");
        
        _windowApi.Init(multiThread: true);
        
        while (!_windowApi.Ready)
        {
            Thread.Sleep(ThreadReadySleepDelay);
        }
        
        _logger.Debug("Initialized window api");
        
        _readyEvent.Set();
        _windowApi.EnterLoop();
    }
}