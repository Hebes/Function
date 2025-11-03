using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;

/// <summary>
/// 日志系统
/// [Conditional("BUGOPEN")] //日志 Project->Player->Other Settings->Script Define Symbols
/// </summary>
public class DebugManager
{
    private const string LOGLock = "LogLock"; //日志锁
    private StreamWriter _logFiLeWriter = null; //输出流,整个日志文件
    private readonly StringBuilder _sb = new StringBuilder();
    private LogSetting LogSetting { get; set; }
    private ILogger _logger = null; //日志接口


    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="logSetting">日志设置</param>
    /// <param name="log">log委托</param>
    /// <param name="warn">warn委托</param>
    /// <param name="error">error委托</param>
    /// <param name="colorAct">DebugManager的ColorUnityLog或者WriteConsoleLog</param>
    public void Init(LogSetting logSetting, Action<string> log, Action<string> warn, Action<string> error)
    {
        //日志接口
        LogSetting = logSetting;
        _logger = logSetting.LoggerType switch
        {
            LoggerType.Unity => new UnityDebug(),
            LoggerType.Console => new ConsoleDebug(),
            _ => _logger
        };
        _logger.LogAct = log;
        _logger.WarnAct = warn;
        _logger.ErrorAct = error;


        if (!logSetting.EnableSave) return; //是否启用日志保存
        var path = $"{logSetting.SavePath}{DateTime.Now:yyyyMMdd@HH-mm-s}{logSetting.SaveName}";
        log($"日志输出路径：{path}");
        if (!Directory.Exists(logSetting.SavePath))
            Directory.CreateDirectory(logSetting.SavePath);
        try
        {
            _logFiLeWriter = File.AppendText(path);
            _logFiLeWriter.AutoFlush = true;
        }
        catch (Exception)
        {
            _logFiLeWriter = null;
        }
    }

    
    public void Log(string msg, LogCoLor logCoLor, params object[] args)
    {
        lock (LOGLock)
        {
            if (LogSetting.EnableLog == false) return;
            msg = DecorateLog(string.Format(msg, args));
            _logger.Log(msg, logCoLor);
            WriteToFile(string.Format($"[L]{msg}"));
        }
    }
    public void Trace(string msg, params object[] args)
    {
        lock (LOGLock)
        {
            if (LogSetting.EnableLog == false) return;
            msg = DecorateLog(string.Format(msg, args), true);
            _logger.Log(msg, LogCoLor.Magenta);
            if (LogSetting.EnableSave)
                WriteToFile(string.Format($"[T]{msg}"));
        }
    }
    public void Warn(string msg, params object[] args)
    {
        lock (LOGLock)
        {
            if (LogSetting.EnableLog == false) return;
            msg = DecorateLog(string.Format(msg, args));
            _logger.Warn(msg);
            WriteToFile(string.Format($"[W]{msg}"));
        }
    }
    public void Error(string msg, params object[] args)
    {
        lock (LOGLock)
        {
            if (LogSetting.EnableLog == false) return;
            msg = DecorateLog(string.Format(msg, args), true);
            _logger.Error(msg);
            WriteToFile(string.Format($"[E]{msg}"));
        }
    }

    
    //************************************工具************************************
    /// <summary>
    /// 路径
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public string GetPath(uint id = 0)
    {
        switch (id)
        {
            case 0: //unity persistentDataPath移动端唯一可读可写的路径
                Type type = Type.GetType("UnityEngine.Application, UnityEngine");
                return type.GetProperty("persistentDataPath").GetValue(null).ToString() + "/Logs/";
            case 1: //控制台 AppDomain.CurrentDomain.BaseDirectory，获取基目录，基目录：指应用程序所在的目录
                return string.Format($"{AppDomain.CurrentDomain.BaseDirectory}Logs\\");
            default:
                throw new Exception("错误的ID");
        }
    }
    /// <summary>
    /// 日志打印
    /// </summary>
    /// <param name="msg"></param>
    /// <param name="isTrace"></param>
    /// <returns></returns>
    private string DecorateLog(string msg, bool isTrace = false)
    {
        _sb.Clear();
        _sb.Capacity = 100;
        lock (LOGLock)
        {
            _sb.Append(LogSetting.LogPrefix);
            // if (LogSetting.EnableTime) //启用时间
            //   _sb.AppendFormat($"时间:{DateTime.Now:hh:mm:ss--fff}");
            if (LogSetting.EnableMillisecond)
                _sb.AppendFormat($"时间{DateTime.Now.TimeOfDay}");
            if (LogSetting.EnableThreadID) //启用线程
                _sb.AppendFormat($"{GetThreadID()}");
            _sb.AppendFormat($" {LogSetting.LogSeparate} {msg}"); //日志分离
            if (isTrace) //是否追踪日志 堆栈
                _sb.AppendFormat($" \nStackTrace:{GetLogTrace()}");
            return _sb.ToString();
        }
    }
    /// <summary>
    /// 获取日志追踪,上下文信息
    /// </summary>
    /// <returns></returns>
    private string GetLogTrace()
    {
        var st = new StackTrace(3, true); //3 跳跃3帧 true-> 获取场下文信息
        var traceInfo = string.Empty;
        for (var i = 0; i < st.FrameCount; i++)
        {
            var sf = st.GetFrame(i);
            //traceInfo += string.Format($"\n\t{sf.GetFileName()}::{sf.GetMethod()}line:{sf.GetFileLineNumber()}");
            //traceInfo += string.Format($"\n\t{sf.GetFileName()}::\n\t{sf.GetMethod()}\tline:{sf.GetFileLineNumber()}");
            traceInfo += string.Format($"\n\t脚本:{sf.GetFileName()}::方法{sf.GetMethod()}行: {sf.GetFileLineNumber()}");
        }

        return traceInfo;
    }
    /// <summary>
    /// 获取线程ID
    /// </summary>
    /// <returns></returns>
    private object GetThreadID()
    {
        return string.Format($" 线程ID:{Thread.CurrentThread.ManagedThreadId}"); //ThreadID
    }
    /// <summary>
    /// 写入文件
    /// </summary>
    /// <param name="msg"></param>
    private void WriteToFile(string msg)
    {
        if (!LogSetting.EnableSave) return;
        if (_logFiLeWriter == null) return;
        try
        {
            _logFiLeWriter.WriteLine(msg);
        }
        catch
        {
            _logFiLeWriter = null;
        }
    }
    /// <summary>
    /// 打印数组数据For Debug
    /// </summary>
    /// <param name="bytes"></param>
    /// <param name="prefix"></param>
    /// <param name="printer"></param>
    public void PrintBytesArray(byte[] bytes, string prefix, Action<string> printer = null)
    {
        var str = prefix + "->\n";
        for (int i = 0; i < bytes.Length; i++)
        {
            if (i % 10 == 0)
                str += bytes[i] + "\n";

            str += bytes[i] + " ";
        }

        if (printer != null)
            printer(str);
        else
            Log(str,LogCoLor.None);
    }
}

