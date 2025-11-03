using UnityEngine;
using System; 
using System.Diagnostics;
using Debug = UnityEngine.Debug;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Unity 性能监控 + 内存趋势曲线 + 安全等级指示 + 一键GC按钮
/// 快捷键：F9 显示/隐藏
/// https://mp.weixin.qq.com/s/2DUoxBCOPT-0MeC5k6kAPQ
/// </summary>
public class GCMonitorPro : MonoBehaviour
{
    private static GCMonitorPro instance;
    public static GCMonitorPro Instance
    {
        get
        {
            if (instance == null)
            {
                var go = new GameObject("[GCMonitorPro]");
                instance = go.AddComponent<GCMonitorPro>();
                DontDestroyOnLoad(go);
            }
            return instance;
        }
    }

    public bool showOnScreen = true;

    private float deltaTime;
    private float fps;
    private float fpsTimer;
    private float fpsUpdateInterval = 0.5f;

    private int lastGCCount;
    private float lastGCTime;
    private Stopwatch stopwatch = new Stopwatch();
    private float nextUpdateTime;
    private float updateInterval = 0.5f;

    // 内存监控
    private long lastMemory;
    private float memoryWarningThreshold = 800f;

    // 泄漏检测
    private const int trendSampleCount = 10;
    private float[] memorySamples = new float[trendSampleCount];
    private int sampleIndex = 0;
    private bool leakSuspected = false;

    // 曲线图数据
    private const int maxGraphPoints = 120; // 最近120次采样（约60秒）
    private float[] memoryHistory = new float[maxGraphPoints];
    private int historyIndex = 0;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
        stopwatch.Start();

        lastMemory = GC.GetTotalMemory(false);
        lastGCCount = GC.CollectionCount(0);
    }

    private void Update()
    {
        // FPS
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        fpsTimer += Time.unscaledDeltaTime;
        if (fpsTimer >= fpsUpdateInterval)
        {
            fps = 1.0f / deltaTime;
            fpsTimer = 0f;
        }

        if (Time.time >= nextUpdateTime)
        {
            nextUpdateTime = Time.time + updateInterval;
            UpdateMemoryStatus();
        }
    }

    private void UpdateMemoryStatus()
    {
        long currentMemory = GC.GetTotalMemory(false);
        int currentGCCount = GC.CollectionCount(0);

        // GC触发检测
        if (currentGCCount > lastGCCount)
        {
            float gcInterval = stopwatch.ElapsedMilliseconds / 1000f - lastGCTime;
            lastGCTime = stopwatch.ElapsedMilliseconds / 1000f;
            Debug.Log($"<color=orange>[GC]</color> GC触发 | 间隔: {gcInterval:F2}s | 总次数: {currentGCCount}");
            lastGCCount = currentGCCount;
        }

        // 泄漏趋势检测
        float memoryMB = currentMemory / (1024f * 1024f);
        memorySamples[sampleIndex] = memoryMB;
        sampleIndex = (sampleIndex + 1) % trendSampleCount;

        if (sampleIndex == 0)
        {
            leakSuspected = CheckLeakTrend();
            if (leakSuspected)
                Debug.LogWarning("<color=#FF5555>[GCMonitorPro]</color> ⚠ 检测到内存持续上升，可能存在泄漏！");
        }

        // 添加到历史数据
        memoryHistory[historyIndex] = memoryMB;
        historyIndex = (historyIndex + 1) % maxGraphPoints;

        lastMemory = currentMemory;
    }

    private bool CheckLeakTrend()
    {
        float first = memorySamples[0];
        float last = memorySamples[trendSampleCount - 1];
        if (last - first > 50f)
        {
            int upCount = 0;
            for (int i = 1; i < trendSampleCount; i++)
                if (memorySamples[i] > memorySamples[i - 1]) upCount++;
            return upCount > trendSampleCount * 0.7f;
        }
        return false;
    }

#if UNITY_EDITOR
    [MenuItem("Tools/Performance/Toggle GC Monitor (F9) _F9")]
    private static void ToggleGCMonitor()
    {
        if (Instance.showOnScreen)
        {
            Instance.showOnScreen = false;
            Debug.Log("<color=gray>[GCMonitorPro] 隐藏监控面板</color>");
        }
        else
        {
            Instance.showOnScreen = true;
            Debug.Log("<color=green>[GCMonitorPro] 显示监控面板</color>");
        }
    }
