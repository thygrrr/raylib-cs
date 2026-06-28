#if BROWSER
using Examples;
namespace Examples.Core;

public partial class WindowLetterbox : IExample
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
        public string Name => "Core / Window Letterbox";

        private const int gamescreenWidth = 640;
        private const int gamescreenHeight = 480;

        private RenderTexture2D _target;
        private Color[] _colors;

        public void Init()
        {
            // Render texture initialization, used to hold the rendering result so we can easily resize it
            _target = LoadRenderTexture(gamescreenWidth, gamescreenHeight);
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
                (float)GetScreenWidth() / gamescreenWidth,
                (float)GetScreenHeight() / gamescreenHeight
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
            virtualMouse.X = (mouse.X - (GetScreenWidth() - (gamescreenWidth * scale)) * 0.5f) / scale;
            virtualMouse.Y = (mouse.Y - (GetScreenHeight() - (gamescreenHeight * scale)) * 0.5f) / scale;

            Vector2 max = new((float)gamescreenWidth, (float)gamescreenHeight);
            virtualMouse = Vector2.Clamp(virtualMouse, Vector2.Zero, max);

            // Apply the same transformation as the virtual mouse to the real mouse (i.e. to work with raygui)
            //SetMouseOffset(-(GetScreenWidth() - (gamescreenWidth*scale))*0.5f, -(GetScreenHeight() - (gamescreenHeight*scale))*0.5f);
            //SetMouseScale(1/scale, 1/scale);

            // Draw everything in the render texture, note this will not be rendered on screen, yet
            BeginTextureMode(_target);
            ClearBackground(Color.RayWhite);  // Clear render texture background color

            for (int i = 0; i < 10; i++)
            {
                DrawRectangle(0, (gamescreenHeight / 10) * i, gamescreenWidth, gamescreenHeight / 10, _colors[i]);
            }

            DrawText(
                "If executed inside a window,\nyou can resize the window,\nand see the screen scaling!",
                10,
                25,
                20,
                Color.White
            );

            DrawText($"Default Mouse: [{(int)mouse.X} , {(int)mouse.Y}]", 350, 25, 20, Color.Green);
            DrawText($"Virtual Mouse: [{(int)virtualMouse.X} , {(int)virtualMouse.Y}]", 350, 55, 20, Color.Yellow);

            EndTextureMode();

            BeginDrawing();
            ClearBackground(Color.Black);     // Clear screen background

            // Draw render texture to screen, properly scaled
            Rectangle sourceRec = new(
                0.0f,
                0.0f,
                (float)_target.Texture.Width,
                (float)-_target.Texture.Height
            );
            Rectangle destRec = new(
                (GetScreenWidth() - ((float)gamescreenWidth * scale)) * 0.5f,
                (GetScreenHeight() - ((float)gamescreenHeight * scale)) * 0.5f,
                (float)gamescreenWidth * scale,
                (float)gamescreenHeight * scale
            );
            DrawTexturePro(_target.Texture, sourceRec, destRec, new Vector2(0, 0), 0.0f, Color.White);

            EndDrawing();
        }

        public void Unload()
        {
            UnloadRenderTexture(_target);
        }
    }
}
#endif
