#if BROWSER
using Examples;
namespace Examples.Text;

public partial class FontSpritefont : IExample
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
        public string Name => "Text / Font Spritefont";

        private const int screenWidth = 800;
        private const int screenHeight = 450;

        private const string Msg1 = "THIS IS A custom SPRITE FONT...";
        private const string Msg2 = "...and this is ANOTHER CUSTOM font...";
        private const string Msg3 = "...and a THIRD one! GREAT! :D";

        private Font _font1;
        private Font _font2;
        private Font _font3;

        private Vector2 _fontPosition1;
        private Vector2 _fontPosition2;
        private Vector2 _fontPosition3;

        public void Init()
        {
            // NOTE: Textures/Fonts MUST be loaded after Window initialization (OpenGL context is required)
            _font1 = LoadFont("resources/fonts/custom_mecha.png");
            _font2 = LoadFont("resources/fonts/custom_alagard.png");
            _font3 = LoadFont("resources/fonts/custom_jupiter_crash.png");

            _fontPosition1 = new(
                screenWidth / 2 - MeasureTextEx(_font1, Msg1, _font1.BaseSize, -3).X / 2,
                screenHeight / 2 - _font1.BaseSize / 2 - 80
            );

            _fontPosition2 = new(
                screenWidth / 2 - MeasureTextEx(_font2, Msg2, _font2.BaseSize, -2).X / 2,
                screenHeight / 2 - _font2.BaseSize / 2 - 10
            );

            _fontPosition3 = new(
                screenWidth / 2 - MeasureTextEx(_font3, Msg3, _font3.BaseSize, 2).X / 2,
                screenHeight / 2 - _font3.BaseSize / 2 + 50
            );
        }

        public void Update()
        {
            BeginDrawing();
            ClearBackground(Color.RayWhite);

            DrawTextEx(_font1, Msg1, _fontPosition1, _font1.BaseSize, -3, Color.White);
            DrawTextEx(_font2, Msg2, _fontPosition2, _font2.BaseSize, -2, Color.White);
            DrawTextEx(_font3, Msg3, _fontPosition3, _font3.BaseSize, 2, Color.White);

            EndDrawing();
        }

        public void Unload()
        {
            UnloadFont(_font1);
            UnloadFont(_font2);
            UnloadFont(_font3);
        }
    }
}
#endif