/// <summary>
/// 日志设置
/// </summary>
public class LogSetting
{
    public bool EnableLog = true; //启用日志
    public string LogPrefix = "#"; //日志前缀
    public bool EnableTime = true; //是否启用时间
    public bool EnableMillisecond = true; //是否启用毫秒级调用(可以定位代码的调用顺序)
    public string LogSeparate = ">>"; //日志分离
    public bool EnableThreadID = true; //启用线程ID
    public bool EnableTrace = true; //启用跟踪
    public bool EnableSave = true; //启用保存
    public LoggerType LoggerType = LoggerType.Unity; //日志类型
    public string SavePath; //保存路径
    public string SaveName; //保存名称
}

/// <summary>
/// 日志颜色
/// </summary>
public enum LogCoLor
{
    /// <summary> 空 </summary>
    None,

    /// <summary> 深红 </summary>
    DarkRed,

    /// <summary> 绿色 </summary>
    Green,

    /// <summary> 蓝色 </summary>
    Blue,

    /// <summary> 青色 </summary>
    Cyan,

    /// <summary> 紫色 </summary>
    Magenta,

    /// <summary> 深黄 </summary>
    DarkYellow,
}

public enum LoggerType
{
    /// <summary> Unity编辑器 </summary>
    Unity,

    /// <summary> 服务器 </summary>
    Console,
}

