// Adapted for the browser from Examples/Shapes/RectangleScaling.cs
namespace Examples.Web;

public class ShapesRectangleScaling : IWebExample
{
    public string Name => "Shapes / Rectangle Scaling";

    private const int MOUSE_SCALE_MARK_SIZE = 12;

    private Rectangle _rec;
    private Vector2 _mousePosition;
    private bool _mouseScaleReady;
    private bool _mouseScaleMode;

    public void Init()
    {
        _rec = new Rectangle(100, 100, 200, 80);
        _mousePosition = new Vector2(0, 0);

        _mouseScaleReady = false;
        _mouseScaleMode = false;
    }

    public void Update()
    {
        _mousePosition = GetMousePosition();

        Rectangle area = new(
            _rec.X + _rec.Width - MOUSE_SCALE_MARK_SIZE,
            _rec.Y + _rec.Height - MOUSE_SCALE_MARK_SIZE,
            MOUSE_SCALE_MARK_SIZE,
            MOUSE_SCALE_MARK_SIZE
        );

        if (CheckCollisionPointRec(_mousePosition, _rec) &&
            CheckCollisionPointRec(_mousePosition, area))
        {
            _mouseScaleReady = true;
            if (IsMouseButtonPressed(MouseButton.Left))
            {
                _mouseScaleMode = true;
            }
        }
        else
        {
            _mouseScaleReady = false;
        }

        if (_mouseScaleMode)
        {
            _mouseScaleReady = true;

            _rec.Width = (_mousePosition.X - _rec.X);
            _rec.Height = (_mousePosition.Y - _rec.Y);

            if (_rec.Width < MOUSE_SCALE_MARK_SIZE)
            {
                _rec.Width = MOUSE_SCALE_MARK_SIZE;
            }
            if (_rec.Height < MOUSE_SCALE_MARK_SIZE)
            {
                _rec.Height = MOUSE_SCALE_MARK_SIZE;
            }

            if (IsMouseButtonReleased(MouseButton.Left))
            {
                _mouseScaleMode = false;
            }
        }

        BeginDrawing();
        ClearBackground(Color.RayWhite);

        DrawText("Scale rectangle dragging from bottom-right corner!", 10, 10, 20, Color.Gray);
        DrawRectangleRec(_rec, ColorAlpha(Color.Green, 0.5f));

        if (_mouseScaleReady)
        {
            DrawRectangleLinesEx(_rec, 1, Color.Red);
            DrawTriangle(
                new Vector2(_rec.X + _rec.Width - MOUSE_SCALE_MARK_SIZE, _rec.Y + _rec.Height),
                new Vector2(_rec.X + _rec.Width, _rec.Y + _rec.Height),
                new Vector2(_rec.X + _rec.Width, _rec.Y + _rec.Height - MOUSE_SCALE_MARK_SIZE),
                Color.Red
            );
        }

        EndDrawing();
    }

    public void Unload()
    {
    }
}
