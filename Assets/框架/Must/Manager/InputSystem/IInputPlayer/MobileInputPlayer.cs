using System;
using UnityEngine;
using UnityEngine.UI;

public class MobileInputPlayer : BaseBehaviour, IInputPlayer
{
	public static MobileInputPlayer instance;
	
	public bool Visible
	{
		get => _panel.gameObject.activeSelf;
		set => _panel.gameObject.SetActive(value);
	}

	public bool MainControllerVisiable
	{
		get => _mainControllerVisiable;
		set
		{
			_mainControllerVisiable = value;
			//_mainController.alpha = (float)((!value) ? 0 : 1);
		}
	}

	// public bool OptionsVisible
	// {
	// 	get => _options.IsActive;
	// 	set => _options.IsActive = true;
	// }
	//
	// public bool Button6Visible
	// {
	// 	get => _button6.IsActive;
	// 	set => _button6.IsActive = value;
	// }

	public bool L2R2Visiable
	{
		get => _l2R2Widget.gameObject.activeSelf;
		private set => _l2R2Widget.gameObject.SetActive(value);
	}

	public void Awake()
	{
		instance = this;
		Visible = true;
		MainControllerVisiable = true;
		
	}

	public void VisiableBladeStorm()
	{
		//_button1.gameObject.SetActive(R.Player.Enhancement.BladeStorm > 0);
	}

	private void OnEnable()
	{
		//EventManager.RegisterEvent<EnhanceArgs>("EnhanceLevelup", new EventManager.FBEventHandler<EnhanceArgs>(this.OnEnhancementLevelUp), EventManager.ListenerQueue.Game);
	}

	private void OnDisable()
	{
		//EventManager.UnregisterEvent<EnhanceArgs>("EnhanceLevelup", new EventManager.FBEventHandler<EnhanceArgs>(this.OnEnhancementLevelUp), EventManager.ListenerQueue.Game);
	}

	private void Update()
	{
		// if (R.Mode.CurrentMode != Mode.AllMode.Battle)
		// {
		// 	return;
		// }
		// _button3Sprite.Term = ((!R.Enemy.CanBeExecutedEnemyExist() || !R.Player.CanExecute()) ? "mobile/uisprite/button_02_1" : "mobile/uisprite/button_02_2");
	}

	public void EnterSHI()
	{
		_button1.gameObject.SetActive(false);
		_button2.gameObject.SetActive(false);
		_button3.gameObject.SetActive(false);
		_button4.gameObject.SetActive(false);
		// _button5SHIMainSprite.spriteName = "Button_03";
		// _button5SHINumSprite.spriteName = "Num03";
	}

	public void ExitSHI()
	{
		VisiableBladeStorm();
		_button2.gameObject.SetActive(true);
		_button3.gameObject.SetActive(true);
		_button4.gameObject.SetActive(true);
		// _button5SHIMainSprite.spriteName = "Button_01";
		// _button5SHINumSprite.spriteName = "Num01";
	}

	public Vector2 GetJoystick(string axis)
	{
		// if (axis != null)
		// {
		// 	if (axis == "Joystick")
		// 	{
		// 		return (!MainControllerVisiable) ? Vector2.zero : _joystick.Axis;
		// 	}
		// 	if (axis == "Swipe")
		// 	{
		// 		return (!MainControllerVisiable) ? Vector2.zero : _swipe.Direction;
		// 	}
		// }
		throw new ArgumentOutOfRangeException("axis", axis);
	}

	public Vector2 GetJoystickRaw(string axis)
	{
		if (axis != null)
		{
		}
		throw new ArgumentOutOfRangeException("axis", axis);
	}

	public bool GetButton(string buttonName)
	{
		// switch (buttonName)
		// {
		// case "Button1":
		// 	return MainControllerVisiable && _button1.IsPressed;
		// case "Button2":
		// 	return MainControllerVisiable && _button2.IsPressed;
		// case "Button3":
		// 	return MainControllerVisiable && _button3.IsPressed;
		// case "Button4":
		// 	return MainControllerVisiable && _button4.IsPressed;
		// case "Button5":
		// 	return MainControllerVisiable && _button5.IsPressed;
		// case "Button6":
		// 	return _button6.IsPressed;
		// case "Options":
		// 	return _options.IsPressed;
		// case "L2":
		// 	return _l2.IsPressed;
		// case "R2":
		// 	return _r2.IsPressed;
		// }
		//throw new Exception("buttonName", buttonName, string.Format("Button \"{0}\" is not exist.", buttonName));
		return default;
	}

	public void SetVibration(float leftMotorValue, float rightMotorValue)
	{
	}

	public void ShowL2R2(string l2, string r2)
	{
		if (!L2R2Visiable)
		{
			L2R2Visiable = true;
			MainControllerVisiable = false;
		}
		// _l2.Text = l2;
		// _r2.Text = r2;
	}

	public void HideL2R2(bool showMainController = true)
	{
		L2R2Visiable = false;
		MainControllerVisiable = showMainController;
	}

	// private bool OnEnhancementLevelUp(string eventDefine, object sender, EnhanceArgs msg)
	// {
	// 	if (!_button1.gameObject.activeSelf && msg.Name == "bladeStorm")
	// 	{
	// 		_button1.gameObject.SetActive(true);
	// 	}
	// 	return true;
	// }

	public void UpdateRadiusAndPosition()
	{
		//_joystick.UpdateRadiusAndPosition();
	}

	[SerializeField]
	private Button _panel;

	[SerializeField]
	private Button _mainController;

	[SerializeField]
	private Button _l2R2Widget;

	private bool _mainControllerVisiable;

	[SerializeField]
	private Button _button1;

	[SerializeField]
	private Button _button2;

	[SerializeField]
	private Button _button3;

	[SerializeField]
	private Button _button4;

	[SerializeField]
	private Button _button5;

	[SerializeField]
	private Button _button6;

	[SerializeField]
	private Button _options;

	[SerializeField]
	private Button _l2;

	[SerializeField]
	private Button _r2;

	// [SerializeField]
	// private FBTouchPad _joystick;//遥感 艾希
	//
	// [SerializeField]
	// private FBSwipe _swipe;
	//
	// [SerializeField]
	// private Localize _button3Sprite;
	//
	// [SerializeField]
	// private Image _button5SHIMainSprite;
	//
	// [SerializeField]
	// private UISprite _button5SHINumSprite;
}
