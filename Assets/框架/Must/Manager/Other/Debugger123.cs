using UnityEngine;
using System.Collections.Generic;
using System;
using Unity.Profiling;
using UnityEngine.Profiling;

/// <summary>
/// Unity 微型调试器 Debugger
/// https://blog.csdn.net/qq992817263/article/details/78771766
/// https://docs.unity3d.com/2020.2/Documentation/Manual/ProfilerWindow.html
/// </summary>
public class Debugger123 : MonoBehaviour
{
    //首配置
    [Header("是否允许调试")] public bool AllowDebugging = true;
    private DebugType _debugType = DebugType.Console;
    private Rect _windowRect = new Rect(0, 0, 100, 60);

    //日志
    private List<LogData> _logInformations = new List<LogData>();
    private int _currentLogIndex = -1;
    private int _infoLogCount = 0;
    private int _warningLogCount = 0;
    private int _errorLogCount = 0;
    private int _fatalLogCount = 0;
    private bool _showInfoLog = true;
    private bool _showWarningLog = true;
    private bool _showErrorLog = true;
    private bool _showFatalLog = true;
    private Vector2 _scrollLogView = Vector2.zero;
    private Vector2 _scrollCurrentLogView = Vector2.zero;
    private Vector2 _scrollSystemView = Vector2.zero;
    private bool _expansion = false;

    //fps
    private float _fps = 0;
    private float deltaTime;
    private float fpsTimer;
    [Header("FPS间隔")] public float fpsUpdateInterval = 0.5f;
    private Color _fpsColor = Color.white;

