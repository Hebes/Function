using System;
using System.Collections;

/// <summary>
/// 输入
/// </summary>
public static class Input1 
{ 
    public static bool JoystickIsOpen = true;

    public static readonly InputButtonProcessor AnyKey = new InputButtonProcessor();

    /// <summary>
    /// 游戏
    /// </summary>
    public static class Game
    {
        public static readonly InputButtonProcessor MoveDown = new InputButtonProcessor();

        public static readonly InputButtonProcessor MoveUp = new InputButtonProcessor();

        public static readonly InputButtonProcessor MoveLeft = new InputButtonProcessor();

        public static readonly InputButtonProcessor MoveRight = new InputButtonProcessor();

        public static readonly InputButtonProcessor Atk = new InputButtonProcessor();

        public static readonly InputButtonProcessor CirtAtk = new InputButtonProcessor();

        public static readonly InputButtonProcessor Jump = new InputButtonProcessor();

        public static readonly InputButtonProcessor UpRising = new InputButtonProcessor();

        public static readonly InputButtonProcessor HitGround = new InputButtonProcessor();

        public static readonly InputButtonProcessor Charging = new InputButtonProcessor();

        public static readonly InputButtonProcessor Execute = new InputButtonProcessor();

        public static readonly InputButtonProcessor Defence = new InputButtonProcessor();

        public static readonly InputButtonProcessor JumpDown = new InputButtonProcessor();

        public static readonly InputButtonProcessor Chase = new InputButtonProcessor();

        public static readonly InputButtonProcessor FlashAttack = new InputButtonProcessor();

        public static readonly InputButtonProcessor BladeStorm = new InputButtonProcessor();

        [Obsolete] public static readonly InputButtonProcessor ShadeAtk = new InputButtonProcessor();

        public static readonly InputButtonProcessor Search = new InputButtonProcessor();

        public static readonly InputButtonProcessor L2 = new InputButtonProcessor();

        public static readonly InputButtonProcessor R2 = new InputButtonProcessor();

        public static class Flash
        {
            public static readonly InputButtonProcessor Left = new InputButtonProcessor();

            public static readonly InputButtonProcessor Right = new InputButtonProcessor();

            public static readonly InputButtonProcessor Up = new InputButtonProcessor();

            public static readonly InputButtonProcessor Down = new InputButtonProcessor();

            public static readonly InputButtonProcessor RightUp = new InputButtonProcessor();

            public static readonly InputButtonProcessor LeftUp = new InputButtonProcessor();

            public static readonly InputButtonProcessor RightDown = new InputButtonProcessor();

            public static readonly InputButtonProcessor LeftDown = new InputButtonProcessor();

            public static readonly InputButtonProcessor FaceDirection = new InputButtonProcessor();
        }
    }

    /// <summary>
    /// UI
    /// </summary>
    public static class UI
    {
        public static readonly InputButtonProcessor Down = new InputButtonProcessor();

        public static readonly InputButtonProcessor Up = new InputButtonProcessor();

        public static readonly InputButtonProcessor Left = new InputButtonProcessor();

        public static readonly InputButtonProcessor Right = new InputButtonProcessor();

        public static readonly InputButtonProcessor Confirm = new InputButtonProcessor();

        public static readonly InputButtonProcessor Cancel = new InputButtonProcessor();

        public static readonly InputButtonProcessor Pause = new InputButtonProcessor();

        public static readonly InputButtonProcessor Debug = new InputButtonProcessor();
    }

    /// <summary>
    /// 故事
    /// </summary>
    public static class Story
    {
        public static readonly InputButtonProcessor Skip = new InputButtonProcessor();

        public static readonly InputButtonProcessor BackGame = new InputButtonProcessor();
    }

    /// <summary>
    /// 辅助关门系统
    /// </summary>
    public static class Shi
    {
        public static readonly InputButtonProcessor Down = new InputButtonProcessor();

        public static readonly InputButtonProcessor Up = new InputButtonProcessor();

        public static readonly InputButtonProcessor Left = new InputButtonProcessor();

        public static readonly InputButtonProcessor Right = new InputButtonProcessor();

        public static readonly InputButtonProcessor Jump = new InputButtonProcessor();

        public static readonly InputButtonProcessor Pause = new InputButtonProcessor();
    }

    public static class Vibration
    {
        /// <summary>
        /// 振动数据
        /// </summary>
        private static float[][] VibrationData
        {
            get
            {
                return default;  }//SingletonMono<EnemyDataPreload>.Instance.VibrationData;
        }

        public static void Vibrate(int id)
        {
            // if (!R.Settings.IsVibrate)
            // {
            //     return;
            // }
            //
            // id--;
            // if (_isPlaying)
            // {
            //     R.Coroutine.Stop(PlayCoroutine(id));
            //     SetVibration(0f, 0f);
            //     _isPlaying = false;
            // }
            //
            // R.Coroutine.Start(PlayCoroutine(id));
        }

        public static void Stop()
        {
            // if (!R.Settings.IsVibrate)
            // {
            //     return;
            // }
            //
            // R.Coroutine.Stop("PlayCoroutine");
            // SetVibration(0f, 0f);
            // _isPlaying = false;
        }

        private static IEnumerator PlayCoroutine(int id)
        {
            _isPlaying = true;
            float[] leftMoterRecords = VibrationData[id * 2];
            float[] rightMoterRecord = VibrationData[id * 2 + 1];
            for (int i = 0; i < leftMoterRecords.Length; i++)
            {
                if (!_isPlaying)
                {
                    SetVibration(0f, 0f);
                    yield break;
                }

                float l2Axis = leftMoterRecords[i];
                float r2Axis = rightMoterRecord[i];
                SetVibration(l2Axis, r2Axis);
                yield return null;
                for (int j = 0; j < _period - 1; j++)
                {
                    yield return null;
                }
            }

            SetVibration(0f, 0f);
            yield break;
        }

        private static void SetVibration(float leftMotorValue, float rightMotorValue)
        {
            //InputDriver.Vibration.SetVibration(leftMotorValue, rightMotorValue);
        }

        private static bool _isPlaying;

        private static int _period = 3;
    }
}