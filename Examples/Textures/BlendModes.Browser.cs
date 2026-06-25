#if BROWSER
using Examples;
namespace Examples.Textures;

public partial class BlendModes : IExample
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
        public string Name => "Textures / Blend Modes";

        private const int screenWidth = 800;
        private const int screenHeight = 450;

        private const int blendCountMax = 4;

        private Texture2D _bgTexture;
        private Texture2D _fgTexture;

        private BlendMode _blendMode;

        public void Init()
        {
            // NOTE: Textures MUST be loaded after Window initialization (OpenGL context is required)
            Image bgImage = LoadImage("resources/cyberpunk_street_background.png");
            _bgTexture = LoadTextureFromImage(bgImage);

            Image fgImage = LoadImage("resources/cyberpunk_street_foreground.png");
            _fgTexture = LoadTextureFromImage(fgImage);

            // Once image has been converted to texture and uploaded to VRAM, it can be unloaded from RAM
            UnloadImage(bgImage);
            UnloadImage(fgImage);

            _blendMode = 0;
        }

        public void Update()
        {
            if (IsKeyPressed(KeyboardKey.Space))
            {
                if ((int)_blendMode >= (blendCountMax - 1))
                {
                    _blendMode = 0;
                }
                else
                {
                    _blendMode++;
                }
            }

            BeginDrawing();
            ClearBackground(Color.RayWhite);

            int bgX = screenWidth / 2 - _bgTexture.Width / 2;
            int bgY = screenHeight / 2 - _bgTexture.Height / 2;
            DrawTexture(_bgTexture, bgX, bgY, Color.White);

            // Apply the blend mode and then draw the foreground texture
            BeginBlendMode(_blendMode);
            int fgX = screenWidth / 2 - _fgTexture.Width / 2;
            int fgY = screenHeight / 2 - _fgTexture.Height / 2;
            DrawTexture(_fgTexture, fgX, fgY, Color.White);
            EndBlendMode();

            // Draw the texts
            DrawText("Press SPACE to change blend modes.", 310, 350, 10, Color.Gray);

            switch (_blendMode)
            {
                case BlendMode.Alpha:
                    DrawText("Current: BLEND_ALPHA", (screenWidth / 2) - 60, 370, 10, Color.Gray);
                    break;
                case BlendMode.Additive:
                    DrawText("Current: BLEND_ADDITIVE", (screenWidth / 2) - 60, 370, 10, Color.Gray);
                    break;
                case BlendMode.Multiplied:
                    DrawText("Current: BLEND_MULTIPLIED", (screenWidth / 2) - 60, 370, 10, Color.Gray);
                    break;
                case BlendMode.AddColors:
                    DrawText("Current: BLEND_ADD_COLORS", (screenWidth / 2) - 60, 370, 10, Color.Gray);
                    break;
                default:
                    break;
            }

            string text = "(c) Cyberpunk Street Environment by Luis Zuno (@ansimuz)";
            DrawText(text, screenWidth - 330, screenHeight - 20, 10, Color.Gray);

            EndDrawing();
        }

        public void Unload()
        {
            UnloadTexture(_fgTexture);
            UnloadTexture(_bgTexture);
        }
    }
}
#endif