    //RenderingProfilerModule
    private ProfilerRecorder setPassCallsRecorder;
    private ProfilerRecorder drawCallsRecorder;
    private ProfilerRecorder verticesRecorder;
    private ProfilerRecorder totalBatchesRecorder;
    
    
    private void OnEnable()
    {
        setPassCallsRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "SetPass Calls Count");
        drawCallsRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Draw Calls Count");
        verticesRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Vertices Count");
        totalBatchesRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Total Batches Count");
    }
    private void Start()
    {
        if (AllowDebugging)
            Application.logMessageReceived += LogHandler;
    }
    private void Update()
    {
        if (AllowDebugging)
            FPSCalculate();
    }
    private void OnDisable()
    {
        setPassCallsRecorder.Dispose();
        drawCallsRecorder.Dispose();
        verticesRecorder.Dispose();
        totalBatchesRecorder.Dispose();
    }
    private void OnDestroy()
    {
        if (AllowDebugging)
            Application.logMessageReceived -= LogHandler;
    }


    private void OnGUI()
    {
        if (AllowDebugging)
        {
            _windowRect = _expansion ? GUI.Window(0, _windowRect, ExpansionGUIWindow, "DEBUGGER") : GUI.Window(0, _windowRect, ShrinkGUIWindow, "DEBUGGER");
        }
    }
    private void ShrinkGUIWindow(int windowId)
    {
        GUI.DragWindow(new Rect(0, 0, 10000, 20));

        GUI.contentColor = _fpsColor;
        if (GUILayout.Button($"FPS: <color=#00FF00>{_fps:F1}</color>", GUILayout.Width(80), GUILayout.Height(30)))
        {
            int length = Enum.GetValues(typeof(DebugType)).Length;
            _expansion = true;
            _windowRect.width = 600; //length * 85; //
            _windowRect.height = 360;
        }

        GUI.contentColor = Color.white;
    }
    private void ExpansionGUIWindow(int windowId)
    {
        GUI.DragWindow(new Rect(0, 0, 10000, 20));

        #region title

        GUILayout.BeginHorizontal();
        GUI.contentColor = _fpsColor;
        if (GUILayout.Button($"FPS: <color=#00FF00>{_fps:F1}</color>", GUILayout.Height(30)))
        {
            _expansion = false;
            _windowRect.width = 100;
            _windowRect.height = 60;
        }
        GUI.contentColor = (_debugType == DebugType.Console ? Color.white : Color.gray);
        if (GUILayout.Button(DebugType.Console.ToString(), GUILayout.Height(30))) _debugType = DebugType.Console;
        GUI.contentColor = (_debugType == DebugType.Memory ? Color.white : Color.gray);
        if (GUILayout.Button(DebugType.Memory.ToString() , GUILayout.Height(30))) _debugType = DebugType.Memory;
        GUI.contentColor = (_debugType == DebugType.System ? Color.white : Color.gray);
        if (GUILayout.Button(DebugType.System.ToString(), GUILayout.Height(30)))_debugType = DebugType.System;
        GUI.contentColor = (_debugType == DebugType.Screen ? Color.white : Color.gray);
        if (GUILayout.Button(DebugType.Screen.ToString(), GUILayout.Height(30)))_debugType = DebugType.Screen;
        GUI.contentColor = (_debugType == DebugType.Quality ? Color.white : Color.gray);
        if (GUILayout.Button(DebugType.Quality.ToString(), GUILayout.Height(30)))_debugType = DebugType.Quality;
        GUI.contentColor = (_debugType == DebugType.Environment ? Color.white : Color.gray);
        if (GUILayout.Button(DebugType.Environment.ToString(), GUILayout.Height(30)))_debugType = DebugType.Environment;
        GUI.contentColor = (_debugType == DebugType.Rendering ? Color.white : Color.gray);
        if (GUILayout.Button(DebugType.Rendering.ToString(), GUILayout.Height(30)))_debugType = DebugType.Rendering;
        GUI.contentColor = Color.white;
        GUILayout.EndHorizontal();

        #endregion

        switch (_debugType)
        {
            case DebugType.Console:
            {
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Clear"))
                {
                    _logInformations.Clear();
                    _fatalLogCount = 0;
                    _warningLogCount = 0;
                    _errorLogCount = 0;
                    _infoLogCount = 0;
                    _currentLogIndex = -1;
                    _fpsColor = Color.white;
                }

                GUI.contentColor = (_showInfoLog ? Color.white : Color.gray);
                _showInfoLog = GUILayout.Toggle(_showInfoLog, "Info [" + _infoLogCount + "]");
                GUI.contentColor = (_showWarningLog ? Color.white : Color.gray);
                _showWarningLog = GUILayout.Toggle(_showWarningLog, "Warning [" + _warningLogCount + "]");
                GUI.contentColor = (_showErrorLog ? Color.white : Color.gray);
                _showErrorLog = GUILayout.Toggle(_showErrorLog, "Error [" + _errorLogCount + "]");
                GUI.contentColor = (_showFatalLog ? Color.white : Color.gray);
                _showFatalLog = GUILayout.Toggle(_showFatalLog, "Fatal [" + _fatalLogCount + "]");
                GUI.contentColor = Color.white;
                GUILayout.EndHorizontal();

                _scrollLogView = GUILayout.BeginScrollView(_scrollLogView, "Box", GUILayout.Height(165));
                for (int i = 0; i < _logInformations.Count; i++)
                {
                    bool show = false;
                    Color color = Color.white;
                    switch (_logInformations[i].type)
                    {
                        case "Fatal":
                            show = _showFatalLog;
                            color = Color.red;
                            break;
                        case "Error":
                            show = _showErrorLog;
                            color = Color.red;
                            break;
                        case "Info":
                            show = _showInfoLog;
                            color = Color.white;
                            break;
                        case "Warning":
                            show = _showWarningLog;
                            color = Color.yellow;
                            break;
                        default: break;
                    }

                    if (show)
                    {
                        GUILayout.BeginHorizontal();
                        if (GUILayout.Toggle(_currentLogIndex == i, ""))
                        {
                            _currentLogIndex = i;
                        }

                        GUI.contentColor = color;
                        GUILayout.Label("[" + _logInformations[i].type + "] ");
                        GUILayout.Label("[" + _logInformations[i].time + "] ");
                        GUILayout.Label(_logInformations[i].message);
                        GUILayout.FlexibleSpace();
                        GUI.contentColor = Color.white;
                        GUILayout.EndHorizontal();
                    }
                }

                GUILayout.EndScrollView();

                _scrollCurrentLogView = GUILayout.BeginScrollView(_scrollCurrentLogView, "Box", GUILayout.Height(100));
                if (_currentLogIndex != -1)
                {
                    GUILayout.Label(_logInformations[_currentLogIndex].message + "\r\n\r\n" + _logInformations[_currentLogIndex].stackTrace);
                }

                GUILayout.EndScrollView();

                // if (_currentLogIndex != -1)
                // {
                //     if (GUILayout.Button("Copy选中的日志"))
                //         Copy(_logInformations[_currentLogIndex].message + "\r\n\r\n" + _logInformations[_currentLogIndex].stackTrace);
                // }
                break;
            }
            case DebugType.Memory:
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label("Memory Information");
                GUILayout.EndHorizontal();

                GUILayout.BeginVertical("Box");
#if UNITY_5
            GUILayout.Label("总内存：" + Profiler.GetTotalReservedMemory() / 1000000 + "MB");
            GUILayout.Label("已占用内存：" + Profiler.GetTotalAllocatedMemory() / 1000000 + "MB");
            GUILayout.Label("空闲中内存：" + Profiler.GetTotalUnusedReservedMemory() / 1000000 + "MB");
            GUILayout.Label("总Mono堆内存：" + Profiler.GetMonoHeapSize() / 1000000 + "MB");
            GUILayout.Label("已占用Mono堆内存：" + Profiler.GetMonoUsedSize() / 1000000 + "MB");
#endif
#if UNITY_7
            GUILayout.Label("总内存：" + Profiler.GetTotalReservedMemoryLong() / 1000000 + "MB");
            GUILayout.Label("已占用内存：" + Profiler.GetTotalAllocatedMemoryLong() / 1000000 + "MB");
            GUILayout.Label("空闲中内存：" + Profiler.GetTotalUnusedReservedMemoryLong() / 1000000 + "MB");
            GUILayout.Label("总Mono堆内存：" + Profiler.GetMonoHeapSizeLong() / 1000000 + "MB");
            GUILayout.Label("已占用Mono堆内存：" + Profiler.GetMonoUsedSizeLong() / 1000000 + "MB");
#endif
                GUILayout.EndVertical();

                GUILayout.BeginHorizontal();
                if (GUILayout.Button("卸载未使用的资源"))
                {
                    Resources.UnloadUnusedAssets();
                }

                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                if (GUILayout.Button("使用GC垃圾回收"))
                {
                    GC.Collect();
                }

                GUILayout.EndHorizontal();
                break;
            }
            case DebugType.System:
                GUILayout.BeginHorizontal();
                GUILayout.Label("System Information");
                GUILayout.EndHorizontal();

                _scrollSystemView = GUILayout.BeginScrollView(_scrollSystemView, "Box");
                GUILayout.Label("操作系统：" + SystemInfo.operatingSystem);
                GUILayout.Label("系统内存：" + SystemInfo.systemMemorySize + "MB");
                GUILayout.Label("处理器：" + SystemInfo.processorType);
                GUILayout.Label("处理器数量：" + SystemInfo.processorCount);
                GUILayout.Label("显卡：" + SystemInfo.graphicsDeviceName);
                GUILayout.Label("显卡类型：" + SystemInfo.graphicsDeviceType);
                GUILayout.Label("显存：" + SystemInfo.graphicsMemorySize + "MB");
                GUILayout.Label("显卡标识：" + SystemInfo.graphicsDeviceID);
                GUILayout.Label("显卡供应商：" + SystemInfo.graphicsDeviceVendor);
                GUILayout.Label("显卡供应商标识码：" + SystemInfo.graphicsDeviceVendorID);
                GUILayout.Label("设备模式：" + SystemInfo.deviceModel);
                GUILayout.Label("设备名称：" + SystemInfo.deviceName);
                GUILayout.Label("设备类型：" + SystemInfo.deviceType);
                GUILayout.Label("设备标识：" + SystemInfo.deviceUniqueIdentifier);
                GUILayout.EndScrollView();
                break;
            case DebugType.Screen:
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label("Screen Information");
                GUILayout.EndHorizontal();

                GUILayout.BeginVertical("Box");
                GUILayout.Label("DPI：" + Screen.dpi);
                GUILayout.Label("分辨率：" + Screen.currentResolution.ToString());
                GUILayout.EndVertical();

                GUILayout.BeginHorizontal();
                if (GUILayout.Button("全屏"))
                {
                    Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, !Screen.fullScreen);
                }

                GUILayout.EndHorizontal();
                break;
            }
            case DebugType.Quality:
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label("Quality Information");
                GUILayout.EndHorizontal();

                GUILayout.BeginVertical("Box");
                string value = "";
                if (QualitySettings.GetQualityLevel() == 0)
                {
                    value = " [最低]";
                }
                else if (QualitySettings.GetQualityLevel() == QualitySettings.names.Length - 1)
                {
                    value = " [最高]";
                }

                GUILayout.Label("图形质量：" + QualitySettings.names[QualitySettings.GetQualityLevel()] + value);
                GUILayout.EndVertical();

                GUILayout.BeginHorizontal();
                if (GUILayout.Button("降低一级图形质量"))
                {
                    QualitySettings.DecreaseLevel();
                }

                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                if (GUILayout.Button("提升一级图形质量"))
                {
                    QualitySettings.IncreaseLevel();
                }

                GUILayout.EndHorizontal();
                break;
            }
            case DebugType.Environment:
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label("Environment Information");
                GUILayout.EndHorizontal();

                GUILayout.BeginVertical("Box");
                GUILayout.Label($"项目名称：{Application.productName}");
