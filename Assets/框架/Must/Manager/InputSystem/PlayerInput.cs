// using System;
// using Core;
//
// public class PlayerInput : BaseBehaviour
// {
// 	private void Awake()
// 	{
// 		this.pab = base.GetComponent<PlayerAbilities>();
// 		this.pAttr = R.Player.Attribute;
// 	}
//
// 	private void Update()
// 	{
// 		if (!R.SceneData.isPausing)
// 		{
// 			this.UpdateInput();
// 		}
// 	}
//
// 	private void UpdateInput()
// 	{
// 		if (this.pAttr.isDead)
// 		{
// 			return;
// 		}
// 		if (!this.battlePause)
// 		{
// 			if (Input.Game.JumpDown.OnPressed)
// 			{
// 				this.pab.jumpDown.JumpDown();
// 				return;
// 			}
// 			if (Input.Game.BladeStorm.OnPressed)
// 			{
// 				this.pab.skill.BladeStorm();
// 				return;
// 			}
// 			if (Input.Game.ShadeAtk.OnPressed)
// 			{
// 				this.pab.skill.ShadeAtk();
// 				return;
// 			}
// 			if (PlayerInput.Setting.CanFlash)
// 			{
// 				if (Input.Game.Flash.RightUp.OnPressed)
// 				{
// 					this.pab.flash.FlashRightUp();
// 					return;
// 				}
// 				if (Input.Game.Flash.RightDown.OnPressed)
// 				{
// 					this.pab.flash.FlashRightDown();
// 					return;
// 				}
// 				if (Input.Game.Flash.LeftUp.OnPressed)
// 				{
// 					this.pab.flash.FlashLeftUp();
// 					return;
// 				}
// 				if (Input.Game.Flash.LeftDown.OnPressed)
// 				{
// 					this.pab.flash.FlashLeftDown();
// 					return;
// 				}
// 				if (Input.Game.Flash.Left.OnPressed)
// 				{
// 					this.pab.flash.FlashLeft();
// 					return;
// 				}
// 				if (Input.Game.Flash.Right.OnPressed)
// 				{
// 					this.pab.flash.FlashRight();
// 					return;
// 				}
// 				if (Input.Game.Flash.Up.OnPressed)
// 				{
// 					this.pab.flash.FlashUp();
// 					return;
// 				}
// 				if (Input.Game.Flash.Down.OnPressed)
// 				{
// 					this.pab.flash.FlashDown();
// 					return;
// 				}
// 				if (Input.Game.Flash.FaceDirection.OnPressed)
// 				{
// 					this.pab.flash.FlashFace();
// 					return;
// 				}
// 			}
// 			if (Input.Game.UpRising.OnPressed && PlayerInput.Setting.CanUpRising)
// 			{
// 				this.pab.upRising.UpJumpAttack();
// 			}
// 			if (Input.Game.HitGround.OnPressed)
// 			{
// 				this.pab.hitGround.HitGround();
// 			}
// 			if (Input.Game.FlashAttack.OnPressed && this.pab.flashAttack.PressFlashAttack())
// 			{
// 				return;
// 			}
// 			if (Input.Game.Execute.OnPressed && PlayerInput.Setting.CanExecute)
// 			{
// 				this.pab.execute.Execute();
// 				return;
// 			}
// 			if (Input.Game.Chase.OnPressed)
// 			{
// 				this.pab.chase.Chase();
// 			}
// 			if (Input.Game.Jump.OnPressed && PlayerInput.Setting.CanJump)
// 			{
// 				this.pab.jump.Jump();
// 			}
// 			if (Input.Game.Atk.OnClick && PlayerInput.Setting.CanAttack)
// 			{
// 				if (Input.Game.MoveLeft.Pressed)
// 				{
// 					this.pab.attack.PlayerAttack(-1, false);
// 				}
// 				else if (Input.Game.MoveRight.Pressed)
// 				{
// 					this.pab.attack.PlayerAttack(1, false);
// 				}
// 				else
// 				{
// 					this.pab.attack.PlayerAttack(3, false);
// 				}
// 			}
// 			if (Input.Game.CirtAtk.OnClick && PlayerInput.Setting.CanAttack)
// 			{
// 				if (Input.Game.MoveLeft.Pressed)
// 				{
// 					this.pab.attack.PlayerAttack(-1, true);
// 				}
// 				else if (Input.Game.MoveRight.Pressed)
// 				{
// 					this.pab.attack.PlayerAttack(1, true);
// 				}
// 				else
// 				{
// 					this.pab.attack.PlayerAttack(3, true);
// 				}
// 			}
// 			if (Input.Game.CirtAtk.LongPressed && PlayerInput.Setting.CanAttack)
// 			{
// 				if (Input.Game.MoveLeft.Pressed)
// 				{
// 					this.pab.attack.PlayerCirtPressAttack(-1);
// 				}
// 				else if (Input.Game.MoveRight.Pressed)
// 				{
// 					this.pab.attack.PlayerCirtPressAttack(1);
// 				}
// 				else
// 				{
// 					this.pab.attack.PlayerCirtPressAttack(3);
// 				}
// 			}
// 			if (Input.Game.CirtAtk.OnReleased && PlayerInput.Setting.CanAttack)
// 			{
// 				this.pab.attack.PlayerCirtPressAttackReleasd();
// 			}
// 			if (Input.Game.Charging.LongPressed && PlayerInput.Setting.CanCharging)
// 			{
// 				this.pab.charge.Charging();
// 			}
// 		}
// 		if (PlayerInput.Setting.CanMove)
// 		{
// 			if (Input.Game.MoveLeft.Pressed)
// 			{
// 				this.pab.move.Move(-1);
// 			}
// 			else if (Input.Game.MoveRight.Pressed)
// 			{
// 				this.pab.move.Move(1);
// 			}
// 			if (Input.Game.MoveLeft.OnReleased || Input.Game.MoveRight.OnReleased)
// 			{
// 				R.Player.Action.tempDir = 3;
// 			}
// 		}
// 	}
//
// 	private const int LEFT = -1;
//
// 	private const int RIGHT = 1;
//
// 	private const int CURRENT = 3;
//
// 	private PlayerAttribute pAttr;
//
// 	private PlayerAbilities pab;
//
// 	public bool battlePause;
//
// 	public class Setting
// 	{
// 		private static void SetFlag(PlayerInput.Setting.InputType inputType, bool value)
// 		{
// 			if (value)
// 			{
// 				PlayerInput.Setting.AllowInput |= inputType;
// 			}
// 			else
// 			{
// 				PlayerInput.Setting.AllowInput &= ~inputType;
// 			}
// 		}
//
// 		private static bool GetFlag(PlayerInput.Setting.InputType inputType)
// 		{
// 			return (inputType & PlayerInput.Setting.AllowInput) == inputType;
// 		}
//
// 		public static bool CanJump
// 		{
// 			get
// 			{
// 				return PlayerInput.Setting.GetFlag(PlayerInput.Setting.InputType.Jump);
// 			}
// 			set
// 			{
// 				PlayerInput.Setting.SetFlag(PlayerInput.Setting.InputType.Jump, value);
// 			}
// 		}
//
// 		public static bool CanMove
// 		{
// 			get
// 			{
// 				return PlayerInput.Setting.GetFlag(PlayerInput.Setting.InputType.Move);
// 			}
// 			set
// 			{
// 				PlayerInput.Setting.SetFlag(PlayerInput.Setting.InputType.Move, value);
// 			}
// 		}
//
// 		public static bool CanAttack
// 		{
// 			get
// 			{
// 				return PlayerInput.Setting.GetFlag(PlayerInput.Setting.InputType.Attack);
// 			}
// 			set
// 			{
// 				PlayerInput.Setting.SetFlag(PlayerInput.Setting.InputType.Attack, value);
// 			}
// 		}
//
// 		public static bool CanFlash
// 		{
// 			get
// 			{
// 				return PlayerInput.Setting.GetFlag(PlayerInput.Setting.InputType.Flash);
// 			}
// 			set
// 			{
// 				PlayerInput.Setting.SetFlag(PlayerInput.Setting.InputType.Flash, value);
// 			}
// 		}
//
// 		public static bool CanSkill
// 		{
// 			get
// 			{
// 				return PlayerInput.Setting.GetFlag(PlayerInput.Setting.InputType.Skill);
// 			}
// 			set
// 			{
// 				PlayerInput.Setting.SetFlag(PlayerInput.Setting.InputType.Skill, value);
// 			}
// 		}
//
// 		public static bool CanUpRising
// 		{
// 			get
// 			{
// 				return PlayerInput.Setting.GetFlag(PlayerInput.Setting.InputType.UpRising);
// 			}
// 			set
// 			{
// 				PlayerInput.Setting.SetFlag(PlayerInput.Setting.InputType.UpRising, value);
// 			}
// 		}
//
// 		public static bool CanHitGround
// 		{
// 			get
// 			{
// 				return PlayerInput.Setting.GetFlag(PlayerInput.Setting.InputType.HitGround);
// 			}
// 			set
// 			{
// 				PlayerInput.Setting.SetFlag(PlayerInput.Setting.InputType.HitGround, value);
// 			}
// 		}
//
// 		public static bool CanCharging
// 		{
// 			get
// 			{
// 				return PlayerInput.Setting.GetFlag(PlayerInput.Setting.InputType.Charging);
// 			}
// 			set
// 			{
// 				PlayerInput.Setting.SetFlag(PlayerInput.Setting.InputType.Charging, value);
// 			}
// 		}
//
// 		public static bool CanExecute
// 		{
// 			get
// 			{
// 				return PlayerInput.Setting.GetFlag(PlayerInput.Setting.InputType.Execute);
// 			}
// 			set
// 			{
// 				PlayerInput.Setting.SetFlag(PlayerInput.Setting.InputType.Execute, value);
// 			}
// 		}
//
// 		public static PlayerInput.Setting.InputType AllowInput = PlayerInput.Setting.InputType.All;
//
// 		[Flags]
// 		public enum InputType
// 		{
// 			None = 0,
// 			Move = 1,
// 			Attack = 2,
// 			Flash = 4,
// 			Skill = 8,
// 			HitGround = 16,
// 			UpRising = 32,
// 			Jump = 64,
// 			Charging = 128,
// 			Execute = 256,
// 			Option = 512,
// 			Map = 1024,
// 			All = 2047
// 		}
// 	}
// }
