// Adapted for the browser from Examples/Shaders/PaletteSwitch.cs
namespace Examples.Web;

public class ShadersPaletteSwitch : IWebExample
{
    public string Name => "Shaders / Palette Switch";

    // NOTE: raylib's web build uses GLSL ES 1.00 (WebGL1), so we target glsl100
    private const int GlslVersion = 100;
    private const int ColorsPerPalette = 8;
    private const int VALUES_PER_COLOR = 3;
    private const int screenHeight = 540;

    private static readonly int[][] Palettes = new int[][] {
            // 3-BIT RGB
            new int[] {
                0, 0, 0,
                255, 0, 0,
                0, 255, 0,
                0, 0, 255,
                0, 255, 255,
                255, 0, 255,
                255, 255, 0,
                255, 255, 255,
            },
            // AMMO-8 (GameBoy-like)
            new int[] {
                4, 12, 6,
                17, 35, 24,
                30, 58, 41,
                48, 93, 66,
                77, 128, 97,
                137, 162, 87,
                190, 220, 127,
                238, 255, 204,
            },
            // RKBV (2-strip film)
            new int[] {
                21, 25, 26,
                138, 76, 88,
                217, 98, 117,
                230, 184, 193,
                69, 107, 115,
                75, 151, 166,
                165, 189, 194,
                255, 245, 247,
            }
        };

    private static readonly string[] PaletteText = new string[] {
            "3-BIT RGB",
            "AMMO-8 (GameBoy-like)",
            "RKBV (2-strip film)"
        };

    private Shader _shader;
    private int _paletteLoc;
    private int _currentPalette;
    private int _lineHeight;

    public void Init()
    {
        // Load shader to be used on some parts drawing
        // NOTE 1: Using GLSL 100 shader version for WebGL1 (OpenGL ES 2.0)
        // NOTE 2: Defining 0 (NULL) for vertex shader forces usage of internal default vertex shader
        _shader = LoadShader(null, $"resources/shaders/glsl{GlslVersion}/palette_switch.fs");

        // Get variable (uniform) location on the shader to connect with the program
        // NOTE: If uniform variable could not be found in the shader, function returns -1
        _paletteLoc = GetShaderLocation(_shader, "palette");

        _currentPalette = 0;
        _lineHeight = screenHeight / ColorsPerPalette;
    }

    public void Update()
    {
        if (IsKeyPressed(KeyboardKey.Right))
        {
            _currentPalette++;
        }
        else if (IsKeyPressed(KeyboardKey.Left))
        {
            _currentPalette--;
        }

        if (_currentPalette >= Palettes.Length)
        {
            _currentPalette = 0;
        }
        else if (_currentPalette < 0)
        {
            _currentPalette = Palettes.Length - 1;
        }

        // Send new value to the shader to be used on drawing.
        // NOTE: We are sending RGB triplets w/o the alpha channel
        Raylib.SetShaderValueV(
            _shader,
            _paletteLoc,
            Palettes[_currentPalette],
            ShaderUniformDataType.IVec3,
            ColorsPerPalette
        );

        BeginDrawing();
        ClearBackground(Color.RayWhite);

        BeginShaderMode(_shader);

        for (int i = 0; i < ColorsPerPalette; i++)
        {
            // Draw horizontal screen-wide rectangles with increasing "palette index"
            // The used palette index is encoded in the RGB components of the pixel
            DrawRectangle(0, _lineHeight * i, GetScreenWidth(), _lineHeight, new Color(i, i, i, 255));
        }

        EndShaderMode();

        DrawText("< >", 10, 10, 30, Color.DarkBlue);
        DrawText("CURRENT PALETTE:", 60, 15, 20, Color.RayWhite);
        DrawText(PaletteText[_currentPalette], 300, 15, 20, Color.Red);

        DrawFPS(700, 15);

        EndDrawing();
    }

    public void Unload()
    {
        UnloadShader(_shader);
    }
}
