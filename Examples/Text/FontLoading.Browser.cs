#if BROWSER
using Examples;
namespace Examples.Text;

public partial class FontLoading : IExample
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
        public string Name => "Text / Font Loading";

        // Define characters to draw
        // NOTE: raylib supports UTF-8 encoding, following list is actually codified as UTF8 internally
        private const string Msg = "!\"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHI\nJKLMNOPQRSTUVWXYZ[]^_`abcdefghijklmn\nopqrstuvwxyz{|}~¿ÀÁÂÃÄÅÆÇÈÉÊËÌÍÎÏÐÑÒÓ\nÔÕÖ×ØÙÚÛÜÝÞßàáâãäåæçèéêëìíîïðñòóôõö÷\nøùúûüýþÿ";

        private Font _fontBm;
        private Font _fontTtf;
        private bool _useTtf;

        public void Init()
        {
            // NOTE: Textures/Fonts MUST be loaded after Window initialization (OpenGL context is required)

            // BMFont (AngelCode) : Font data and image atlas have been generated using external program
            _fontBm = LoadFont("resources/fonts/pixantiqua.fnt");

            // TTF font : Font data and atlas are generated directly from TTF
            // NOTE: We define a font base size of 32 pixels tall and up-to 250 characters
            _fontTtf = LoadFontEx("resources/fonts/pixantiqua.ttf", 32, null, 250);

            _useTtf = false;
        }

        public void Update()
        {
            if (IsKeyDown(KeyboardKey.Space))
            {
                _useTtf = true;
            }
            else
            {
                _useTtf = false;
            }

            BeginDrawing();
            ClearBackground(Color.RayWhite);

            DrawText("Hold SPACE to use TTF generated font", 20, 20, 20, Color.LightGray);

            if (!_useTtf)
            {
                DrawTextEx(_fontBm, Msg, new Vector2(20.0f, 100.0f), _fontBm.BaseSize, 2, Color.Maroon);
                DrawText("Using BMFont (Angelcode) imported", 20, GetScreenHeight() - 30, 20, Color.Gray);
            }
            else
            {
                DrawTextEx(_fontTtf, Msg, new Vector2(20.0f, 100.0f), _fontTtf.BaseSize, 2, Color.Lime);
                DrawText("Using TTF font generated", 20, GetScreenHeight() - 30, 20, Color.Gray);
            }

            EndDrawing();
        }

        public void Unload()
        {
            UnloadFont(_fontBm);
            UnloadFont(_fontTtf);
        }
    }
}
#endif