#if UNITY_5
            GUILayout.Label("项目ID：" + Application.bundleIdentifier);
#endif
#if UNITY_7
            GUILayout.Label("项目ID：" + Application.identifier);
#endif
                GUILayout.Label($"项目版本：{Application.version}");
                GUILayout.Label($"Unity版本：{Application.unityVersion}");
                GUILayout.Label($"公司名称：{Application.companyName}");
                GUILayout.EndVertical();

                GUILayout.BeginHorizontal();
                if (GUILayout.Button("退出程序"))Application.Quit();
                GUILayout.EndHorizontal();
                break;
            }
            case DebugType.Rendering:
                GUILayout.BeginVertical("Box");
                if (setPassCallsRecorder.Valid)
                    GUILayout.Label($"一帧中切换用于渲染游戏对象的着色器通道的次数SetPass Calls: {setPassCallsRecorder.LastValue}");
                if (drawCallsRecorder.Valid)
                    GUILayout.Label($"绘制调用 Draw Calls: {drawCallsRecorder.LastValue}");
                if (verticesRecorder.Valid)
                    GUILayout.Label($"一帧内处理的顶点数Vertices: {verticesRecorder.LastValue}");
                if (totalBatchesRecorder.Valid)
                    GUILayout.Label($"一帧内处理的批次总数。这个数字包括静态和动态批次Total Batches: {totalBatchesRecorder.LastValue}");
                GUILayout.EndVertical();
                break;
        }
    }


    //功能
    private void LogHandler(string condition, string stackTrace, LogType type)
    {
        LogData log = new LogData
        {
            time = DateTime.Now.ToString("HH:mm:ss"),
            message = condition,
            stackTrace = stackTrace
        };

        switch (type)
        {
            case LogType.Assert:
                log.type = "Fatal";
                _fatalLogCount += 1;
                break;
            case LogType.Exception:
            case LogType.Error:
                log.type = "Error";
                _errorLogCount += 1;
                break;
            case LogType.Warning:
                log.type = "Warning";
                _warningLogCount += 1;
                break;
            case LogType.Log:
                log.type = "Info";
                _infoLogCount += 1;
                break;
        }

        _logInformations.Add(log);

        if (_warningLogCount > 0)
        {
            _fpsColor = Color.yellow;
        }

        if (_errorLogCount > 0)
        {
            _fpsColor = Color.red;
        }
    }
    private void FPSCalculate()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        fpsTimer += Time.unscaledDeltaTime;
        if (fpsTimer >= fpsUpdateInterval)
        {
            _fps = 1.0f / deltaTime;
            fpsTimer = 0f;
        }
    }
    private string GetColor(string str, string colorCode = "#00FF00")
    {
        return $"<color={colorCode}>str</color>";
    }
    public void Copy(string str)
    {
        TextEditor te = new() { text = str };
        te.SelectAll();
        te.Copy();
    }
}

public struct LogData
{
    public string time;
    public string type;
    public string message;
    public string stackTrace;
}

public enum DebugType
{
    Console,
    Memory,
    System,
    Screen,
    Quality,
    Environment,
    Rendering,
}