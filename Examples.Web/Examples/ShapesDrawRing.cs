// Adapted for the browser from Examples/Shapes/DrawRing.cs
using System;

namespace Examples.Web;

public class ShapesDrawRing : IWebExample
{
    public string Name => "Shapes / Draw Ring";

    private Vector2 _center;
    private float _innerRadius;
    private float _outerRadius;
    private int _startAngle;
    private int _endAngle;
    private int _segments;
    private int _minSegments;
    private bool _drawRing;
    private bool _drawRingLines;
    private bool _drawCircleLines;

    public void Init()
    {
        _center = new Vector2((GetScreenWidth() - 300) / 2, GetScreenHeight() / 2);

        _innerRadius = 80.0f;
        _outerRadius = 190.0f;

        _startAngle = 0;
        _endAngle = 360;
        _segments = 0;
        _minSegments = 4;

        _drawRing = true;
        _drawRingLines = false;
        _drawCircleLines = false;
    }

    public void Update()
    {
        // NOTE: All variables update happens inside GUI control functions

        BeginDrawing();
        ClearBackground(Color.RayWhite);

        DrawLine(500, 0, 500, GetScreenHeight(), ColorAlpha(Color.LightGray, 0.6f));
        DrawRectangle(500, 0, GetScreenWidth() - 500, GetScreenHeight(), ColorAlpha(Color.LightGray, 0.3f));

        if (_drawRing)
        {
            DrawRing(
                _center,
                _innerRadius,
                _outerRadius,
                _startAngle,
                _endAngle,
                _segments,
                ColorAlpha(Color.Maroon, 0.3f)
            );
        }
        if (_drawRingLines)
        {
            DrawRingLines(
                _center,
                _innerRadius,
                _outerRadius,
                _startAngle,
                _endAngle,
                _segments,
                ColorAlpha(Color.Black, 0.4f)
            );
        }
        if (_drawCircleLines)
        {
            DrawCircleSectorLines(
                _center,
                _outerRadius,
                _startAngle,
                _endAngle,
                _segments,
                ColorAlpha(Color.Black, 0.4f)
            );
        }

        _minSegments = (int)MathF.Ceiling((_endAngle - _startAngle) / 90);
        Color color = (_segments >= _minSegments) ? Color.Maroon : Color.DarkGray;
        DrawText($"MODE: {((_segments >= _minSegments) ? "MANUAL" : "AUTO")}", 600, 270, 10, color);

        DrawFPS(10, 10);

        EndDrawing();
    }

    public void Unload()
    {
    }
}