public interface ILogger
{
    public Action<string> LogAct { get; set; }
    public Action<string> WarnAct { get; set; }
    public Action<string> ErrorAct { get; set; }
    public void Log(string msg, LogCoLor logCoLor = LogCoLor.None);
    public void Warn(string msg);
    public void Error(string msg);
}

public class UnityDebug : ILogger
{
    public Action<string> LogAct { get; set; }
    public Action<string> WarnAct { get; set; }
    public Action<string> ErrorAct { get; set; }

    public void Log(string msg, LogCoLor logCoLor = LogCoLor.None) => ColorUnityLog(msg, logCoLor);
    public void Warn(string msg) => WarnAct.Invoke(msg);
    public void Error(string msg) => ErrorAct.Invoke(msg);

    private void ColorUnityLog(string msg, LogCoLor logCoLor = LogCoLor.None)
    {
        switch (logCoLor)
        {
            default:
            case LogCoLor.None:
                msg = string.Format($"<coLor=#FF000O>{msg}</coLor>");
                break;
            case LogCoLor.DarkRed:
                msg = string.Format($"<coLor=#FF000O>{msg}</coLor>");
                break;
            case LogCoLor.Green:
                msg = string.Format($"<coLor=#00FF00>{msg}</coLor>");
                break;
            case LogCoLor.Blue:
                msg = string.Format($"<coLor=#0000FF>{msg}</coLor>");
                break;
            case LogCoLor.Cyan:
                msg = string.Format($"<coLor=#00FFFF>{msg}</coLor>");
                break;
            case LogCoLor.Magenta:
                msg = string.Format($"<coLor=#FF00FF>{msg}</coLor>");
                break;
            case LogCoLor.DarkYellow:
                msg = string.Format($"<coLor=#FFff0O>{msg}</coLor>");
                break;
        }

        LogAct.Invoke(msg);
    }
}

public class ConsoleDebug : ILogger
{
    public Action<string> LogAct { get; set; }
    public Action<string> WarnAct { get; set; }
    public Action<string> ErrorAct { get; set; }

    public void Log(string msg, LogCoLor logCoLor = LogCoLor.None) => WriteConsoleLog(msg, logCoLor);
    public void Warn(string msg) => Log(msg, LogCoLor.DarkYellow);
    public void Error(string msg) => Log(msg, LogCoLor.DarkRed);

    private void WriteConsoleLog(string msg, LogCoLor logCoLor = LogCoLor.None)
    {
        Console.ForegroundColor = default;
        switch (logCoLor)
        {
            default:
            case LogCoLor.None:
                Console.ForegroundColor = ConsoleColor.White;
                break;
            case LogCoLor.DarkRed:
                Console.ForegroundColor = ConsoleColor.DarkRed;
                break;
            case LogCoLor.Green:
                Console.ForegroundColor = ConsoleColor.Green;
                break;
            case LogCoLor.Blue:
                Console.ForegroundColor = ConsoleColor.Blue;
                break;
            case LogCoLor.Cyan:
                Console.ForegroundColor = ConsoleColor.Cyan;
                break;
            case LogCoLor.Magenta:
                Console.ForegroundColor = ConsoleColor.Magenta;
                break;
            case LogCoLor.DarkYellow:
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                break;
        }

        Console.WriteLine(msg);
        Console.ForegroundColor = ConsoleColor.Gray;
    }
}