#if BROWSER
using Examples;
namespace Examples.Shapes;

public partial class CollisionArea : IExample
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
        public string Name => "Shapes / Collision Area";

        private const int screenWidth = 800;

        private Rectangle _boxA;
        private int _boxASpeedX;
        private Rectangle _boxB;
        private Rectangle _boxCollision;
        private int _screenUpperLimit;
        private bool _pause;
        private bool _collision;

        public void Init()
        {
            // Box A: Moving box
            _boxA = new Rectangle(10, GetScreenHeight() / 2 - 50, 200, 100);
            _boxASpeedX = 4;

            // Box B: Mouse moved box
            _boxB = new Rectangle(GetScreenWidth() / 2 - 30, GetScreenHeight() / 2 - 30, 60, 60);
            _boxCollision = new Rectangle();

            _screenUpperLimit = 40;

            // Movement pause
            _pause = false;
            _collision = false;
        }

        public void Update()
        {
            // Move box if not paused
            if (!_pause)
            {
                _boxA.X += _boxASpeedX;
            }

            // Bounce box on x screen limits
            if (((_boxA.X + _boxA.Width) >= GetScreenWidth()) || (_boxA.X <= 0))
            {
                _boxASpeedX *= -1;
            }

            // Update player-controlled-box (box02)
            _boxB.X = GetMouseX() - _boxB.Width / 2;
            _boxB.Y = GetMouseY() - _boxB.Height / 2;

            // Make sure Box B does not go out of move area limits
            if ((_boxB.X + _boxB.Width) >= GetScreenWidth())
            {
                _boxB.X = GetScreenWidth() - _boxB.Width;
            }
            else if (_boxB.X <= 0)
            {
                _boxB.X = 0;
            }

            if ((_boxB.Y + _boxB.Height) >= GetScreenHeight())
            {
                _boxB.Y = GetScreenHeight() - _boxB.Height;
            }
            else if (_boxB.Y <= _screenUpperLimit)
            {
                _boxB.Y = _screenUpperLimit;
            }

            // Check boxes collision
            _collision = CheckCollisionRecs(_boxA, _boxB);

            // Get collision rectangle (only on collision)
            if (_collision)
            {
                _boxCollision = GetCollisionRec(_boxA, _boxB);
            }

            // Pause Box A movement
            if (IsKeyPressed(KeyboardKey.Space))
            {
                _pause = !_pause;
            }

            BeginDrawing();
            ClearBackground(Color.RayWhite);

            DrawRectangle(0, 0, screenWidth, _screenUpperLimit, _collision ? Color.Red : Color.Black);

            DrawRectangleRec(_boxA, Color.Gold);
            DrawRectangleRec(_boxB, Color.Blue);

            if (_collision)
            {
                // Draw collision area
                DrawRectangleRec(_boxCollision, Color.Lime);

                // Draw collision message
                int cx = GetScreenWidth() / 2 - MeasureText("COLLISION!", 20) / 2;
                int cy = _screenUpperLimit / 2 - 10;
                DrawText("COLLISION!", cx, cy, 20, Color.Black);

                // Draw collision area
                string text = $"Collision Area: {(int)_boxCollision.Width * (int)_boxCollision.Height}";
                DrawText(text, GetScreenWidth() / 2 - 100, _screenUpperLimit + 10, 20, Color.Black);
            }

            DrawFPS(10, 10);

            EndDrawing();
        }

        public void Unload()
        {
        }
    }
}
#endif
