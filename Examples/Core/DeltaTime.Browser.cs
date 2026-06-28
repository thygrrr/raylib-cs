#if BROWSER
using Examples;
namespace Examples.Core;

public partial class DeltaTime : IExample
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
        public string Name => "Core / Delta Time";

        private const int screenWidth = 800;
        private const int screenHeight = 450;

        private const float speed = 10.0f;
        private const float circleRadius = 32.0f;

        private int _currentFps;
        private Vector2 _deltaCircle;
        private Vector2 _frameCircle;

        public void Init()
        {
            _currentFps = 60;

            _deltaCircle = new Vector2(0, (float)screenHeight / 3.0f);
            _frameCircle = new Vector2(0, (float)screenHeight * (2.0f / 3.0f));
        }

        public void Update()
        {
            // Adjust the FPS target based on the mouse wheel
            // NOTE: The browser host owns SetTargetFPS, so changing the target here only
            // updates the on-screen label; the actual frame rate is fixed by the host.
            float mouseWheel = GetMouseWheelMove();
            if (mouseWheel != 0)
            {
                _currentFps += (int)mouseWheel;
                if (_currentFps < 0)
                {
                    _currentFps = 0;
                }
            }

            // GetFrameTime() returns the time it took to draw the last frame, in seconds (usually called delta time)
            // Uses the delta time to make the circle look like it's moving at a "consistent" speed regardless of FPS

            // Multiply by 6.0 (an arbitrary value) in order to make the speed
            // visually closer to the other circle (at 60 fps), for comparison
            _deltaCircle.X += GetFrameTime() * 6.0f * speed;
            // This circle can move faster or slower visually depending on the FPS
            _frameCircle.X += 0.1f * speed;

            // If either circle is off the screen, reset it back to the start
            if (_deltaCircle.X > screenWidth)
            {
                _deltaCircle.X = 0;
            }

            if (_frameCircle.X > screenWidth)
            {
                _frameCircle.X = 0;
            }

            // Reset both circles positions
            if (IsKeyPressed(KeyboardKey.R))
            {
                _deltaCircle.X = 0;
                _frameCircle.X = 0;
            }

            BeginDrawing();
            ClearBackground(Color.RayWhite);

            // Draw both circles to the screen
            DrawCircleV(_deltaCircle, circleRadius, Color.Red);
            DrawCircleV(_frameCircle, circleRadius, Color.Blue);

            // Draw the help text
            // Determine what help text to show depending on the current FPS target
            var fpsText = "";
            if (_currentFps <= 0)
            {
                fpsText = $"FPS: unlimited ({GetFPS()})";
            }
            else
            {
                fpsText = $"FPS: {GetFPS()} (target: {_currentFps})";
            }
            DrawText(fpsText, 10, 10, 20, Color.DarkGray);
            DrawText($"Frame time: {GetFrameTime():F2} ms", 10, 30, 20, Color.DarkGray);
            DrawText("Use the scroll wheel to change the fps limit, r to reset", 10, 50, 20, Color.DarkGray);

            // Draw the text above the circles
            DrawText("FUNC: x += GetFrameTime()*speed", 10, 90, 20, Color.Red);
            DrawText("FUNC: x += speed", 10, 240, 20, Color.Blue);

            EndDrawing();
        }

        public void Unload()
        {
        }
    }
}
#endif
