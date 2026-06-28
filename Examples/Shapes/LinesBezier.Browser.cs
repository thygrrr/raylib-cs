#if BROWSER
using Examples;
namespace Examples.Shapes;

public partial class LinesBezier : IExample
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
        public string Name => "Shapes / Lines Bezier";

        private const int screenWidth = 800;
        private const int screenHeight = 450;

        private Vector2 _startPoint;
        private Vector2 _endPoint;
        private bool _moveStartPoint;
        private bool _moveEndPoint;

        public void Init()
        {
            _startPoint = new Vector2(30, 30);
            _endPoint = new Vector2(screenWidth - 30, screenHeight - 30);
            _moveStartPoint = false;
            _moveEndPoint = false;
        }

        public void Update()
        {
            Vector2 mouse = GetMousePosition();

            if (CheckCollisionPointCircle(mouse, _startPoint, 10.0f) && IsMouseButtonDown(MouseButton.Left))
            {
                _moveStartPoint = true;
            }
            else if (CheckCollisionPointCircle(mouse, _endPoint, 10.0f) && IsMouseButtonDown(MouseButton.Left))
            {
                _moveEndPoint = true;
            }

            if (_moveStartPoint)
            {
                _startPoint = mouse;
                if (IsMouseButtonReleased(MouseButton.Left))
                {
                    _moveStartPoint = false;
                }
            }

            if (_moveEndPoint)
            {
                _endPoint = mouse;
                if (IsMouseButtonReleased(MouseButton.Left))
                {
                    _moveEndPoint = false;
                }
            }

            BeginDrawing();
            ClearBackground(Color.RayWhite);

            DrawText("MOVE START-END POINTS WITH MOUSE", 15, 20, 20, Color.Gray);

            // Draw line Cubic Bezier, in-out interpolation (easing), no control points
            DrawLineBezier(_startPoint, _endPoint, 4.0f, Color.Blue);

            // Draw start-end spline circles with some details
            DrawCircleV(_startPoint, CheckCollisionPointCircle(mouse, _startPoint, 10.0f) ? 14.0f : 8.0f, _moveStartPoint ? Color.Red : Color.Blue);
            DrawCircleV(_endPoint, CheckCollisionPointCircle(mouse, _endPoint, 10.0f) ? 14.0f : 8.0f, _moveEndPoint ? Color.Red : Color.Blue);

            EndDrawing();
        }

        public void Unload()
        {
        }
    }
}
#endif
