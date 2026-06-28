#if BROWSER
using Examples;
namespace Examples.Core;

public partial class InputGamepad : IExample
{
    private readonly BrowserAdapter _browserAdapter = new();

    public string Name => _browserAdapter.Name;

    public void Init()
    {
        _browserAdapter.Init();
    }

    public void Update()
    {
        _browserAdapter.Update();
    }

    public void Unload()
    {
        _browserAdapter.Unload();
    }

    private sealed class BrowserAdapter : IExample
    {
        public string Name => "Core / Input Gamepad";

        // NOTE: Gamepad name ID depends on drivers and OS
        // These are some possible names the gamepads could have.
        private const string XBOX_ALIAS_1 = "xbox";
        private const string XBOX_ALIAS_2 = "x-box";
        private const string PS_ALIAS_1 = "playstation";
        private const string PS_ALIAS_2 = "sony";

        // Set axis deadzones
        private const float leftStickDeadzoneX = 0.1f;
        private const float leftStickDeadzoneY = 0.1f;
        private const float rightStickDeadzoneX = 0.1f;
        private const float rightStickDeadzoneY = 0.1f;
        private const float leftTriggerDeadzone = -0.9f;
        private const float rightTriggerDeadzone = -0.9f;

        private Texture2D _texPs3Pad;
        private Texture2D _texXboxPad;

        private int _gamepad;
        private Rectangle _vibrateButton;

        public void Init()
        {
            _texPs3Pad = LoadTexture("resources/ps3.png");
            _texXboxPad = LoadTexture("resources/xbox.png");

            _gamepad = 0;
            _vibrateButton = new Rectangle();
        }

