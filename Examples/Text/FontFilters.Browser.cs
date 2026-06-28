#if BROWSER
using Examples;
namespace Examples.Text;

public partial class FontFilters : IExample
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
        public string Name => "Text / Font Filters";

        private const int screenWidth = 800;
        private const int screenHeight = 450;

        private const string Msg = "Loaded Font";

        private Font _font;
        private float _fontSize;
        private Vector2 _fontPosition;
        private Vector2 _textSize;
        private TextureFilter _currentFontFilter;

        public void Init()
        {
            // NOTE: Textures/Fonts MUST be loaded after Window initialization (OpenGL context is required)

            // TTF Font loading with custom generation parameters
            _font = LoadFontEx("resources/fonts/KAISG.ttf", 96, null, 0);

            // Generate mipmap levels to use trilinear filtering
            // NOTE: On 2D drawing it won't be noticeable, it looks like FILTER_BILINEAR
            GenTextureMipmaps(ref _font.Texture);

            _fontSize = _font.BaseSize;
            _fontPosition = new(40, screenHeight / 2 - 80);
            _textSize = new(0.0f, 0.0f);

            // Setup texture scaling filter
            SetTextureFilter(_font.Texture, TextureFilter.Point);
            _currentFontFilter = TextureFilter.Point;
        }

        public void Update()
        {
            _fontSize += GetMouseWheelMove() * 4.0f;

            // Choose font texture filter method
            if (IsKeyPressed(KeyboardKey.One))
            {
                SetTextureFilter(_font.Texture, TextureFilter.Point);
                _currentFontFilter = TextureFilter.Point;
            }
            else if (IsKeyPressed(KeyboardKey.Two))
            {
                SetTextureFilter(_font.Texture, TextureFilter.Bilinear);
                _currentFontFilter = TextureFilter.Bilinear;
            }
            else if (IsKeyPressed(KeyboardKey.Three))
            {
                // NOTE: Trilinear filter won't be noticed on 2D drawing
                SetTextureFilter(_font.Texture, TextureFilter.Trilinear);
                _currentFontFilter = TextureFilter.Trilinear;
            }

            _textSize = MeasureTextEx(_font, Msg, _fontSize, 0);

            if (IsKeyDown(KeyboardKey.Left))
            {
                _fontPosition.X -= 10;
            }
            else if (IsKeyDown(KeyboardKey.Right))
            {
                _fontPosition.X += 10;
            }

            // NOTE: drag-and-drop font loading is not supported in the browser host; default loaded font is kept.

            BeginDrawing();
            ClearBackground(Color.RayWhite);

            DrawText("Use mouse wheel to change font size", 20, 20, 10, Color.Gray);
            DrawText("Use KEY_RIGHT and KEY_LEFT to move text", 20, 40, 10, Color.Gray);
            DrawText("Use 1, 2, 3 to change texture filter", 20, 60, 10, Color.Gray);
            DrawText("Drop a new TTF font for dynamic loading", 20, 80, 10, Color.DarkGray);

            DrawTextEx(_font, Msg, _fontPosition, _fontSize, 0, Color.Black);

            // TODO: It seems texSize measurement is not accurate due to chars offsets...
            //DrawRectangleLines((int)_fontPosition.X, (int)_fontPosition.Y, (int)_textSize.X, (int)_textSize.Y, Color.Red);

            DrawRectangle(0, screenHeight - 80, screenWidth, 80, Color.LightGray);
            DrawText($"Font size: {_fontSize:00.00}", 20, screenHeight - 50, 10, Color.DarkGray);
            DrawText($"Text size: [{_textSize.X:00.00}, {_textSize.Y:00.00}]", 20, screenHeight - 30, 10, Color.DarkGray);
            DrawText("CURRENT TEXTURE FILTER:", 250, 400, 20, Color.Gray);

            if (_currentFontFilter == TextureFilter.Point)
            {
                DrawText("POINT", 570, 400, 20, Color.Black);
            }
            else if (_currentFontFilter == TextureFilter.Bilinear)
            {
                DrawText("BILINEAR", 570, 400, 20, Color.Black);
            }
            else if (_currentFontFilter == TextureFilter.Trilinear)
            {
                DrawText("TRILINEAR", 570, 400, 20, Color.Black);
            }

            EndDrawing();
        }

        public void Unload()
        {
            UnloadFont(_font);
        }
    }
}
#endif
