// Adapted for the browser from Examples/Shapes/LinesBezier.cs
namespace Examples.Web;

public class ShapesLinesBezier : IWebExample
{
    public string Name => "Shapes / Lines Bezier";

    private const int screenWidth = 960;
    private const int screenHeight = 540;

    private Vector2 _start;
    private Vector2 _end;

    public void Init()
    {
        _start = new Vector2(0, 0);
        _end = new Vector2(screenWidth, screenHeight);
    }

    public void Update()
    {
        if (IsMouseButtonDown(MouseButton.Left))
        {
            _start = GetMousePosition();
        }
        else if (IsMouseButtonDown(MouseButton.Right))
        {
            _end = GetMousePosition();
        }

        BeginDrawing();
        ClearBackground(Color.RayWhite);

        DrawText("USE MOUSE LEFT-RIGHT CLICK to DEFINE LINE START and END POINTS", 15, 20, 20, Color.Gray);
        DrawLineBezier(_start, _end, 2.0f, Color.Red);

        EndDrawing();
    }

    public void Unload()
    {
    }
}
