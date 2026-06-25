#if BROWSER
using Examples;
namespace Examples.Textures;

public partial class BackgroundScrolling : IExample
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
        public string Name => "Textures / Background Scrolling";

        private const int screenWidth = 800;
        private const int screenHeight = 450;

        // NOTE: Be careful, background width must be equal or bigger than screen width
        // if not, texture should be draw more than two times for scrolling effect
        private Texture2D _background;
        private Texture2D _midground;
        private Texture2D _foreground;

        private float _scrollingBack;
        private float _scrollingMid;
        private float _scrollingFore;

        public void Init()
        {
            _background = LoadTexture("resources/cyberpunk_street_background.png");
            _midground = LoadTexture("resources/cyberpunk_street_midground.png");
            _foreground = LoadTexture("resources/cyberpunk_street_foreground.png");

            _scrollingBack = 0.0f;
            _scrollingMid = 0.0f;
            _scrollingFore = 0.0f;
        }

        public void Update()
        {
            _scrollingBack -= 0.1f;
            _scrollingMid -= 0.5f;
            _scrollingFore -= 1.0f;

            // NOTE: Texture is scaled twice its size, so it sould be considered on scrolling
            if (_scrollingBack <= -_background.Width * 2)
            {
                _scrollingBack = 0;
            }
            if (_scrollingMid <= -_midground.Width * 2)
            {
                _scrollingMid = 0;
            }
            if (_scrollingFore <= -_foreground.Width * 2)
            {
                _scrollingFore = 0;
            }

            BeginDrawing();
            ClearBackground(GetColor(0x052c46ff));

            // Draw background image twice
            // NOTE: Texture is scaled twice its size
            DrawTextureEx(_background, new Vector2(_scrollingBack, 20), 0.0f, 2.0f, Color.White);
            DrawTextureEx(
                _background,
                new Vector2(_background.Width * 2 + _scrollingBack, 20),
                0.0f,
                2.0f,
                Color.White
            );

            // Draw midground image twice
            DrawTextureEx(_midground, new Vector2(_scrollingMid, 20), 0.0f, 2.0f, Color.White);
            DrawTextureEx(_midground, new Vector2(_midground.Width * 2 + _scrollingMid, 20), 0.0f, 2.0f, Color.White);

            // Draw foreground image twice
            DrawTextureEx(_foreground, new Vector2(_scrollingFore, 70), 0.0f, 2.0f, Color.White);
            DrawTextureEx(
                _foreground,
                new Vector2(_foreground.Width * 2 + _scrollingFore, 70),
                0.0f,
                2.0f,
                Color.White
            );

            DrawText("BACKGROUND SCROLLING & PARALLAX", 10, 10, 20, Color.Red);

            int x = screenWidth - 330;
            int y = screenHeight - 20;
            DrawText("(c) Cyberpunk Street Environment by Luis Zuno (@ansimuz)", x, y, 10, Color.RayWhite);

            EndDrawing();
        }

        public void Unload()
        {
            UnloadTexture(_background);
            UnloadTexture(_midground);
            UnloadTexture(_foreground);
        }
    }
}
#endif
