using UnityEngine;

public class ShowFPS : MonoBehaviour
{
    // Time.realtimeSinceStartup: 指的是我们当前从启动开始到现在运行的时间，单位(s)
    private readonly float _timeDelta = 0.5f; // 固定的一个时间间隔
    private float _prevTime = 0.0f; // 上一次统计FPS的时间;
    private float _fps = 0.0f; // 计算出来的FPS的值;
    private int _iFrames = 0; // 累计我们刷新的帧数;
    private GUIStyle _style; // GUI显示;

    void Awake()
    {
        // 假设CPU 100% 工作的状态下FPS 300，
        // 当你设置了这个以后，他就维持在60FPS左右，不会继续冲高;
        // -1, 游戏引擎就会不段的刷新我们的画面，有多高，刷多高; 60FPS左右;
        //Application.targetFrameRate = 60;
    }

    void Start()
    {
        _prevTime = Time.realtimeSinceStartup;
        _style = new GUIStyle();
        _style.fontSize = 15;
        _style.normal.textColor = new Color(255, 255, 255);
    }

    void OnGUI()
    {
        GUI.Label(new Rect(0, Screen.height - 20, 200, 200), "FPS:" + _fps.ToString("f2"), _style);
    }

    void Update()
    {
        _iFrames++;

        if (Time.realtimeSinceStartup >= _prevTime + _timeDelta)
        {
            _fps = ((float)_iFrames) / (Time.realtimeSinceStartup - _prevTime);
            _prevTime = Time.realtimeSinceStartup;
            _iFrames = 0; // 重新累积我们的FPS
        }
    }
}