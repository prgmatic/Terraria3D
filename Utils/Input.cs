using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Terraria;

namespace Terraria3D
{
    public static class Input
    {
        public static bool PlayerHasKeyControl
        {
            get
            {
                return !Main.blockInput && !Main.editChest && !Main.editSign && !Main.drawingPlayerChat;
            }
        }
        public static Vector2 MousePosition
        {
            get { return new Vector2(Main.mouseX, Main.mouseY); }
        }
        public static float ScrollDelta
        {
            get { return _mouseState.ScrollWheelValue - _lastMouseState.ScrollWheelValue; }
        }
        public static float ScrollValue
        {
            get { return _mouseState.ScrollWheelValue; }
        }

        private static KeyboardState _lastKeyState;
        private static MouseState _mouseState;
        private static MouseState _lastMouseState;
        internal static bool _suppressMouse = false;

        #region Key Polling
        // If te player does not have control of the keyboard (editing signs, chatting, etc), 
        // then by default, all key press queries will return as if the keys are up.
        public static bool KeyPressed(Keys key, bool ignoreIfPlayerDoesNotHaveControl = true)
        {
            if (ignoreIfPlayerDoesNotHaveControl && !PlayerHasKeyControl) return false;
            return Main.keyState.IsKeyDown(key) && _lastKeyState.IsKeyUp(key);
        }
        public static bool KeyReleased(Keys key, bool ignoreIfPlayerDoesNotHaveControl = true)
        {
            if (ignoreIfPlayerDoesNotHaveControl && !PlayerHasKeyControl) return true;
            return Main.keyState.IsKeyUp(key) && _lastKeyState.IsKeyDown(key);
        }
        public static bool IsKeyDown(Keys key, bool ignoreIfPlayerDoesNotHaveControl = true)
        {
            if (ignoreIfPlayerDoesNotHaveControl && !PlayerHasKeyControl) return false;
            return Main.keyState.IsKeyDown(key);

        }
        public static bool IsKeyUp(Keys key, bool ignoreIfPlayerDoesNotHaveControl = true)
        {
            if (ignoreIfPlayerDoesNotHaveControl && !PlayerHasKeyControl) return false;
            return Main.keyState.IsKeyUp(key);
        }
        #endregion

        #region Mouse Polling
        public static bool MouseButtonPressed(MouseButton button)
        {
            return GetMouseButton(_mouseState, button) && !GetMouseButton(_lastMouseState, button);
        }
        public static bool MouseButtonReleased(MouseButton button)
        {
            return !GetMouseButton(_mouseState, button) && GetMouseButton(_lastMouseState, button);
        }
        public static bool IsMouseButtonDown(MouseButton button)
        {
            return GetMouseButton(_mouseState, button);
        }
        public static bool IsMouseButtonUp(MouseButton button)
        {
            return !IsMouseButtonDown(button);
        }
        private static bool GetMouseButton(MouseState state, MouseButton button)
        {
            switch (button)
            {
                case MouseButton.Left:
                    return state.LeftButton == ButtonState.Pressed;
                case MouseButton.Right:
                    return state.RightButton == ButtonState.Pressed;
                case MouseButton.Middle:
                    return state.MiddleButton == ButtonState.Pressed;
            }
            return false;
        }
        #endregion

        public static void SuppressGameKeyboardInput()
        {
            Main.keyState = new KeyboardState();
        }
        public static void SuppressGameMouseInput()
        {
            if (!Main.gameMenu)
                _suppressMouse = true;
        }

        internal static void Update()
        {
            _lastKeyState = Main.keyState;
            _lastMouseState = _mouseState;
            _mouseState = Mouse.GetState();

            if (_suppressMouse)
            {
                _suppressMouse = false;
                Main.player[Main.myPlayer].mouseInterface = true;
                Main.mouseLeft = false;
                Main.mouseRight = false;
                Main.mouseLeftRelease = true;
                Main.mouseRightRelease = true;
            }
        }
    }

    public enum MouseButton
    {
        Left,
        Right,
        Middle
    }

}
