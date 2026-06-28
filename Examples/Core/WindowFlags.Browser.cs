#if BROWSER
using Examples;
using static Raylib_cs.ConfigFlags;

namespace Examples.Core;

public partial class WindowFlags : IExample
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
        public string Name => "Core / Window Flags";

        private Vector2 _ballPosition;
        private Vector2 _ballSpeed;
        private float _ballRadius;
        private int _framesCounter;

        public void Init()
        {
            _ballPosition = new Vector2(GetScreenWidth() / 2.0f, GetScreenHeight() / 2.0f);
            _ballSpeed = new Vector2(5.0f, 4.0f);
            _ballRadius = 20;

            _framesCounter = 0;
        }

        public void Update()
        {
            if (IsKeyPressed(KeyboardKey.F))
            {
                // modifies window size when scaling!
                ToggleFullscreen();
            }

            if (IsKeyPressed(KeyboardKey.R))
            {
                if (IsWindowState(ResizableWindow))
                {
                    ClearWindowState(ResizableWindow);
                }
                else
                {
                    SetWindowState(ResizableWindow);
                }
            }

            if (IsKeyPressed(KeyboardKey.D))
            {
                if (IsWindowState(UndecoratedWindow))
                {
                    ClearWindowState(UndecoratedWindow);
                }
                else
                {
                    SetWindowState(UndecoratedWindow);
                }
            }

            if (IsKeyPressed(KeyboardKey.H))
            {
                if (!IsWindowState(HiddenWindow))
                {
                    SetWindowState(HiddenWindow);
                }

                _framesCounter = 0;
            }

            if (IsWindowState(HiddenWindow))
            {
                _framesCounter++;
                if (_framesCounter >= 240)
                {
                    // Show window after 3 seconds
                    ClearWindowState(HiddenWindow);
                }
            }

            if (IsKeyPressed(KeyboardKey.N))
            {
                if (!IsWindowState(MinimizedWindow))
                {
                    MinimizeWindow();
                }

                _framesCounter = 0;
            }

            if (IsWindowState(MinimizedWindow))
            {
                _framesCounter++;
                if (_framesCounter >= 240)
                {
                    // Restore window after 3 seconds
                    RestoreWindow();
                }
            }

            if (IsKeyPressed(KeyboardKey.M))
            {
                // NOTE: Requires FLAG_WINDOW_RESIZABLE enabled!
                if (IsWindowState(MaximizedWindow))
                {
                    RestoreWindow();
                }
                else
                {
                    MaximizeWindow();
                }
            }

            if (IsKeyPressed(KeyboardKey.U))
            {
                if (IsWindowState(UnfocusedWindow))
                {
                    ClearWindowState(UnfocusedWindow);
                }
                else
                {
                    SetWindowState(UnfocusedWindow);
                }
            }

            if (IsKeyPressed(KeyboardKey.T))
            {
                if (IsWindowState(TopmostWindow))
                {
                    ClearWindowState(TopmostWindow);
                }
                else
                {
                    SetWindowState(TopmostWindow);
                }
            }

            if (IsKeyPressed(KeyboardKey.A))
            {
                if (IsWindowState(AlwaysRunWindow))
                {
                    ClearWindowState(AlwaysRunWindow);
                }
                else
                {
                    SetWindowState(AlwaysRunWindow);
                }
            }

            if (IsKeyPressed(KeyboardKey.V))
            {
                if (IsWindowState(VSyncHint))
                {
                    ClearWindowState(VSyncHint);
                }
                else
                {
                    SetWindowState(VSyncHint);
                }
            }

            if (IsKeyPressed(KeyboardKey.B))
            {
                ToggleBorderlessWindowed();
            }

            // Bouncing ball logic
            _ballPosition.X += _ballSpeed.X;
            _ballPosition.Y += _ballSpeed.Y;
            if ((_ballPosition.X >= (GetScreenWidth() - _ballRadius)) || (_ballPosition.X <= _ballRadius))
            {
                _ballSpeed.X *= -1.0f;
            }
            if ((_ballPosition.Y >= (GetScreenHeight() - _ballRadius)) || (_ballPosition.Y <= _ballRadius))
            {
                _ballSpeed.Y *= -1.0f;
            }

            BeginDrawing();

            if (IsWindowState(TransparentWindow))
            {
                ClearBackground(Color.Blank);
            }
            else
            {
                ClearBackground(Color.RayWhite);
            }

            DrawCircleV(_ballPosition, _ballRadius, Color.Maroon);
            DrawRectangleLinesEx(new Rectangle(0, 0, GetScreenWidth(), GetScreenHeight()), 4, Color.RayWhite);

            DrawCircleV(GetMousePosition(), 10, Color.DarkBlue);

            DrawFPS(10, 10);

            DrawText($"Screen Size: [{GetScreenWidth()}, {GetScreenHeight()}]", 10, 40, 10, Color.Green);

            DrawText("Following flags can be set after window creation:", 10, 60, 10, Color.Gray);

            DrawWindowState(FullscreenMode, "[F] FLAG_FULLSCREEN_MODE: ", 10, 80, 10);
            DrawWindowState(ResizableWindow, "[R] FLAG_WINDOW_RESIZABLE: ", 10, 100, 10);
            DrawWindowState(UndecoratedWindow, "[D] FLAG_WINDOW_UNDECORATED: ", 10, 120, 10);
            DrawWindowState(HiddenWindow, "[H] FLAG_WINDOW_HIDDEN: ", 10, 140, 10);
            DrawWindowState(MinimizedWindow, "[N] FLAG_WINDOW_MINIMIZED: ", 10, 160, 10);
            DrawWindowState(MaximizedWindow, "[M] FLAG_WINDOW_MAXIMIZED: ", 10, 180, 10);
            DrawWindowState(UnfocusedWindow, "[G] FLAG_WINDOW_UNFOCUSED: ", 10, 200, 10);
            DrawWindowState(TopmostWindow, "[T] FLAG_WINDOW_TOPMOST: ", 10, 220, 10);
            DrawWindowState(AlwaysRunWindow, "[A] FLAG_WINDOW_ALWAYS_RUN: ", 10, 240, 10);
            DrawWindowState(VSyncHint, "[V] FLAG_VSYNC_HINT: ", 10, 260, 10);
            DrawWindowState(BorderlessWindowMode, "[B] FLAG_BORDERLESS_WINDOWED_MODE: ", 10, 280, 10);

            DrawText("Following flags can only be set before window creation:", 10, 320, 10, Color.Gray);

            DrawWindowState(HighDpiWindow, "FLAG_WINDOW_HIGHDPI: ", 10, 340, 10);
            DrawWindowState(TransparentWindow, "FLAG_WINDOW_TRANSPARENT: ", 10, 360, 10);
            DrawWindowState(Msaa4xHint, "FLAG_MSAA_4X_HINT: ", 10, 380, 10);

            EndDrawing();
        }

        public void Unload()
        {
        }

        private static void DrawWindowState(ConfigFlags flag, string text, int posX, int posY, int fontSize)
        {
            Color onColor = Color.Lime;
            Color offColor = Color.Maroon;

            if (Raylib.IsWindowState(flag))
            {
                DrawText($"{text}on", posX, posY, fontSize, onColor);
            }
            else
            {
                DrawText($"{text}off", posX, posY, fontSize, offColor);
            }
        }
    }
}
#endif
