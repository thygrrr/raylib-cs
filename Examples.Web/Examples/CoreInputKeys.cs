// Adapted for the browser from Examples/Core/InputKeys.cs
namespace Examples.Web;

public class CoreInputKeys : IWebExample
{
    public string Name => "Core / Input Keys";

    private Vector2 _ball;

    public void Init()
    {
        _ball = new Vector2(GetScreenWidth() / 2f, GetScreenHeight() / 2f);
    }

    public void Update()
    {
        if (IsKeyDown(KeyboardKey.Right)) _ball.X += 2.0f;
        if (IsKeyDown(KeyboardKey.Left)) _ball.X -= 2.0f;
        if (IsKeyDown(KeyboardKey.Up)) _ball.Y -= 2.0f;
        if (IsKeyDown(KeyboardKey.Down)) _ball.Y += 2.0f;

        BeginDrawing();
        ClearBackground(Color.RayWhite);

        DrawText("move the ball with arrow keys", 10, 10, 20, Color.DarkGray);
        DrawCircleV(_ball, 50, Color.Maroon);

        EndDrawing();
    }

    public void Unload()
    {
    }
}
