// Adapted for the browser from Examples/Core/InputMouse.cs
namespace Examples.Web;

public class CoreInputMouse : IWebExample
{
    public string Name => "Core / Input Mouse";

    private Vector2 _ball;
    private Color _color;

    public void Init()
    {
        _ball = new Vector2(-100.0f, -100.0f);
        _color = Color.DarkBlue;
    }

    public void Update()
    {
        if (IsKeyPressed(KeyboardKey.H))
        {
            if (IsCursorHidden())
            {
                ShowCursor();
            }
            else
            {
                HideCursor();
            }
        }

        _ball = GetMousePosition();

        if (IsMouseButtonPressed(MouseButton.Left)) _color = Color.Maroon;
        else if (IsMouseButtonPressed(MouseButton.Middle)) _color = Color.Lime;
        else if (IsMouseButtonPressed(MouseButton.Right)) _color = Color.DarkBlue;
        else if (IsMouseButtonPressed(MouseButton.Extra)) _color = Color.Yellow;
        else if (IsMouseButtonPressed(MouseButton.Forward)) _color = Color.Orange;
        else if (IsMouseButtonPressed(MouseButton.Back)) _color = Color.Beige;

        BeginDrawing();
        ClearBackground(Color.RayWhite);

        DrawCircleV(_ball, 40, _color);

        DrawText("move ball with mouse and click mouse button to change color", 10, 10, 20, Color.DarkGray);
        DrawText("Press 'H' to toggle cursor visibility", 10, 30, 20, Color.DarkGray);

        if (IsCursorHidden())
        {
            DrawText("CURSOR HIDDEN", 20, 60, 20, Color.Red);
        }
        else
        {
            DrawText("CURSOR VISIBLE", 20, 60, 20, Color.Lime);
        }

        EndDrawing();
    }

    public void Unload()
    {
    }
}
