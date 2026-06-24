// Adapted for the browser from Examples/Shapes/DrawRectangleRounded.cs
namespace Examples.Web;

public class ShapesDrawRectangleRounded : IWebExample
{
    public string Name => "Shapes / Draw Rectangle Rounded";

    private float _roundness;
    private int _width;
    private int _height;
    private int _segments;
    private int _lineThick;

    private bool _drawRect;
    private bool _drawRoundedRect;
    private bool _drawRoundedLines;

    public void Init()
    {
        _roundness = 0.2f;
        _width = 400;
        _height = 200;
        _segments = 0;
        _lineThick = 10;

        _drawRect = false;
        _drawRoundedRect = false;
        _drawRoundedLines = true;
    }

    public void Update()
    {
        Rectangle rec = new(
            (GetScreenWidth() - _width - 250) / 2.0f,
            (GetScreenHeight() - _height) / 2.0f,
            (float)_width,
            (float)_height
        );

        BeginDrawing();
        ClearBackground(Color.RayWhite);

        DrawLine(560, 0, 560, GetScreenHeight(), ColorAlpha(Color.LightGray, 0.6f));
        DrawRectangle(560, 0, GetScreenWidth() - 500, GetScreenHeight(), ColorAlpha(Color.LightGray, 0.3f));

        if (_drawRect)
        {
            DrawRectangleRec(rec, ColorAlpha(Color.Gold, 0.6f));
        }
        if (_drawRoundedRect)
        {
            DrawRectangleRounded(rec, _roundness, _segments, ColorAlpha(Color.Maroon, 0.2f));
        }
        if (_drawRoundedLines)
        {
            DrawRectangleRoundedLinesEx(rec, _roundness, _segments, (float)_lineThick, ColorAlpha(Color.Maroon, 0.4f));
        }

        string text = $"MODE: {((_segments >= 4) ? "MANUAL" : "AUTO")}";
        DrawText(text, 640, 280, 10, (_segments >= 4) ? Color.Maroon : Color.DarkGray);
        DrawFPS(10, 10);

        EndDrawing();
    }

    public void Unload()
    {
    }
}
