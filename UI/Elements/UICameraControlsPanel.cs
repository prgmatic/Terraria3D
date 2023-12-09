using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.UI.Elements;

namespace Terraria3D;

public class UICameraControlsPanel : UIPanel
{
	public bool Visible { get; set; } = true;

	private float _height = 0;
	private UITextPanel<string> _moveCameraModeButton;
	private UITextPanel<string> _startYOffsetButton;
	private CameraDriver _cameraDriver => Terraria3D.Instance.Scene.CameraDriver;

	public UICameraControlsPanel()
	{
		AddLine("Camera Controls", 1.5f);
		AddControlLine("F", "Reset");
		AddControlLine("WASD", " Move");
		AddControlLine("QE", " Move Vertically");
		AddControlLine("Left Click", " Rotate");
		AddControlLine("Alt + Left Click", " Orbit");
		AddControlLine("Alt + Middle Click", " Pan");
		AddControlLine("Alt + Right Click", " Zoom");
		AddControlLine("Arrow Keys", "Orbit");
		_moveCameraModeButton = AddButtonWithLabel("Move Mode");
		UpdateMoveCameraModeButtonText();
		_moveCameraModeButton.OnLeftClick += (evt, listener) =>
		{
			if (evt.Target == _moveCameraModeButton)
			{
				_cameraDriver.MoveMode = _cameraDriver.MoveMode == CameraDriver.CameraMoveMode.Relative ?
					CameraDriver.CameraMoveMode.World :
					CameraDriver.CameraMoveMode.Relative;
				UpdateMoveCameraModeButtonText();
			}
		};
		_startYOffsetButton = AddButtonWithLabel("Start Y Pos");
		UpdateStartYOffsetButtonText();
		_startYOffsetButton.OnLeftClick += (evt, listener) =>
		{
			if(evt.Target == _startYOffsetButton)
			{
				_cameraDriver.YStartPosition = _cameraDriver.YStartPosition == CameraDriver.CameraYStart.Center ?
					CameraDriver.CameraYStart.Offset :
					CameraDriver.CameraYStart.Center;
					
				_cameraDriver.ResetCameraPosition();
				UpdateStartYOffsetButtonText();
			}
		};

		Width.Pixels = 250;
		Height.Pixels = _height + PaddingTop + PaddingBottom;

		Settings.SettingsLoaded += SettingsLoaded;
	}

	public void Dispose() => Settings.SettingsLoaded -= SettingsLoaded;

	public void SettingsLoaded(Serialization.SettingsData settings)
	{
		UpdateMoveCameraModeButtonText();
		UpdateStartYOffsetButtonText();
	}

	private void AddLine(string text, float textScale = 1, bool large = false)
	{
		var uiText = new UIText(text, textScale, large);
		uiText.Top.Pixels = _height;
		uiText.MarginBottom = 10;
		uiText.Recalculate();
		_height += uiText.GetOuterDimensions().Height;
		Append(uiText);
	}

	private void AddControlLine(string control, string action)
	{
		var controlText = new UIText(control);
		controlText.Top.Pixels = _height;
		controlText.MarginTop = 5;
		controlText.Recalculate();
		_height += controlText.GetOuterDimensions().Height;

		var actionText = new UIText(action);
		actionText.Top = controlText.Top;
		actionText.HAlign = 1;

		Append(controlText);
		Append(actionText);
	}

	private UITextPanel<string> AddButtonWithLabel(string labelText)
	{
		var button = new UITextPanel<string>(string.Empty);
		button.HAlign = 1;
		button.Top.Pixels = _height;
		button.MarginTop = 5;
		button.Recalculate();
		_height += button.GetOuterDimensions().Height;

		var label = new UIText(labelText);
		label.Top.Pixels = button.Top.Pixels + 15;
		Append(button);
		Append(label);
		return button;
	}

	private void UpdateMoveCameraModeButtonText()
		=> _moveCameraModeButton.SetText(_cameraDriver.MoveMode == CameraDriver.CameraMoveMode.Relative ? "Camera" : "World");
	private void UpdateStartYOffsetButtonText()
		=> _startYOffsetButton.SetText(_cameraDriver.YStartPosition == CameraDriver.CameraYStart.Offset ? "Offset" : "Center");

	public override void Update(GameTime gameTime)
	{
		if (Visible)
			base.Update(gameTime);
	}

	public override void Draw(SpriteBatch spriteBatch)
	{
		if (Visible)
			base.Draw(spriteBatch);
	}
}