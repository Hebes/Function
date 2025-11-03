using System.Collections.Generic;
using UnityEngine;

public class InputMode
{
    private AllMode _currentMode;
    private readonly Stack<AllMode> _modeStack = new();
    public AllMode CurrentMode => _currentMode;

    public InputMode()
    {
        SetInputMode(AllMode.Normal);
    }
    public bool CheckMode(AllMode modeValue) => _currentMode == modeValue;
    public void Reset()
    {
        _modeStack.Clear();
        _currentMode = AllMode.Normal;
        SetInputMode(AllMode.Normal);
        // InputSetting.Assistant = true;
        // InputSetting.Resume(true);
    }
    public void EnterMode(AllMode nextMode)
    {
        LogBefore(nextMode, true);
        _modeStack.Push(_currentMode);
        SetInputMode(nextMode);
        _currentMode = nextMode;
        LogAfter(nextMode, true);
        if (nextMode == AllMode.UI || nextMode == AllMode.Story)
        {
           // SingletonMono<MobileInputPlayer>.Instance.Visible = false;
        }
    }
    public void ExitMode(AllMode mode)
    {
        LogBefore(mode, false);
        if (_currentMode != mode)
        {
            Debug.LogError(string.Concat("当前模式与退出意图不符，当前模式为 ", _currentMode, " 退出模式为", mode));
            return;
        }
        if (_modeStack.Count == 0)
        {
            Debug.LogError("没有状态可以退出");
            return;
        }
        AllMode allMode = _modeStack.Pop();
        SetInputMode(allMode);
        _currentMode = allMode;
        LogAfter(mode, false);
        if (mode == AllMode.UI || mode == AllMode.Story)
        {
            //SingletonMono<MobileInputPlayer>.Instance.Visible = true;
        }
        
    }
    private static void SetInputMode(AllMode mode)
	{
		switch (mode)
		{
		case AllMode.Normal:
			SetAllGameInputState(true);
			Input1.Game.Search.IsOpen = true;
			SetAllUIInputState(false);
			Input1.UI.Pause.IsOpen = true;
			Input1.UI.Debug.IsOpen = true;
			SetAllStoryInputState(false);
			SetAllShiInputState(false);
			break;
		case AllMode.Battle:
			SetAllGameInputState(true);
			Input1.Game.Search.IsOpen = false;
			SetAllUIInputState(false);
			Input1.UI.Pause.IsOpen = true;
			Input1.UI.Debug.IsOpen = true;
			SetAllStoryInputState(false);
			SetAllShiInputState(false);
			break;
		case AllMode.Story:
			SetAllGameInputState(false);
			SetAllUIInputState(false);
			SetAllStoryInputState(true);
			SetAllShiInputState(false);
			break;
		case AllMode.UI:
			SetAllGameInputState(false);
			SetAllUIInputState(true);
			SetAllStoryInputState(false);
			SetAllShiInputState(false);
			break;
		
		}
	}
	private static void SetAllGameInputState(bool open)
	{
		Input1.Game.MoveDown.IsOpen = open;
		Input1.Game.MoveUp.IsOpen = open;
		Input1.Game.MoveLeft.IsOpen = open;
		Input1.Game.MoveRight.IsOpen = open;
		Input1.Game.Atk.IsOpen = open;
		Input1.Game.CirtAtk.IsOpen = open;
		Input1.Game.Jump.IsOpen = open;
		Input1.Game.Flash.Left.IsOpen = open;
		Input1.Game.Flash.Right.IsOpen = open;
		Input1.Game.Flash.Up.IsOpen = open;
		Input1.Game.Flash.Down.IsOpen = open;
		Input1.Game.Flash.RightUp.IsOpen = open;
		Input1.Game.Flash.LeftUp.IsOpen = open;
		Input1.Game.Flash.RightDown.IsOpen = open;
		Input1.Game.Flash.LeftDown.IsOpen = open;
		Input1.Game.Flash.FaceDirection.IsOpen = open;
		Input1.Game.UpRising.IsOpen = open;
		Input1.Game.HitGround.IsOpen = open;
		Input1.Game.Charging.IsOpen = open;
		Input1.Game.Execute.IsOpen = open;
		Input1.Game.Defence.IsOpen = open;
		Input1.Game.JumpDown.IsOpen = open;
		Input1.Game.Chase.IsOpen = open;
		Input1.Game.FlashAttack.IsOpen = open;
		Input1.Game.BladeStorm.IsOpen = open;
		Input1.Game.ShadeAtk.IsOpen = open;
		Input1.Game.Search.IsOpen = open;
		Input1.Game.L2.IsOpen = open;
		Input1.Game.R2.IsOpen = open;
	}
	private static void SetAllUIInputState(bool open)
	{
		Input1.UI.Down.IsOpen = open;
		Input1.UI.Up.IsOpen = open;
		Input1.UI.Left.IsOpen = open;
		Input1.UI.Right.IsOpen = open;
		Input1.UI.Confirm.IsOpen = open;
		Input1.UI.Cancel.IsOpen = open;
		Input1.UI.Pause.IsOpen = open;
		Input1.UI.Debug.IsOpen = open;
	}
	private static void SetAllStoryInputState(bool open)
	{
		Input1.Story.Skip.IsOpen = open;
		Input1.Story.BackGame.IsOpen = open;
	}
	private static void SetAllShiInputState(bool open)
	{
		Input1.Shi.Down.IsOpen = open;
		Input1.Shi.Up.IsOpen = open;
		Input1.Shi.Left.IsOpen = open;
		Input1.Shi.Right.IsOpen = open;
		Input1.Shi.Jump.IsOpen = open;
		Input1.Shi.Pause.IsOpen = open;
	}
    private void LogBefore(AllMode mode, bool isEnter) => $"当前模式{_currentMode},{(!isEnter ? "退出" : "进入")}模式:{mode}".Log();
    private void LogAfter(AllMode mode, bool isEnter) => $"{(isEnter ? "进入" : "退出")}模式成功，当前模式{_currentMode}".Log();
    
}

public enum AllMode
{
    /// <summary>
    /// 默认
    /// </summary>
    Normal,

    /// <summary>
    /// 战斗
    /// </summary>
    Battle,

    /// <summary>
    /// 故事
    /// </summary>
    Story,

    /// <summary>
    /// UI
    /// </summary>
    UI,
}