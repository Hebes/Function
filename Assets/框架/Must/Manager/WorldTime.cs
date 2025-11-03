using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldTime : MonoBehaviour
{
    public static bool IsPausing { get; private set; } //是否暂停
    private static int _pauseNum => _timeScaleStack.Count; //暂停次数
    public static float Fps => Mathf.Clamp(_fps.Fps, 1f, float.MaxValue); //Fps
    public static float PhysicsFps => _fps.PhysicsFps; //物理学帧率

    public event EventHandler<FrozenArgs> FrozenEvent; //冻结事件
    public event EventHandler<FrozenArgs> ResumeEvent; //恢复事件

    public bool IsFrozen { get; private set; } //是否冻结
    public bool IsSlow { get; private set; } //是否是迟钝的


    private static float _sceneStartTime; //世界开始事件
    private static Stack<float> _timeScaleStack = new Stack<float>();
    private static readonly FramesPerSecond _fps = new FramesPerSecond();
    public const float TargetFps = 60f;
    private FrozenArgs _frozenArgs;
    private int _frozeFrame; //静止帧
    private bool _autoRecover; //自动恢复
    private float _slowEnd;
    private float _slowRecover;

    private void Start()
    {
        _sceneStartTime = Time.time;
        _fps.Start();
    }

    private void Update()
    {
        _fps.Update();
    }

    private void FixedUpdate()
    {
        _fps.FixedUpdate();
        if (IsFrozen && _autoRecover)
        {
            if (IsPausing) return;

            if (_frozeFrame > 0)
            {
                _frozeFrame--;
            }
            else
            {
                _autoRecover = false;
                FrozenResume();
            }
        }

        float num = _slowEnd - Time.time;
        if (IsSlow && num < 0f)
        {
            Time.timeScale = Mathf.Clamp01(Time.timeScale + _slowRecover);
            if (Math.Abs(Time.timeScale - 1f) < 1.401298E-45f)
            {
                IsSlow = false;
                _slowEnd = 0f;
            }
        }
    }

    protected void OnDestroy()
    {
        Reset();
    }

    public static void Pause()
    {
        Debug.Log("Time Pause, Push " + Time.timeScale);
        _timeScaleStack.Push(Time.timeScale);
        Time.timeScale = 0f;
        IsPausing = true;
    }

    public static void Resume()
    {
        if (_timeScaleStack.Count == 0)
        {
            return;
        }

        Debug.Log("Time Resume, Pop " + _timeScaleStack.Peek());
        Time.timeScale = _timeScaleStack.Pop();
        IsPausing = false;
    }

    /// <summary>
    /// 时间便慢慢
    /// </summary>
    /// <param name="slowTime"></param>
    /// <param name="slowScale"></param>
    public void TimeSlow(float slowTime, float slowScale)
    {
        if (IsPausing)
        {
            return;
        }

        _slowEnd = Time.time + slowTime * slowScale;
        IsSlow = true;
        Time.timeScale = slowScale;
        _slowRecover = (1f - slowScale) / 7f;
    }

    /// <summary>
    /// 每帧延时（在 60 帧每秒的帧率下）
    /// </summary>
    /// <param name="slowFrame">停帧</param>
    /// <param name="slowScale">缓慢的节奏</param>
    public void TimeSlowByFrameOn60Fps(int slowFrame, float slowScale)
    {
        TimeSlow(slowFrame / 60f, slowScale);
    }

    /// <summary>
    /// 时间冻结
    /// </summary>
    /// <param name="second"></param>
    /// <param name="type"></param>
    /// <param name="autoRecover"></param>
    public void TimeFrozen(float second, FrozenArgs.FrozenType type = FrozenArgs.FrozenType.All, bool autoRecover = true)
    {
        TimeFrozenByFixedFrame(FixedSecondToFrame(second), type, autoRecover);
    }

    /// <summary>
    /// 时间因固定帧而停滞
    /// </summary>
    /// <param name="frame"></param>
    /// <param name="type"></param>
    /// <param name="autoRecover"></param>
    public void TimeFrozenByFixedFrame(int frame, FrozenArgs.FrozenType type = FrozenArgs.FrozenType.All, bool autoRecover = true)
    {
        if (frame == 0)return;
        _frozeFrame = frame;
        if (!IsFrozen && FrozenEvent != null)
        {
            _frozenArgs = new FrozenArgs(type, null);
            FrozenEvent(this, _frozenArgs);
        }

        _autoRecover = autoRecover;
        IsFrozen = true;
    }

    public void TimeFrozenByFixedFrame(int frame, GameObject frozenTarget)
    {
        if (frame == 0)
        {
            return;
        }

        _frozeFrame = frame;
        if (!IsFrozen && FrozenEvent != null)
        {
            _frozenArgs = new FrozenArgs(FrozenArgs.FrozenType.Target, frozenTarget);
            FrozenEvent(this, _frozenArgs);
        }

        _autoRecover = true;
        IsFrozen = true;
    }

    public void FrozenResume()
    {
        if (!IsFrozen)
        {
            return;
        }

        IsFrozen = false;
        if (ResumeEvent != null)
        {
            ResumeEvent(this, _frozenArgs);
        }
    }

    public static float FrameToSecond(int frame)
    {
        return frame / ((Fps <= 30f) ? 60f : Fps);
    }

    public static float FrameToFixedSecond(int frame)
    {
        return frame * Time.fixedDeltaTime;
    }

    public static int SecondToFrame(float second)
    {
        if (second < 0f)
        {
            Debug.LogError("Seconds can not be negative");
            return 0;
        }

        return (int)(second * ((Fps <= 30f) ? 60f : Fps));
    }

    public static int FixedSecondToFrame(float second)
    {
        if (second < 0f)
        {
            Debug.LogError("Seconds can not be negative");
            return 0;
        }

        return (int)(second / Time.fixedDeltaTime);
    }

    public Coroutine WaitForSecondsIgnoreTimeScale(float seconds)
    {
        return StartCoroutine(WaitForSecondsIgnoreTimeScaleCoroutine(seconds));
    }

    private static IEnumerator WaitForSecondsIgnoreTimeScaleCoroutine(float seconds)
    {
        for (float totalSeconds = 0f; totalSeconds < seconds; totalSeconds += Time.unscaledDeltaTime)
        {
            yield return null;
        }
    }

    public static void Reset()
    {
        IsPausing = false;
        _timeScaleStack.Clear();
        Time.timeScale = 1f;
    }


    public class FrozenArgs : EventArgs
    {
        public FrozenArgs(FrozenType type, GameObject target)
        {
            Type = type;
            Target = target;
        }

        public readonly FrozenType Type;

        public readonly GameObject Target;

        public enum FrozenType
        {
            Enemy,
            Player,
            All,
            Target
        }
    }

    private class FramesPerSecond
    {
        internal void Start()
        {
            _lastInterval = Time.realtimeSinceStartup;
            _physicsLastInterval = Time.realtimeSinceStartup;
            _frames = 0;
            _physicsFrames = 0;
        }

        internal void Update()
        {
            _frames++;
            if (Time.realtimeSinceStartup > _lastInterval + 0.5f)
            {
                Fps = _frames / (Time.realtimeSinceStartup - _lastInterval);
                _frames = 0;
                _lastInterval = Time.realtimeSinceStartup;
            }
        }

        internal void FixedUpdate()
        {
            _physicsFrames++;
            if (Time.realtimeSinceStartup > _physicsLastInterval + 0.5f)
            {
                PhysicsFps = _physicsFrames / (Time.realtimeSinceStartup - _physicsLastInterval);
                _physicsFrames = 0;
                _physicsLastInterval = Time.realtimeSinceStartup;
            }
        }

        private const float UpdateInterval = 0.5f;

        private float _lastInterval;

        private int _frames;

        internal float Fps;

        private const float PhysicsUpdateInterval = 0.5f;

        private float _physicsLastInterval;

        private int _physicsFrames;

        internal float PhysicsFps;
    }
}