using System.Diagnostics;

namespace Enigma_Framework.Core.LogSystem;

public static class Logger
{
    private static readonly int MainThreadId;
    public static bool IsActive = true;
    public static bool ShowLog = true;
    public static bool ShowWarning = true;
    public static bool ShowError = true;

    static Logger()
    {
        MainThreadId = Thread.CurrentThread.ManagedThreadId;
        AppDomain.CurrentDomain.UnhandledException += HandleUnhandledException;
        TaskScheduler.UnobservedTaskException += HandleUnobservedTaskException;
    }

    private static void HandleUnobservedTaskException(object? sender,
        UnobservedTaskExceptionEventArgs e)
    {
        LogError($"UnobservedTaskException: {e.Exception}");
    }

    private static void HandleUnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        if (e.ExceptionObject is Exception ex) LogError($"UnhandledException: {ex}");
    }

    private static string GetCallerInfo(int skipFrames = 2)
    {
        var stackTrace = new StackTrace();
        var frame = stackTrace.GetFrame(skipFrames);
        var method = frame?.GetMethod();
        var className = method?.DeclaringType?.Name ?? "UnknownClass";
        var methodName = method?.Name ?? "UnknownMethod";

        return $"{className}.{methodName}";
    }

    private static string GetThreadInfo()
    {
        var currentThreadId = Thread.CurrentThread.ManagedThreadId;
        var threadType = currentThreadId == MainThreadId ? "MainThread" : $"WorkerThread({currentThreadId})";
        return threadType;
    }

    public static void Log(string info)
    {
        if (!ShowLog) return;

        LogInternal("INFO", info);
    }

    public static void LogWarning(string info)
    {
        if (!ShowWarning) return;

        LogInternal("WARNING", info, ConsoleColor.Yellow);
    }

    public static void LogError(string info)
    {
        if (!ShowError) return;

        LogInternal("ERROR", info, ConsoleColor.Red);
    }

    public static void LogError(Exception ex)
    {
        if (!ShowError) return;

        LogInternal("EXCEPTION", ex.ToString(), ConsoleColor.Red);
    }

    private static void LogInternal(string level, string message, ConsoleColor color = ConsoleColor.White)
    {
        var caller = GetCallerInfo();
        var threadInfo = GetThreadInfo();
        var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
        Console.ForegroundColor = color;
        Console.WriteLine($"[{timestamp}] [{level}] [{threadInfo}] [{caller}] {message}");
        Console.ResetColor();
    }
}