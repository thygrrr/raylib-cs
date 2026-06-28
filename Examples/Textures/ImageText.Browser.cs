#if BROWSER
using Examples;
namespace Examples.Textures;

public partial class ImageText : IExample
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
        public string Name => "Textures / Image Text";

        private const int screenWidth = 800;
        private const int screenHeight = 450;

        private Font _font;
        private Texture2D _texture;
        private Vector2 _position;

        public void Init()
        {
            // TTF Font loading with custom generation parameters
            _font = LoadFontEx("resources/fonts/KAISG.ttf", 64, null, 95);

            Image parrots = LoadImage("resources/parrots.png");

            // Draw over image using custom font
            ImageDrawTextEx(
                ref parrots,
                _font,
                "[Parrots font drawing]",
                new Vector2(20, 20),
                _font.BaseSize,
                0,
                Color.White
            );

            // Image converted to texture, uploaded to GPU memory (VRAM)
            _texture = LoadTextureFromImage(parrots);
            UnloadImage(parrots);

            _position = new(
                screenWidth / 2 - _texture.Width / 2,
                screenHeight / 2 - _texture.Height / 2 - 20
            );
        }

        public void Update()
        {
            bool showFont = IsKeyDown(KeyboardKey.Space);

            BeginDrawing();
            ClearBackground(Color.RayWhite);

            if (!showFont)
            {
                // Draw texture with text already drawn inside
                DrawTextureV(_texture, _position, Color.White);

                // Draw text directly using sprite font
                Vector2 textPosition = new(_position.X + 20, _position.Y + 20 + 280);
                DrawTextEx(_font, "[Parrots font drawing]", textPosition, _font.BaseSize, 0, Color.White);
            }
            else
            {
                DrawTexture(_font.Texture, screenWidth / 2 - _font.Texture.Width / 2, 50, Color.Black);
            }

            DrawText("PRESS SPACE to SEE USED SPRITEFONT ", 290, 420, 10, Color.DarkGray);

            EndDrawing();
        }

        public void Unload()
        {
            UnloadTexture(_texture);
            UnloadFont(_font);
        }
    }
}
#endif
