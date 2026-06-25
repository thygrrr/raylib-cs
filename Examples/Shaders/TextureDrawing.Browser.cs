#if BROWSER
using Examples;
namespace Examples.Shaders;

public partial class TextureDrawing : IExample
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
        public string Name => "Shaders / Texture Drawing";

        // NOTE: raylib's web build uses GLSL ES 1.00 (WebGL1), so we target glsl100
        private const int GlslVersion = 100;

        private Texture2D _texture;
        private Shader _shader;
        private int _timeLoc;

        public void Init()
        {
            // Load blank texture to fill on shader
            Image imBlank = GenImageColor(1024, 1024, Color.Blank);
            _texture = LoadTextureFromImage(imBlank);
            UnloadImage(imBlank);

            // NOTE: Using GLSL 100 shader version for WebGL1 (OpenGL ES 2.0)
            _shader = LoadShader(null, $"resources/shaders/glsl{GlslVersion}/cubes_panning.fs");

            float time = 0.0f;
            _timeLoc = GetShaderLocation(_shader, "uTime");
            Raylib.SetShaderValue(_shader, _timeLoc, time, ShaderUniformDataType.Float);
        }

        public void Update()
        {
            float time = (float)GetTime();
            Raylib.SetShaderValue(_shader, _timeLoc, time, ShaderUniformDataType.Float);

            BeginDrawing();
            ClearBackground(Color.RayWhite);

            // Enable our custom shader for next shapes/textures drawings
            BeginShaderMode(_shader);

            // Drawing blank texture, all magic happens on shader
            DrawTexture(_texture, 0, 0, Color.White);

            // Disable our custom shader, return to default shader
            EndShaderMode();

            DrawText("BACKGROUND is PAINTED and ANIMATED on SHADER!", 10, 10, 20, Color.Maroon);

            EndDrawing();
        }

        public void Unload()
        {
            UnloadShader(_shader);
            UnloadTexture(_texture);
        }
    }
}
#endif
