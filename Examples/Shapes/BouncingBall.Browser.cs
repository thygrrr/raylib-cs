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
        private bool _pause;
        private int _framesCounter;

        public void Init()
        {
            _position = new Vector2(GetScreenWidth() / 2f, GetScreenHeight() / 2f);
            _speed = new Vector2(5.0f, 4.0f);
            _pause = false;
            _framesCounter = 0;
        }

        public void Update()
        {
            if (IsKeyPressed(KeyboardKey.Space))
            {
                _pause = !_pause;
            }

            if (!_pause)
            {
                _position.X += _speed.X;
                _position.Y += _speed.Y;

                if ((_position.X >= GetScreenWidth() - Radius) || (_position.X <= Radius)) _speed.X *= -1.0f;
                if ((_position.Y >= GetScreenHeight() - Radius) || (_position.Y <= Radius)) _speed.Y *= -1.0f;
            }
            else
            {
                _framesCounter += 1;
            }

            BeginDrawing();
            ClearBackground(Color.RayWhite);

            DrawCircleV(_position, Radius, Color.Maroon);
            DrawText("PRESS SPACE to PAUSE BALL MOVEMENT", 10, GetScreenHeight() - 25, 20, Color.LightGray);

            if (_pause && ((_framesCounter / 30) % 2) == 0)
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
