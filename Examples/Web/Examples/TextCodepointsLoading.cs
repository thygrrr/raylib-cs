// Adapted for the browser from Examples/Text/CodepointsLoading.cs
using System.Globalization;

namespace Examples.Web;

public class TextCodepointsLoading : IWebExample
{
    public string Name => "Text / Codepoints Loading";

    // Text to be displayed, must be UTF-8 (save this code file as UTF-8)
    // NOTE: It can contain all the required text for the game,
    // this text will be scanned to get all the required codepoints
    private const string Text =
        "いろはにほへと　ちりぬるを\nわかよたれそ　つねならむ\nうゐのおくやま　けふこえて\nあさきゆめみし　ゑひもせす";

    private List<int> _codepoints;
    private int[] _codepointsNoDuplicates;
    private Font _font;
    private bool _showFontAtlas;

    public void Init()
    {
        // Get codepoints from text
        _codepoints = GetCodePoints(Text);

        // Remove duplicate codepoints to generate smaller font atlas
        _codepointsNoDuplicates = _codepoints.Distinct().ToArray();

        // Load font containing all the provided codepoint glyphs
        // A texture font atlas is automatically generated
        _font = LoadFontEx(
            "resources/fonts/DotGothic16-Regular.ttf",
            36,
            _codepointsNoDuplicates,
            _codepointsNoDuplicates.Length
        );

        // Set bilinear scale filter for better font scaling
        SetTextureFilter(_font.Texture, TextureFilter.Bilinear);

        _showFontAtlas = false;
    }

    public void Update()
    {
        if (IsKeyPressed(KeyboardKey.Space))
        {
            _showFontAtlas = !_showFontAtlas;
        }

        BeginDrawing();

        ClearBackground(Color.RayWhite);

        DrawRectangle(0, 0, GetScreenWidth(), 70, Color.Black);
        DrawText($"Total codepoints contained in provided text: {_codepoints.Count}", 10, 10, 20, Color.Green);
        DrawText(
            $"Total codepoints required for font atlas (duplicates excluded): {_codepointsNoDuplicates.Length}",
            10,
            40,
            20,
            Color.Green
        );

        if (_showFontAtlas)
        {
            // Draw generated font texture atlas containing provided codepoints
            DrawTexture(_font.Texture, 150, 100, Color.Black);
            DrawRectangleLines(150, 100, _font.Texture.Width, _font.Texture.Height, Color.Black);
        }
        else
        {
            // Draw provided text with loaded font, containing all required codepoint glyphs
            DrawTextEx(_font, Text, new Vector2(160, 110), 48, 5, Color.Black);
        }

        DrawText("Press SPACE to toggle font atlas view!", 10, GetScreenHeight() - 30, 20, Color.Gray);

        EndDrawing();
    }

    public void Unload()
    {
        UnloadFont(_font);
    }

    private static List<int> GetCodePoints(string text)
    {
        List<int> codePoints = new();

        TextElementEnumerator enumerator = StringInfo.GetTextElementEnumerator(text);

        while (enumerator.MoveNext())
        {
            int codePoint = char.ConvertToUtf32(enumerator.Current.ToString(), 0);
            codePoints.Add(codePoint);
        }

        return codePoints;
    }
}
