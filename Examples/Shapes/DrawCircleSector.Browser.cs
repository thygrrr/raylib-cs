#if BROWSER
using Examples;
using System;

namespace Examples.Shapes;

public partial class DrawCircleSector : IExample
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
        public string Name => "Shapes / Draw Circle Sector";

        private Vector2 _center;
        private float _outerRadius;
        private int _startAngle;
        private int _endAngle;
        private int _segments;
        private int _minSegments;

        public void Init()
        {
            _center = new Vector2((GetScreenWidth() - 300) / 2, GetScreenHeight() / 2);

            _outerRadius = 180.0f;
            _startAngle = 0;
            _endAngle = 180;
            _segments = 0;
            _minSegments = 4;
        }

        public void Update()
        {
            // NOTE: All variables update happens inside GUI control functions

            BeginDrawing();
            ClearBackground(Color.RayWhite);

            DrawLine(500, 0, 500, GetScreenHeight(), ColorAlpha(Color.LightGray, 0.6f));
            DrawRectangle(500, 0, GetScreenWidth() - 500, GetScreenHeight(), ColorAlpha(Color.LightGray, 0.3f));

            DrawCircleSector(_center, _outerRadius, _startAngle, _endAngle, _segments, ColorAlpha(Color.Maroon, 0.3f));
            DrawCircleSectorLines(
                _center,
                _outerRadius,
                _startAngle,
                _endAngle,
                _segments,
                ColorAlpha(Color.Maroon, 0.6f)
            );

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
}
#endif
