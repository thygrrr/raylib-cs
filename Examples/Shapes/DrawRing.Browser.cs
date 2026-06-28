#if BROWSER
using Examples;
using System;

namespace Examples.Shapes;

public partial class DrawRing : IExample
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
        public string Name => "Shapes / Draw Ring";

        private Vector2 _center;
        private float _innerRadius;
        private float _outerRadius;
        private float _startAngle;
        private float _endAngle;
        private float _segments;
        private bool _drawRing;
        private bool _drawRingLines;
        private bool _drawCircleLines;

        public void Init()
        {
            _center = new Vector2((GetScreenWidth() - 300) / 2.0f, GetScreenHeight() / 2.0f);

            _innerRadius = 80.0f;
            _outerRadius = 190.0f;

            _startAngle = 0.0f;
            _endAngle = 360.0f;
            _segments = 0.0f;

            _drawRing = true;
            _drawRingLines = false;
            _drawCircleLines = false;
        }

        public void Update()
        {
            // NOTE: All variables update happens inside GUI control functions

            BeginDrawing();
            ClearBackground(Color.RayWhite);

            DrawLine(500, 0, 500, GetScreenHeight(), Fade(Color.LightGray, 0.6f));
            DrawRectangle(500, 0, GetScreenWidth() - 500, GetScreenHeight(), Fade(Color.LightGray, 0.3f));

            if (_drawRing)
            {
                DrawRing(
                    _center,
                    _innerRadius,
                    _outerRadius,
                    _startAngle,
                    _endAngle,
                    (int)_segments,
                    Fade(Color.Maroon, 0.3f)
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
                    (int)_segments,
                    Fade(Color.Black, 0.4f)
                );
            }
            if (_drawCircleLines)
            {
                DrawCircleSectorLines(
                    _center,
                    _outerRadius,
                    _startAngle,
                    _endAngle,
                    (int)_segments,
                    Fade(Color.Black, 0.4f)
                );
            }

            int minSegments = (int)MathF.Ceiling((_endAngle - _startAngle) / 90);
            Color color = (_segments >= minSegments) ? Color.Maroon : Color.DarkGray;
            DrawText($"MODE: {((_segments >= minSegments) ? "MANUAL" : "AUTO")}", 600, 270, 10, color);

            DrawFPS(10, 10);

            EndDrawing();
        }

        public void Unload()
        {
        }
    }
}
#endif
