#if BROWSER
using Examples;
using System;

namespace Examples.Text;

public partial class FontSdf : IExample
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
        public string Name => "Text / Font SDF";

        private const string msg = "Signed Distance Fields";

        private Font _fontDefault;
        private Font _fontSDF;
        private Shader _shader;

        private Vector2 _fontPosition;
        private float _fontSize;
        // 0 - fontDefault, 1 - fontSDF
        private int _currentFont;

        public void Init()
        {
            // NOTE: Textures/Fonts MUST be loaded after Window initialization (OpenGL context is required)

            // Loading file to memory
            int fileSize = 0;
            byte* fileData = LoadFileData("resources/fonts/anonymous_pro_bold.ttf", ref fileSize);

            // Build the fonts in locals first: taking the address of a struct's field (&font.GlyphCount,
            // &font.Recs) is only allowed for a stack local, not a heap field. Assign to the fields after.

            // Default font generation from TTF font
            Font fontDefault = new();
            fontDefault.BaseSize = 16;
            fontDefault.GlyphCount = 95;

            // Loading font data from memory data
            // Parameters > font size: 16, no chars array provided (0), chars count: 95 (autogenerate chars array)
            fontDefault.Glyphs = LoadFontData(fileData, fileSize, 16, null, 95, FontType.Default, &fontDefault.GlyphCount);
            // Parameters > chars count: 95, font size: 16, chars padding in image: 4 px, pack method: 0 (default)
            Image atlas = GenImageFontAtlas(fontDefault.Glyphs, &fontDefault.Recs, 95, 16, 4, 0);
            fontDefault.Texture = LoadTextureFromImage(atlas);
            UnloadImage(atlas);
            _fontDefault = fontDefault;

            // SDF font generation from TTF font
            Font fontSDF = new();
            fontSDF.BaseSize = 16;
            fontSDF.GlyphCount = 95;
            // Parameters > font size: 16, no chars array provided (0), chars count: 0 (defaults to 95)
            fontSDF.Glyphs = LoadFontData(fileData, fileSize, 16, null, 0, FontType.Sdf, &fontSDF.GlyphCount);
            // Parameters > chars count: 95, font size: 16, chars padding in image: 0 px, pack method: 1 (Skyline algorythm)
            atlas = GenImageFontAtlas(fontSDF.Glyphs, &fontSDF.Recs, 95, 16, 0, 1);
            fontSDF.Texture = LoadTextureFromImage(atlas);
            UnloadImage(atlas);
            _fontSDF = fontSDF;

            // Free memory from loaded file
            UnloadFileData(fileData);

            // Load SDF required shader (we use default vertex shader)
            _shader = LoadShader(null, "resources/shaders/glsl100/sdf.fs");
            // Required for SDF font
            SetTextureFilter(_fontSDF.Texture, TextureFilter.Bilinear);

            _fontPosition = new(40, GetScreenHeight() / 2 - 50);
            _fontSize = 16.0f;
            _currentFont = 0;
        }

        public void Update()
        {
            Vector2 textSize = new(0.0f);

            _fontSize += GetMouseWheelMove() * 8.0f;

            if (_fontSize < 6)
            {
                _fontSize = 6;
            }

            if (IsKeyDown(KeyboardKey.Space))
            {
                _currentFont = 1;
            }
            else
            {
                _currentFont = 0;
            }

            if (_currentFont == 0)
            {
                textSize = MeasureTextEx(_fontDefault, msg, _fontSize, 0);
            }
            else
            {
                textSize = MeasureTextEx(_fontSDF, msg, _fontSize, 0);
            }

            _fontPosition.X = GetScreenWidth() / 2 - textSize.X / 2;
            _fontPosition.Y = GetScreenHeight() / 2 - textSize.Y / 2 + 80;

            BeginDrawing();
            ClearBackground(Color.RayWhite);

            if (_currentFont == 1)
            {
                // NOTE: SDF fonts require a custom SDf shader to compute fragment color
                BeginShaderMode(_shader);
                DrawTextEx(_fontSDF, msg, _fontPosition, _fontSize, 0, Color.Black);
                EndShaderMode();

                DrawTexture(_fontSDF.Texture, 10, 10, Color.Black);
            }
            else
            {
                DrawTextEx(_fontDefault, msg, _fontPosition, _fontSize, 0, Color.Black);
                DrawTexture(_fontDefault.Texture, 10, 10, Color.Black);
            }

            if (_currentFont == 1)
            {
                DrawText("SDF!", 320, 20, 80, Color.Red);
            }
            else
            {
                DrawText("default font", 315, 40, 30, Color.Gray);
            }

            DrawText("FONT SIZE: 16.0", GetScreenWidth() - 240, 20, 20, Color.DarkGray);
            DrawText($"RENDER SIZE: {_fontSize:2F}", GetScreenWidth() - 240, 50, 20, Color.DarkGray);
            DrawText("Use MOUSE WHEEL to SCALE TEXT!", GetScreenWidth() - 240, 90, 10, Color.DarkGray);

            DrawText("PRESS SPACE to USE SDF FONT VERSION!", 340, GetScreenHeight() - 30, 20, Color.Maroon);

            EndDrawing();
        }

        public void Unload()
        {
            UnloadFont(_fontDefault);
            UnloadFont(_fontSDF);
            UnloadShader(_shader);
        }
    }
}
#endif
