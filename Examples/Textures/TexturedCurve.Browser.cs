#if BROWSER
using Examples;
using System;

namespace Examples.Textures;

public partial class TexturedCurve : IExample
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

    private unsafe sealed class BrowserAdapter : IExample
    {
        public string Name => "Textures / Textured Curve";

        private class CurvePoint
        {
            public Vector2 value;

            public float X => value.X;
            public float Y => value.Y;

            public static implicit operator CurvePoint(Vector2 v) => new CurvePoint { value = v };
            public static implicit operator Vector2(CurvePoint v) => v.value;
        }

        private Texture2D _texRoad;
        private bool _showCurve = false;
        private float _curveWidth = 50;
        private int _curveSegments = 24;
        private CurvePoint _curveStartPosition;
        private CurvePoint _curveStartPositionTangent;
        private CurvePoint _curveEndPosition;
        private CurvePoint _curveEndPositionTangent;
        private CurvePoint _curveSelectedPoint;

        public void Init()
        {
            // Load the road texture
            _texRoad = LoadTexture("resources/road.png");
            SetTextureFilter(_texRoad, TextureFilter.Bilinear);

            // Setup the curve
            _curveStartPosition = new Vector2(80, 100);
            _curveStartPositionTangent = new Vector2(100, 300);

            _curveEndPosition = new Vector2(700, 350);
            _curveEndPositionTangent = new Vector2(600, 100);

            _showCurve = false;
            _curveWidth = 50;
            _curveSegments = 24;
            _curveSelectedPoint = null;
        }

        public void Update()
        {
            UpdateCurve();
            UpdateOptions();

            BeginDrawing();
            ClearBackground(Color.RayWhite);

            DrawTexturedCurve();
            DrawCurve();

            DrawText("Drag points to move curve, press SPACE to show/hide base curve", 10, 10, 10, Color.DarkGray);
            DrawText($"Curve width: {_curveWidth} (Use + and - to adjust)", 10, 30, 10, Color.DarkGray);
            DrawText($"Curve segments: {_curveSegments} (Use LEFT and RIGHT to adjust)", 10, 50, 10, Color.DarkGray);

            EndDrawing();
        }

        public void Unload()
        {
            UnloadTexture(_texRoad);
        }

        private void DrawCurve()
        {
            if (_showCurve)
            {
                DrawSplineSegmentBezierCubic(
                    _curveStartPosition,
                    _curveEndPosition,
                    _curveStartPositionTangent,
                    _curveEndPositionTangent,
                    2,
                    Color.Blue
                );
            }

            // Draw the various control points and highlight where the mouse is
            DrawLineV(_curveStartPosition, _curveStartPositionTangent, Color.SkyBlue);
            DrawLineV(_curveStartPositionTangent, _curveEndPositionTangent, Fade(Color.LightGray, 0.4f));
            DrawLineV(_curveEndPosition, _curveEndPositionTangent, Color.Purple);
            Vector2 mouse = GetMousePosition();

            if (CheckCollisionPointCircle(mouse, _curveStartPosition, 6))
            {
                DrawCircleV(_curveStartPosition, 7, Color.Yellow);
            }
            DrawCircleV(_curveStartPosition, 5, Color.Red);

            if (CheckCollisionPointCircle(mouse, _curveStartPositionTangent, 6))
            {
                DrawCircleV(_curveStartPositionTangent, 7, Color.Yellow);
            }
            DrawCircleV(_curveStartPositionTangent, 5, Color.Maroon);

            if (CheckCollisionPointCircle(mouse, _curveEndPosition, 6))
            {
                DrawCircleV(_curveEndPosition, 7, Color.Yellow);
            }
            DrawCircleV(_curveEndPosition, 5, Color.Green);

            if (CheckCollisionPointCircle(mouse, _curveEndPositionTangent, 6))
            {
                DrawCircleV(_curveEndPositionTangent, 7, Color.Yellow);
            }
            DrawCircleV(_curveEndPositionTangent, 5, Color.DarkGreen);
        }

        private void UpdateCurve()
        {
            // If the mouse is not down, we are not editing the curve so clear the selection
            if (!IsMouseButtonDown(MouseButton.Left))
            {
                _curveSelectedPoint = null;
                return;
            }

            // If a point was selected, move it
            if (_curveSelectedPoint != null)
            {
                _curveSelectedPoint.value += GetMouseDelta();
            }

            // The mouse is down, and nothing was selected, so see if anything was picked
            Vector2 mouse = GetMousePosition();

            if (CheckCollisionPointCircle(mouse, _curveStartPosition, 6))
            {
                _curveSelectedPoint = _curveStartPosition;
            }
            else if (CheckCollisionPointCircle(mouse, _curveStartPositionTangent, 6))
            {
                _curveSelectedPoint = _curveStartPositionTangent;
            }
            else if (CheckCollisionPointCircle(mouse, _curveEndPosition, 6))
            {
                _curveSelectedPoint = _curveEndPosition;
            }
            else if (CheckCollisionPointCircle(mouse, _curveEndPositionTangent, 6))
            {
                _curveSelectedPoint = _curveEndPositionTangent;
            }
        }

        private void DrawTexturedCurve()
        {
            float step = 1.0f / _curveSegments;

            Vector2 previous = _curveStartPosition;
            Vector2 previousTangent = Vector2.Zero;
            float previousV = 0;

            // We can't compute a tangent for the first point, so we need to reuse the tangent from the first segment
            bool tangentSet = false;

            Vector2 current = Vector2.Zero;
            float t = 0.0f;

            for (int i = 1; i <= _curveSegments; i++)
            {
                // Segment the curve
                t = step * i;
                float a = MathF.Pow(1 - t, 3);
                float b = 3 * MathF.Pow(1 - t, 2) * t;
                float c = 3 * (1 - t) * MathF.Pow(t, 2);
                float d = MathF.Pow(t, 3);

                // Compute the endpoint for this segment
                current.Y = a * _curveStartPosition.Y + b * _curveStartPositionTangent.Y;
                current.Y += c * _curveEndPositionTangent.Y + d * _curveEndPosition.Y;
                current.X = a * _curveStartPosition.X + b * _curveStartPositionTangent.X;
                current.X += c * _curveEndPositionTangent.X + d * _curveEndPosition.X;

                // Vector from previous to current
                Vector2 delta = new(current.X - previous.X, current.Y - previous.Y);

                // The right hand normal to the delta vector
                Vector2 normal = Vector2.Normalize(new Vector2(-delta.Y, delta.X));

                // The v texture coordinate of the segment (add up the length of all the segments so far)
                float v = previousV + delta.Length();

                // Make sure the start point has a normal
                if (!tangentSet)
                {
                    previousTangent = normal;
                    tangentSet = true;
                }

                // Extend out the normals from the previous and current points to get the quad for this segment
                Vector2 prevPosNormal = previous + (previousTangent * _curveWidth);
                Vector2 prevNegNormal = previous + (previousTangent * -_curveWidth);

                Vector2 currentPosNormal = current + (normal * _curveWidth);
                Vector2 currentNegNormal = current + (normal * -_curveWidth);

                // Draw the segment as a quad
                Rlgl.SetTexture(_texRoad.Id);
                Rlgl.Begin(DrawMode.Quads);

                Rlgl.Color4ub(255, 255, 255, 255);
                Rlgl.Normal3f(0.0f, 0.0f, 1.0f);

                Rlgl.TexCoord2f(0, previousV);
                Rlgl.Vertex2f(prevNegNormal.X, prevNegNormal.Y);

                Rlgl.TexCoord2f(1, previousV);
                Rlgl.Vertex2f(prevPosNormal.X, prevPosNormal.Y);

                Rlgl.TexCoord2f(1, v);
                Rlgl.Vertex2f(currentPosNormal.X, currentPosNormal.Y);

                Rlgl.TexCoord2f(0, v);
                Rlgl.Vertex2f(currentNegNormal.X, currentNegNormal.Y);

                Rlgl.End();

                // The current step is the start of the next step
                previous = current;
                previousTangent = normal;
                previousV = v;
            }
        }

        private void UpdateOptions()
        {
            if (IsKeyPressed(KeyboardKey.Space))
            {
                _showCurve = !_showCurve;
            }

            // Update width
            if (IsKeyPressed(KeyboardKey.Equal))
            {
                _curveWidth += 2;
            }
            if (IsKeyPressed(KeyboardKey.Minus))
            {
                _curveWidth -= 2;
            }

            if (_curveWidth < 2)
            {
                _curveWidth = 2;
            }

            // Update segments
            if (IsKeyPressed(KeyboardKey.Left))
            {
                _curveSegments -= 2;
            }
            if (IsKeyPressed(KeyboardKey.Right))
            {
                _curveSegments += 2;
            }
            if (_curveSegments < 2)
            {
                _curveSegments = 2;
            }
        }
    }
}
#endif
