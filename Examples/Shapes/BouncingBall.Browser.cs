#if BROWSER
using Examples;
namespace Examples.Shapes;

public partial class BouncingBall : IExample
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
        public string Name => "Shapes / Bouncing Ball";

        private Vector2 _position;
        private Vector2 _speed;
        private const int Radius = 20;
        private const float Gravity = 0.2f;
        private bool _useGravity;
        private bool _pause;
        private int _framesCounter;

        public void Init()
        {
            _position = new Vector2(GetScreenWidth() / 2.0f, GetScreenHeight() / 2.0f);
            _speed = new Vector2(5.0f, 4.0f);
            _useGravity = true;
            _pause = false;
            _framesCounter = 0;
        }

        public void Update()
        {
            if (IsKeyPressed(KeyboardKey.G))
            {
                _useGravity = !_useGravity;
            }
            if (IsKeyPressed(KeyboardKey.Space))
            {
                _pause = !_pause;
            }

            if (!_pause)
            {
                _position.X += _speed.X;
                _position.Y += _speed.Y;

                if (_useGravity) _speed.Y += Gravity;

                if ((_position.X >= GetScreenWidth() - Radius) || (_position.X <= Radius)) _speed.X *= -1.0f;
                if ((_position.Y >= GetScreenHeight() - Radius) || (_position.Y <= Radius)) _speed.Y *= -0.95f;
            }
            else
            {
                _framesCounter++;
            }

            BeginDrawing();
            ClearBackground(Color.RayWhite);

            DrawCircleV(_position, (float)Radius, Color.Maroon);
            DrawText("PRESS SPACE to PAUSE BALL MOVEMENT", 10, GetScreenHeight() - 25, 20, Color.LightGray);

            if (_useGravity)
            {
                DrawText("GRAVITY: ON (Press G to disable)", 10, GetScreenHeight() - 50, 20, Color.DarkGreen);
            }
            else
            {
                DrawText("GRAVITY: OFF (Press G to enable)", 10, GetScreenHeight() - 50, 20, Color.Red);
            }

            if (_pause && ((_framesCounter / 30) % 2) != 0)
            {
                DrawText("PAUSED", 350, 200, 30, Color.Gray);
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
