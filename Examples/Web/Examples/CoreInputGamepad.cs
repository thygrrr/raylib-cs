// Adapted for the browser from Examples/Core/InputGamepad.cs
namespace Examples.Web;

public class CoreInputGamepad : IWebExample
{
    public string Name => "Core / Input Gamepad";

    // NOTE: Gamepad name ID depends on drivers and OS
    // These are some possible names the gamepads could have.
    private const string XBOX_ALIAS_1 = "xbox";
    private const string XBOX_ALIAS_2 = "x-box";
    private const string PS_ALIAS_1 = "playstation";
    private const string PS_ALIAS_2 = "sony";

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
                DrawCircle(259, 152, 39, Color.Black);
                DrawCircle(259, 152, 34, Color.LightGray);
                DrawCircle(
                    259 + (int)(GetGamepadAxisMovement(_gamepad, GamepadAxis.LeftX) * 20),
                    152 + (int)(GetGamepadAxisMovement(_gamepad, GamepadAxis.LeftY) * 20),
                    25,
                    Color.Black
                );

                // Draw axis: right joystick
                DrawCircle(461, 237, 38, Color.Black);
                DrawCircle(461, 237, 33, Color.LightGray);
                DrawCircle(
                    461 + (int)(GetGamepadAxisMovement(_gamepad, GamepadAxis.RightX) * 20),
                    237 + (int)(GetGamepadAxisMovement(_gamepad, GamepadAxis.RightY) * 20),
                    25, Color.Black
                );

                // Draw axis: left-right triggers
                float leftTriggerX = GetGamepadAxisMovement(_gamepad, GamepadAxis.LeftTrigger);
                float rightTriggerX = GetGamepadAxisMovement(_gamepad, GamepadAxis.RightTrigger);
                DrawRectangle(170, 30, 15, 70, Color.Gray);
                DrawRectangle(604, 30, 15, 70, Color.Gray);
                DrawRectangle(170, 30, 15, (int)(((1.0f + leftTriggerX) / 2.0f) * 70), Color.Red);
                DrawRectangle(604, 30, 15, (int)(((1.0f + rightTriggerX) / 2.0f) * 70), Color.Red);
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
                DrawCircle(319, 255, 35, Color.Black);
                DrawCircle(319, 255, 31, Color.LightGray);
                DrawCircle(
                    319 + (int)(GetGamepadAxisMovement(_gamepad, GamepadAxis.LeftX) * 20),
                    255 + (int)(GetGamepadAxisMovement(_gamepad, GamepadAxis.LeftY) * 20),
                    25,
                    Color.Black
                );

                // Draw axis: right joystick
                DrawCircle(475, 255, 35, Color.Black);
                DrawCircle(475, 255, 31, Color.LightGray);
                DrawCircle(
                    475 + (int)(GetGamepadAxisMovement(_gamepad, GamepadAxis.RightX) * 20),
                    255 + (int)(GetGamepadAxisMovement(_gamepad, GamepadAxis.RightY) * 20),
                    25,
                    Color.Black
                );

                // Draw axis: left-right triggers
                float leftTriggerX = GetGamepadAxisMovement(_gamepad, GamepadAxis.LeftTrigger);
                float rightTriggerX = GetGamepadAxisMovement(_gamepad, GamepadAxis.RightTrigger);
                DrawRectangle(169, 48, 15, 70, Color.Gray);
                DrawRectangle(611, 48, 15, 70, Color.Gray);
                DrawRectangle(169, 48, 15, (int)(((1.0f - leftTriggerX) / 2.0f) * 70), Color.Red);
                DrawRectangle(611, 48, 15, (int)(((1.0f - rightTriggerX) / 2.0f) * 70), Color.Red);
            }
            else
            {
                DrawText("- GENERIC GAMEPAD -", 280, 180, 20, Color.Gray);
                // TODO: Draw generic gamepad
            }

            DrawText($"DETECTED AXIS [{GetGamepadAxisCount(_gamepad)}]:", 10, 50, 10, Color.Maroon);

            for (int i = 0; i < GetGamepadAxisCount(_gamepad); i++)
            {
                DrawText(
                    $"AXIS {i}: {GetGamepadAxisMovement(_gamepad, (GamepadAxis)i)}",
                    20,
                    70 + 20 * i,
                    10,
                    Color.DarkGray
                );
            }

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
