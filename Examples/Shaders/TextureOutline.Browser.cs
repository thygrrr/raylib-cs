#if BROWSER
using Examples;
namespace Examples.Shaders;

public partial class TextureOutline : IExample
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
        public string Name => "Shaders / Texture Outline";

        // NOTE: raylib's web build uses GLSL ES 1.00 (WebGL1), so we target glsl100
        private const int GLSL_VERSION = 100;

        private Texture2D _texture;
        private Shader _shdrOutline;
        private int _outlineSizeLoc;
        private float _outlineSize;

        public void Init()
        {
            _texture = LoadTexture("resources/fudesumi.png");
            _shdrOutline = LoadShader(null, $"resources/shaders/glsl{GLSL_VERSION}/outline.fs");

            _outlineSize = 2.0f;

            // Normalized RED color
            float[] outlineColor = new[] { 1.0f, 0.0f, 0.0f, 1.0f };
            float[] textureSize = { (float)_texture.Width, (float)_texture.Height };

            // Get shader locations
            _outlineSizeLoc = GetShaderLocation(_shdrOutline, "outlineSize");
            int outlineColorLoc = GetShaderLocation(_shdrOutline, "outlineColor");
            int textureSizeLoc = GetShaderLocation(_shdrOutline, "textureSize");

            // Set shader values (they can be changed later)
            Raylib.SetShaderValue(
                _shdrOutline,
                _outlineSizeLoc,
                _outlineSize,
                ShaderUniformDataType.Float
            );
            Raylib.SetShaderValue(
                _shdrOutline,
                outlineColorLoc,
                outlineColor,
                ShaderUniformDataType.Vec4
            );
            Raylib.SetShaderValue(
                _shdrOutline,
                textureSizeLoc,
                textureSize,
                ShaderUniformDataType.Vec2
            );
        }

        public void Update()
        {
            _outlineSize += GetMouseWheelMove();
            if (_outlineSize < 1.0f)
            {
                _outlineSize = 1.0f;
            }

            Raylib.SetShaderValue(
                _shdrOutline,
                _outlineSizeLoc,
                _outlineSize,
                ShaderUniformDataType.Float
            );

            BeginDrawing();

            ClearBackground(Color.RayWhite);

            BeginShaderMode(_shdrOutline);
            DrawTexture(_texture, GetScreenWidth() / 2 - _texture.Width / 2, -30, Color.White);
            EndShaderMode();

            DrawText("Shader-based\ntexture\noutline", 10, 10, 20, Color.Gray);
            DrawText("Scroll mouse wheel to\nchange outline size", 10, 72, 20, Color.Gray);
            DrawText($"Outline size: {(int)_outlineSize} px", 10, 120, 20, Color.Maroon);

            DrawFPS(710, 10);

            EndDrawing();
        }

        public void Unload()
        {
            UnloadTexture(_texture);
            UnloadShader(_shdrOutline);
        }
    }
}
#endif
