using System.Text;
using Unity.Profiling;
using UnityEngine;

/// <summary>
/// @author 渡鸦
/// 渲染统计信息
/// https://blog.csdn.net/u010799737/article/details/134435138?spm=1001.2014.3001.5502
/// </summary>
public sealed class RenderingProfiler : MonoBehaviour
{
    private readonly StringBuilder _stringBuilder = new StringBuilder(223);

    private int _frameCount;
    private float _accumulatedFrameTime;
    private const float FrameSampleRate = 0.3f * 1000;

    private ProfilerRecorder _drawCallsRecorder;
    private ProfilerRecorder _batchesRecorder;
    private ProfilerRecorder _trianglesRecorder;
    private ProfilerRecorder _verticesRecorder;
    private ProfilerRecorder _setPassCallsRecorder;
    private ProfilerRecorder _shadowCastersRecorder;

    private bool _isDisplay;
    private Texture2D _background;
    private Color _textColor;
    private RectOffset _padding;

    private void Awake()
    {
        this._background = MakeTex(2, 2, new Color(0f, 0f, 0f, 0.7f));
        this._textColor = new Color(0.9f, 0.9f, 0.9f);
        this._padding = new RectOffset(10, 10, 5, 5);

        DontDestroyOnLoad(this.gameObject);
    }

    private GUIStyle CreateLabelStyle()
    {
        var style = new GUIStyle();
        style.normal.background = this._background;
        style.normal.textColor = this._textColor;
        style.padding = this._padding;
        style.fontSize = 32;
        return style;
    }

    private GUIStyle CreateButtonStyle()
    {
        var style = new GUIStyle(GUI.skin.button);
        style.fontSize = 32;
        return style;
    }

    private Texture2D MakeTex(int width, int height, Color col)
    {
        Color[] pix = new Color[width * height];
        for (int i = 0; i < pix.Length; i++)
            pix[i] = col;

        Texture2D result = new Texture2D(width, height);
        result.SetPixels(pix);
        result.Apply();

        return result;
    }

    private void OnEnable()
    {
        this._drawCallsRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Draw Calls Count");
        this._batchesRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Batches Count");
        this._trianglesRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Triangles Count");
        this._verticesRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Vertices Count");
        this._setPassCallsRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "SetPass Calls Count");
        this._shadowCastersRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Shadow Casters Count");
    }

    private void OnDisable()
    {
        this._drawCallsRecorder.Dispose();
        this._batchesRecorder.Dispose();
        this._trianglesRecorder.Dispose();
        this._verticesRecorder.Dispose();
        this._setPassCallsRecorder.Dispose();
        this._shadowCastersRecorder.Dispose();
    }

    private void LateUpdate()
    {
        this._accumulatedFrameTime += Time.unscaledDeltaTime * 1000.0f;
        ++this._frameCount;
        if (this._accumulatedFrameTime < FrameSampleRate)
        {
            return;
        }

        this._stringBuilder.Clear();

        this._stringBuilder.AppendLine("  >>> Statistics <<<");

        float fps = 1.0f / ((this._accumulatedFrameTime * 0.001f) / this._frameCount);
        this._stringBuilder.AppendLine($"FPS: {fps:0.0}");
        this._frameCount = 0;
        this._accumulatedFrameTime = 0.0f;

        this._stringBuilder.AppendLine($"Draw Calls: {this._drawCallsRecorder.LastValue}");
        this._stringBuilder.AppendLine($"Batches: {this._batchesRecorder.LastValue}");
        this._stringBuilder.AppendLine($"Tris: {this._trianglesRecorder.LastValue / 1000f:0.0}k");
        this._stringBuilder.AppendLine($"Verts: {this._verticesRecorder.LastValue / 1000f:0.0}k");
        this._stringBuilder.AppendLine($"Screen: {Screen.width}x{Screen.height}");
        this._stringBuilder.AppendLine($"SetPass Calls: {this._setPassCallsRecorder.LastValue}");
        this._stringBuilder.AppendLine($"Shadow Casters: {this._shadowCastersRecorder.LastValue}");
    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(13, 22, 166, 66), "Statistics", this.CreateButtonStyle()))
        {
            this._isDisplay = !this._isDisplay;
        }

        if (this._isDisplay)
        {
            GUI.Label(new Rect(13, 88, 321, 345), this._stringBuilder.ToString(), this.CreateLabelStyle());
        }
    }
}