        public void Update()
        {
            if (IsKeyPressed(KeyboardKey.Left) && _gamepad > 0)
            {
                _gamepad--;
            }

            if (IsKeyPressed(KeyboardKey.Right))
            {
                _gamepad++;
            }

            Vector2 mousePosition = GetMousePosition();

            _vibrateButton = new Rectangle(10, 70.0f + 20 * GetGamepadAxisCount(_gamepad) + 20, 75, 24);
            if (IsMouseButtonPressed(MouseButton.Left) && CheckCollisionPointRec(mousePosition, _vibrateButton))
            {
                SetGamepadVibration(_gamepad, 1.0f, 1.0f, 1.0f);
            }

            BeginDrawing();
            ClearBackground(Color.RayWhite);

            if (IsGamepadAvailable(_gamepad))
            {
                string gamepadName = GetGamepadName_(_gamepad);
                DrawText($"GP{_gamepad}: {gamepadName}", 10, 10, 10, Color.Black);

                // Get axis values
                float leftStickX = GetGamepadAxisMovement(_gamepad, GamepadAxis.LeftX);
                float leftStickY = GetGamepadAxisMovement(_gamepad, GamepadAxis.LeftY);
                float rightStickX = GetGamepadAxisMovement(_gamepad, GamepadAxis.RightX);
                float rightStickY = GetGamepadAxisMovement(_gamepad, GamepadAxis.RightY);
                float leftTrigger = GetGamepadAxisMovement(_gamepad, GamepadAxis.LeftTrigger);
                float rightTrigger = GetGamepadAxisMovement(_gamepad, GamepadAxis.RightTrigger);

                // Calculate deadzones
                if (leftStickX > -leftStickDeadzoneX && leftStickX < leftStickDeadzoneX)
                {
                    leftStickX = 0.0f;
                }

                if (leftStickY > -leftStickDeadzoneY && leftStickY < leftStickDeadzoneY)
                {
                    leftStickY = 0.0f;
                }

                if (rightStickX > -rightStickDeadzoneX && rightStickX < rightStickDeadzoneX)
                {
                    rightStickX = 0.0f;
                }

                if (rightStickY > -rightStickDeadzoneY && rightStickY < rightStickDeadzoneY)
                {
                    rightStickY = 0.0f;
                }

                if (leftTrigger < leftTriggerDeadzone)
                {
                    leftTrigger = -1.0f;
                }

                if (rightTrigger < rightTriggerDeadzone)
                {
                    rightTrigger = -1.0f;
                }

                if (gamepadName.Contains(XBOX_ALIAS_1, StringComparison.OrdinalIgnoreCase) ||
                    gamepadName.Contains(XBOX_ALIAS_2, StringComparison.OrdinalIgnoreCase))
                {
                    DrawTexture(_texXboxPad, 0, 0, Color.DarkGray);

                    // Draw buttons: xbox home
                    if (IsGamepadButtonDown(_gamepad, GamepadButton.Middle))
                    {
                        DrawCircle(394, 89, 19, Color.Red);
                    }

                    // Draw buttons: basic
                    if (IsGamepadButtonDown(_gamepad, GamepadButton.MiddleRight))
                    {
                        DrawCircle(436, 150, 9, Color.Red);
                    }

                    if (IsGamepadButtonDown(_gamepad, GamepadButton.MiddleLeft))
                    {
                        DrawCircle(352, 150, 9, Color.Red);
                    }

                    if (IsGamepadButtonDown(_gamepad, GamepadButton.RightFaceLeft))
                    {
                        DrawCircle(501, 151, 15, Color.Blue);
                    }

                    if (IsGamepadButtonDown(_gamepad, GamepadButton.RightFaceDown))
                    {
                        DrawCircle(536, 187, 15, Color.Lime);
                    }

                    if (IsGamepadButtonDown(_gamepad, GamepadButton.RightFaceRight))
                    {
                        DrawCircle(572, 151, 15, Color.Maroon);
                    }

                    if (IsGamepadButtonDown(_gamepad, GamepadButton.RightFaceUp))
                    {
                        DrawCircle(536, 115, 15, Color.Gold);
                    }

                    // Draw buttons: d-pad
                    DrawRectangle(317, 202, 19, 71, Color.Black);
                    DrawRectangle(293, 228, 69, 19, Color.Black);
                    if (IsGamepadButtonDown(_gamepad, GamepadButton.LeftFaceUp))
                    {
                        DrawRectangle(317, 202, 19, 26, Color.Red);
                    }

                    if (IsGamepadButtonDown(_gamepad, GamepadButton.LeftFaceDown))
                    {
                        DrawRectangle(317, 202 + 45, 19, 26, Color.Red);
                    }

                    if (IsGamepadButtonDown(_gamepad, GamepadButton.LeftFaceLeft))
                    {
                        DrawRectangle(292, 228, 25, 19, Color.Red);
                    }

                    if (IsGamepadButtonDown(_gamepad, GamepadButton.LeftFaceRight))
                    {
                        DrawRectangle(292 + 44, 228, 26, 19, Color.Red);
                    }

                    // Draw buttons: left-right back
                    if (IsGamepadButtonDown(_gamepad, GamepadButton.LeftTrigger1))
                    {
                        DrawCircle(259, 61, 20, Color.Red);
                    }

                    if (IsGamepadButtonDown(_gamepad, GamepadButton.RightTrigger1))
                    {
                        DrawCircle(536, 61, 20, Color.Red);
                    }

                    // Draw axis: left joystick
                    Color leftGamepadColor = Color.Black;
                    if (IsGamepadButtonDown(_gamepad, GamepadButton.LeftThumb))
                    {
                        leftGamepadColor = Color.Red;
                    }

                    DrawCircle(259, 152, 39, Color.Black);
                    DrawCircle(259, 152, 34, Color.LightGray);
                    DrawCircle(259 + (int)(leftStickX * 20), 152 + (int)(leftStickY * 20), 25, leftGamepadColor);

                    // Draw axis: right joystick
                    Color rightGamepadColor = Color.Black;
                    if (IsGamepadButtonDown(_gamepad, GamepadButton.RightThumb))
                    {
                        rightGamepadColor = Color.Red;
                    }

                    DrawCircle(461, 237, 38, Color.Black);
                    DrawCircle(461, 237, 33, Color.LightGray);
                    DrawCircle(461 + (int)(rightStickX * 20), 237 + (int)(rightStickY * 20), 25, rightGamepadColor);

                    // Draw axis: left-right triggers
                    DrawRectangle(170, 30, 15, 70, Color.Gray);
                    DrawRectangle(604, 30, 15, 70, Color.Gray);
                    DrawRectangle(170, 30, 15, (int)(((1 + leftTrigger) / 2) * 70), Color.Red);
                    DrawRectangle(604, 30, 15, (int)(((1 + rightTrigger) / 2) * 70), Color.Red);
                }
                else if (gamepadName.Contains(PS_ALIAS_1, StringComparison.OrdinalIgnoreCase) || gamepadName.Contains(PS_ALIAS_2, StringComparison.OrdinalIgnoreCase))
                {
                    DrawTexture(_texPs3Pad, 0, 0, Color.DarkGray);

                    // Draw buttons: ps
                    if (IsGamepadButtonDown(_gamepad, GamepadButton.Middle))
                    {
                        DrawCircle(396, 222, 13, Color.Red);
                    }

                    // Draw buttons: basic
                    if (IsGamepadButtonDown(_gamepad, GamepadButton.MiddleLeft))
                    {
                        DrawRectangle(328, 170, 32, 13, Color.Red);
                    }

                    if (IsGamepadButtonDown(_gamepad, GamepadButton.MiddleRight))
                    {
                        DrawTriangle(
                            new Vector2(436, 168),
                            new Vector2(436, 185),
                            new Vector2(464, 177),
                            Color.Red
                        );
                    }

                    if (IsGamepadButtonDown(_gamepad, GamepadButton.RightFaceUp))
                    {
                        DrawCircle(557, 144, 13, Color.Lime);
                    }

                    if (IsGamepadButtonDown(_gamepad, GamepadButton.RightFaceRight))
                    {
                        DrawCircle(586, 173, 13, Color.Red);
                    }

                    if (IsGamepadButtonDown(_gamepad, GamepadButton.RightFaceDown))
                    {
                        DrawCircle(557, 203, 13, Color.Violet);
                    }

                    if (IsGamepadButtonDown(_gamepad, GamepadButton.RightFaceLeft))
                    {
                        DrawCircle(527, 173, 13, Color.Pink);
                    }

                    // Draw buttons: d-pad
                    DrawRectangle(225, 132, 24, 84, Color.Black);
                    DrawRectangle(195, 161, 84, 25, Color.Black);
                    if (IsGamepadButtonDown(_gamepad, GamepadButton.LeftFaceUp))
                    {
                        DrawRectangle(225, 132, 24, 29, Color.Red);
                    }

                    if (IsGamepadButtonDown(_gamepad, GamepadButton.LeftFaceDown))
                    {
                        DrawRectangle(225, 132 + 54, 24, 30, Color.Red);
                    }

                    if (IsGamepadButtonDown(_gamepad, GamepadButton.LeftFaceLeft))
                    {
                        DrawRectangle(195, 161, 30, 25, Color.Red);
                    }

                    if (IsGamepadButtonDown(_gamepad, GamepadButton.LeftFaceRight))
                    {
                        DrawRectangle(195 + 54, 161, 30, 25, Color.Red);
                    }

                    // Draw buttons: left-right back buttons
                    if (IsGamepadButtonDown(_gamepad, GamepadButton.LeftTrigger1))
                    {
                        DrawCircle(239, 82, 20, Color.Red);
                    }

                    if (IsGamepadButtonDown(_gamepad, GamepadButton.RightTrigger1))
                    {
                        DrawCircle(557, 82, 20, Color.Red);
                    }

                    // Draw axis: left joystick
                    Color leftGamepadColor = Color.Black;
                    if (IsGamepadButtonDown(_gamepad, GamepadButton.LeftThumb))
                    {
                        leftGamepadColor = Color.Red;
                    }

                    DrawCircle(319, 255, 35, Color.Black);
                    DrawCircle(319, 255, 31, Color.LightGray);
                    DrawCircle(319 + (int)(leftStickX * 20), 255 + (int)(leftStickY * 20), 25, leftGamepadColor);

                    // Draw axis: right joystick
                    Color rightGamepadColor = Color.Black;
                    if (IsGamepadButtonDown(_gamepad, GamepadButton.RightThumb))
                    {
                        rightGamepadColor = Color.Red;
                    }

                    DrawCircle(475, 255, 35, Color.Black);
                    DrawCircle(475, 255, 31, Color.LightGray);
                    DrawCircle(475 + (int)(rightStickX * 20), 255 + (int)(rightStickY * 20), 25, rightGamepadColor);

                    // Draw axis: left-right triggers
                    DrawRectangle(169, 48, 15, 70, Color.Gray);
                    DrawRectangle(611, 48, 15, 70, Color.Gray);
                    DrawRectangle(169, 48, 15, (int)(((1 + leftTrigger) / 2) * 70), Color.Red);
                    DrawRectangle(611, 48, 15, (int)(((1 + rightTrigger) / 2) * 70), Color.Red);
                }
                else
                {
                    // Draw background: generic
                    DrawRectangleRounded(new Rectangle(175, 110, 460, 220), 0.3f, 16, Color.DarkGray);

                    // Draw buttons: basic
                    DrawCircle(365, 170, 12, Color.RayWhite);
                    DrawCircle(405, 170, 12, Color.RayWhite);
                    DrawCircle(445, 170, 12, Color.RayWhite);
                    DrawCircle(516, 191, 17, Color.RayWhite);
                    DrawCircle(551, 227, 17, Color.RayWhite);
                    DrawCircle(587, 191, 17, Color.RayWhite);
                    DrawCircle(551, 155, 17, Color.RayWhite);
                    if (IsGamepadButtonDown(_gamepad, GamepadButton.MiddleLeft))
                    {
                        DrawCircle(365, 170, 10, Color.Red);
                    }

                    if (IsGamepadButtonDown(_gamepad, GamepadButton.Middle))
                    {
                        DrawCircle(405, 170, 10, Color.Green);
                    }

                    if (IsGamepadButtonDown(_gamepad, GamepadButton.MiddleRight))
                    {
                        DrawCircle(445, 170, 10, Color.Blue);
                    }

                    if (IsGamepadButtonDown(_gamepad, GamepadButton.RightFaceLeft))
                    {
                        DrawCircle(516, 191, 15, Color.Gold);
                    }

                    if (IsGamepadButtonDown(_gamepad, GamepadButton.RightFaceDown))
                    {
                        DrawCircle(551, 227, 15, Color.Blue);
                    }

                    if (IsGamepadButtonDown(_gamepad, GamepadButton.RightFaceRight))
                    {
                        DrawCircle(587, 191, 15, Color.Green);
                    }

                    if (IsGamepadButtonDown(_gamepad, GamepadButton.RightFaceUp))
                    {
                        DrawCircle(551, 155, 15, Color.Red);
                    }

                    // Draw buttons: d-pad
                    DrawRectangle(245, 145, 28, 88, Color.RayWhite);
                    DrawRectangle(215, 174, 88, 29, Color.RayWhite);
                    DrawRectangle(247, 147, 24, 84, Color.Black);
                    DrawRectangle(217, 176, 84, 25, Color.Black);
                    if (IsGamepadButtonDown(_gamepad, GamepadButton.LeftFaceUp))
                    {
                        DrawRectangle(247, 147, 24, 29, Color.Red);
                    }

                    if (IsGamepadButtonDown(_gamepad, GamepadButton.LeftFaceDown))
                    {
                        DrawRectangle(247, 147 + 54, 24, 30, Color.Red);
                    }

                    if (IsGamepadButtonDown(_gamepad, GamepadButton.LeftFaceLeft))
                    {
                        DrawRectangle(217, 176, 30, 25, Color.Red);
                    }

                    if (IsGamepadButtonDown(_gamepad, GamepadButton.LeftFaceRight))
                    {
                        DrawRectangle(217 + 54, 176, 30, 25, Color.Red);
                    }

                    // Draw buttons: left-right back
                    DrawRectangleRounded(new Rectangle(215, 98, 100, 10), 0.5f, 16, Color.DarkGray);
                    DrawRectangleRounded(new Rectangle(495, 98, 100, 10), 0.5f, 16, Color.DarkGray);
                    if (IsGamepadButtonDown(_gamepad, GamepadButton.LeftTrigger1))
                    {
                        DrawRectangleRounded(new Rectangle(215, 98, 100, 10), 0.5f, 16, Color.Red);
                    }

                    if (IsGamepadButtonDown(_gamepad, GamepadButton.RightTrigger1))
                    {
                        DrawRectangleRounded(new Rectangle(495, 98, 100, 10), 0.5f, 16, Color.Red);
                    }

                    // Draw axis: left joystick
                    Color leftGamepadColor = Color.Black;
                    if (IsGamepadButtonDown(_gamepad, GamepadButton.LeftThumb))
                    {
                        leftGamepadColor = Color.Red;
                    }

                    DrawCircle(345, 260, 40, Color.Black);
                    DrawCircle(345, 260, 35, Color.LightGray);
                    DrawCircle(345 + (int)(leftStickX * 20), 260 + (int)(leftStickY * 20), 25, leftGamepadColor);

                    // Draw axis: right joystick
                    Color rightGamepadColor = Color.Black;
                    if (IsGamepadButtonDown(_gamepad, GamepadButton.RightThumb))
                    {
                        rightGamepadColor = Color.Red;
                    }

                    DrawCircle(465, 260, 40, Color.Black);
                    DrawCircle(465, 260, 35, Color.LightGray);
                    DrawCircle(465 + (int)(rightStickX * 20), 260 + (int)(rightStickY * 20), 25, rightGamepadColor);

                    // Draw axis: left-right triggers
                    DrawRectangle(151, 110, 15, 70, Color.Gray);
                    DrawRectangle(644, 110, 15, 70, Color.Gray);
                    DrawRectangle(151, 110, 15, (int)(((1 + leftTrigger) / 2) * 70), Color.Red);
                    DrawRectangle(644, 110, 15, (int)(((1 + rightTrigger) / 2) * 70), Color.Red);
                }

                DrawText($"DETECTED AXIS [{GetGamepadAxisCount(_gamepad)}]:", 10, 50, 10, Color.Maroon);

                for (int i = 0; i < GetGamepadAxisCount(_gamepad); i++)
                {
                    DrawText(
                        $"AXIS {i}: {GetGamepadAxisMovement(_gamepad, (GamepadAxis)i):F2}",
                        20,
                        70 + 20 * i,
                        10,
                        Color.DarkGray
                    );
                }

                // Draw vibrate button
                DrawRectangleRec(_vibrateButton, Color.SkyBlue);
                DrawText("VIBRATE", (int)(_vibrateButton.X + 14), (int)(_vibrateButton.Y + 1), 10, Color.DarkGray);

                if (GetGamepadButtonPressed() != (int)GamepadButton.Unknown)
                {
                    DrawText($"DETECTED BUTTON: {GetGamepadButtonPressed()}", 10, 430, 10, Color.Red);
                }
                else
                {
                    DrawText("DETECTED BUTTON: NONE", 10, 430, 10, Color.Gray);
                }
            }
            else
            {
                DrawText($"GP{_gamepad}: NOT DETECTED", 10, 10, 10, Color.Gray);
                DrawTexture(_texXboxPad, 0, 0, Color.LightGray);
            }

            EndDrawing();
        }

        public void Unload()
        {
            UnloadTexture(_texPs3Pad);
            UnloadTexture(_texXboxPad);
        }
    }
}
#endif
