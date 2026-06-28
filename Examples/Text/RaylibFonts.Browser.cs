#if BROWSER
using Examples;
namespace Examples.Text;

public partial class RaylibFonts : IExample
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
        public string Name => "Text / Raylib Fonts";

        private const int screenWidth = 800;
        private const int screenHeight = 450;
        private const int MaxFonts = 8;

        private Font[] _fonts;
        private string[] _messages;
        private int[] _spacings;
        private Vector2[] _positions;
        private Color[] _colors;

        public void Init()
        {
            // NOTE: Textures MUST be loaded after Window initialization (OpenGL context is required)
            _fonts = new Font[MaxFonts];

            _fonts[0] = LoadFont("resources/fonts/alagard.png");
            _fonts[1] = LoadFont("resources/fonts/pixelplay.png");
            _fonts[2] = LoadFont("resources/fonts/mecha.png");
            _fonts[3] = LoadFont("resources/fonts/setback.png");
            _fonts[4] = LoadFont("resources/fonts/romulus.png");
            _fonts[5] = LoadFont("resources/fonts/pixantiqua.png");
            _fonts[6] = LoadFont("resources/fonts/alpha_beta.png");
            _fonts[7] = LoadFont("resources/fonts/jupiter_crash.png");

            _messages = new string[MaxFonts] {
                "ALAGARD FONT designed by Hewett Tsoi",
                "PIXELPLAY FONT designed by Aleksander Shevchuk",
                "MECHA FONT designed by Captain Falcon",
                "SETBACK FONT designed by Brian Kent (AEnigma)",
                "ROMULUS FONT designed by Hewett Tsoi",
                "PIXANTIQUA FONT designed by Gerhard Grossmann",
                "ALPHA_BETA FONT designed by Brian Kent (AEnigma)",
                "JUPITER_CRASH FONT designed by Brian Kent (AEnigma)"
            };

            _spacings = new int[MaxFonts] { 2, 4, 8, 4, 3, 4, 4, 1 };
            _positions = new Vector2[MaxFonts];

            for (int i = 0; i < MaxFonts; i++)
            {
                float halfWidth = MeasureTextEx(_fonts[i], _messages[i], _fonts[i].BaseSize * 2, _spacings[i]).X / 2;
                _positions[i].X = screenWidth / 2 - halfWidth;
                _positions[i].Y = 60 + _fonts[i].BaseSize + 45 * i;
            }

            // Small Y position corrections
            _positions[3].Y += 8;
            _positions[4].Y += 2;
            _positions[7].Y -= 8;

            _colors = new Color[MaxFonts] {
                Color.Maroon,
                Color.Orange,
                Color.DarkGreen,
                Color.DarkBlue,
                Color.DarkPurple,
                Color.Lime,
                Color.Gold,
                Color.Red
            };
        }

        public void Update()
        {
            BeginDrawing();
            ClearBackground(Color.RayWhite);

            DrawText("free fonts included with raylib", 250, 20, 20, Color.DarkGray);
            DrawLine(220, 50, 590, 50, Color.DarkGray);

            for (int i = 0; i < MaxFonts; i++)
            {
                DrawTextEx(_fonts[i], _messages[i], _positions[i], _fonts[i].BaseSize * 2, _spacings[i], _colors[i]);
            }

            EndDrawing();
        }

        public void Unload()
        {
            for (int i = 0; i < MaxFonts; i++)
            {
                UnloadFont(_fonts[i]);
            }
        }
    }
}
#endif
