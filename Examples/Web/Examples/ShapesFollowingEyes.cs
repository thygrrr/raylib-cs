// Adapted for the browser from Examples/Shapes/FollowingEyes.cs
using System;

namespace Examples.Web;

public class ShapesFollowingEyes : IWebExample
{
    public string Name => "Shapes / Following Eyes";

    private const float ScleraRadius = 80;
    private const float IrisRadius = 24;

    private Vector2 _scleraLeft;
    private Vector2 _scleraRight;
    private Vector2 _irisLeft;
    private Vector2 _irisRight;

    public void Init()
    {
        _scleraLeft = new Vector2(GetScreenWidth() / 2 - 100, GetScreenHeight() / 2);
        _scleraRight = new Vector2(GetScreenWidth() / 2 + 100, GetScreenHeight() / 2);
        _irisLeft = _scleraLeft;
        _irisRight = _scleraRight;
    }

    public void Update()
    {
        _irisLeft = GetMousePosition();
        _irisRight = GetMousePosition();

        ClampIris(ref _irisLeft, _scleraLeft);
        ClampIris(ref _irisRight, _scleraRight);

        BeginDrawing();
        ClearBackground(Color.RayWhite);

        DrawCircleV(_scleraLeft, ScleraRadius, Color.LightGray);
        DrawCircleV(_irisLeft, IrisRadius, Color.Brown);
        DrawCircleV(_irisLeft, 10, Color.Black);

        DrawCircleV(_scleraRight, ScleraRadius, Color.LightGray);
        DrawCircleV(_irisRight, IrisRadius, Color.DarkGreen);
        DrawCircleV(_irisRight, 10, Color.Black);

        DrawFPS(10, 10);

        EndDrawing();
    }

    private static void ClampIris(ref Vector2 iris, Vector2 sclera)
    {
        if (!CheckCollisionPointCircle(iris, sclera, ScleraRadius - 20))
        {
            float angle = MathF.Atan2(iris.Y - sclera.Y, iris.X - sclera.X);
            iris.X = sclera.X + (ScleraRadius - IrisRadius) * MathF.Cos(angle);
            iris.Y = sclera.Y + (ScleraRadius - IrisRadius) * MathF.Sin(angle);
        }
    }

    public void Unload()
    {
    }
}
