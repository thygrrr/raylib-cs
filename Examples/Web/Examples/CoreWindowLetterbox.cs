// Adapted for the browser from Examples/Core/WindowLetterbox.cs
namespace Examples.Web;

public class CoreWindowLetterbox : IWebExample
{
    public string Name => "Core / Window Letterbox";

    private const int gameScreenWidth = 640;
    private const int gameScreenHeight = 480;

    private RenderTexture2D _target;
    private Color[] _colors;

    public void Init()
    {
        // Render texture initialization, used to hold the rendering result so we can easily resize it
        _target = LoadRenderTexture(gameScreenWidth, gameScreenHeight);
        SetTextureFilter(_target.Texture, TextureFilter.Bilinear);

        _colors = new Color[10];
        for (int i = 0; i < 10; i++)
        {
            _colors[i] = new Color(GetRandomValue(100, 250), GetRandomValue(50, 150), GetRandomValue(10, 100), 255);
        }
    }

    public void Update()
    {
        // Compute required framebuffer scaling
        float scale = MathF.Min(
            (float)GetScreenWidth() / gameScreenWidth,
            (float)GetScreenHeight() / gameScreenHeight
        );

        if (IsKeyPressed(KeyboardKey.Space))
        {
            // Recalculate random colors for the bars
            for (int i = 0; i < 10; i++)
            {
                _colors[i] = new Color(
                    GetRandomValue(100, 250),
                    GetRandomValue(50, 150),
                    GetRandomValue(10, 100),
                    255
                );
            }
        }

        // Update virtual mouse (clamped mouse value behind game screen)
        Vector2 mouse = GetMousePosition();
        Vector2 virtualMouse = Vector2.Zero;
        virtualMouse.X = (mouse.X - (GetScreenWidth() - (gameScreenWidth * scale)) * 0.5f) / scale;
        virtualMouse.Y = (mouse.Y - (GetScreenHeight() - (gameScreenHeight * scale)) * 0.5f) / scale;

        Vector2 max = new((float)gameScreenWidth, (float)gameScreenHeight);
        virtualMouse = Vector2.Clamp(virtualMouse, Vector2.Zero, max);

        BeginDrawing();
        ClearBackground(Color.Black);

        // Draw everything in the render texture, note this will not be rendered on screen, yet
        BeginTextureMode(_target);
        ClearBackground(Color.RayWhite);

        for (int i = 0; i < 10; i++)
        {
            DrawRectangle(0, (gameScreenHeight / 10) * i, gameScreenWidth, gameScreenHeight / 10, _colors[i]);
        }

        DrawText(
            "If executed inside a window,\nyou can resize the window,\nand see the screen scaling!",
            10,
            25,
            20,
            Color.White
        );

        DrawText($"Default Mouse: [{(int)mouse.X} {(int)mouse.Y}]", 350, 25, 20, Color.Green);
        DrawText($"Virtual Mouse: [{(int)virtualMouse.X}, {(int)virtualMouse.Y}]", 350, 55, 20, Color.Yellow);

        EndTextureMode();

        // Draw RenderTexture2D to window, properly scaled
        Rectangle sourceRec = new(
            0.0f,
            0.0f,
            (float)_target.Texture.Width,
            (float)-_target.Texture.Height
        );
        Rectangle destRec = new(
            (GetScreenWidth() - ((float)gameScreenWidth * scale)) * 0.5f,
            (GetScreenHeight() - ((float)gameScreenHeight * scale)) * 0.5f,
            (float)gameScreenWidth * scale,
            (float)gameScreenHeight * scale
        );
        DrawTexturePro(_target.Texture, sourceRec, destRec, new Vector2(0, 0), 0.0f, Color.White);

        EndDrawing();
    }

    public void Unload()
    {
        UnloadRenderTexture(_target);
    }
}