#endif

    private void OnGUI()
    {
        if (!showOnScreen) return;

        float memoryMB = GC.GetTotalMemory(false) / (1024f * 1024f);
        int gcCount = GC.CollectionCount(0);
        long totalMem = UnityEngine.Profiling.Profiler.GetTotalAllocatedMemoryLong();
        bool incrementalGC = UnityEngine.Scripting.GarbageCollector.isIncremental;

        // 🔸计算内存安全等级
        string safetyLevel = GetMemorySafetyLevel(memoryMB, out string colorTag);

        GUIStyle labelStyle = new GUIStyle(GUI.skin.label)
        {
            richText = true,
            fontSize = 28
        };

        GUILayout.BeginArea(new Rect(0, 0, 360, 400), GUI.skin.box);
        GUILayout.Label("<b><color=#00FFFF>🧠 GCMonitor Pro</color></b>", labelStyle);
        GUILayout.Label($"FPS: <color={(fps >= 50 ? "#00FF00" : fps >= 30 ? "#FFFF00" : "#FF0000")}>{fps:F1}</color>", labelStyle);
        GUILayout.Label($"GC次数: <color=orange>{gcCount}</color>", labelStyle);
        GUILayout.Label($"托管内存: <color=white>{memoryMB:F1} MB</color>", labelStyle);
        GUILayout.Label($"总内存: <color=white>{(totalMem / (1024f * 1024f)):F1} MB</color>", labelStyle);
        GUILayout.Label($"增量GC: {(incrementalGC ? "<color=#00FF00>开启</color>" : "<color=#FF0000>关闭</color>")}", labelStyle);

        GUILayout.Space(5);
        GUILayout.Label($"内存安全等级: <b><color={colorTag}>{safetyLevel}</color></b>", labelStyle);

        if (leakSuspected)
            GUILayout.Label("<b><color=#FF5555>⚠ 疑似内存泄漏趋势！</color></b>", labelStyle);

        GUILayout.Space(8);

        // 🔘 Force GC 按钮
        GUI.backgroundColor = Color.Lerp(Color.white, Color.green, 0.3f);
        if (GUILayout.Button("💥 Force GC (手动回收)", GUILayout.Height(28)))
            ForceGC();

        GUILayout.EndArea();

        // 🔹绘制内存曲线
        DrawMemoryGraph(new Rect(0, 420, 360, 120));
    }

    private void ForceGC()
    {
        long before = GC.GetTotalMemory(false);
        GC.Collect();
        GC.WaitForPendingFinalizers();
        long after = GC.GetTotalMemory(true);

        float diff = (before - after) / (1024f * 1024f);
        Debug.Log($"<color=#88FF88>[GCMonitorPro]</color> 手动GC完成，释放约 <b>{diff:F2} MB</b>");
        UpdateMemoryStatus();
    }

    /// <summary>
    /// 绘制内存趋势曲线
    /// </summary>
    private void DrawMemoryGraph(Rect rect)
    {
        GUI.Box(rect, "内存趋势 (MB)");

        float maxMemory = 1200f; // 设定曲线图上限
        float stepX = rect.width / (float)maxGraphPoints;
        float scaleY = rect.height / maxMemory;

        Vector2 prev = Vector2.zero;
        for (int i = 0; i < maxGraphPoints; i++)
        {
            int index = (historyIndex + i) % maxGraphPoints;
            float mem = memoryHistory[index];
            float x = rect.x + i * stepX;
            float y = rect.yMax - mem * scaleY;

            if (i > 0)
                DrawLine(prev, new Vector2(x, y), Color.green, 2f);

            prev = new Vector2(x, y);
        }
    }

    private void DrawLine(Vector2 p1, Vector2 p2, Color color, float width)
    {
        Color oldColor = GUI.color;
        Matrix4x4 matrix = GUI.matrix;
        GUI.color = color;
        float angle = Vector3.Angle(p2 - p1, Vector2.right);
        if (p1.y > p2.y) angle = -angle;
        float length = (p2 - p1).magnitude;
        GUIUtility.RotateAroundPivot(angle, p1);
        GUI.DrawTexture(new Rect(p1.x, p1.y, length, width), Texture2D.whiteTexture);
        GUI.matrix = matrix;
        GUI.color = oldColor;
    }

    private string GetMemorySafetyLevel(float memoryMB, out string color)
    {
        if (memoryMB < 400f)
        {
            color = "#00FF00";
            return "🟢 安全";
        }
        else if (memoryMB < 800f)
        {
            color = "#FFFF00";
            return "🟡 偏高";
        }
        else
        {
            color = "#FF4040";
            return "🔴 危险";
        }
    }
}
