#if BROWSER
using Examples;
namespace Examples.Shaders;

public partial class TextureWaves : IExample
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
        public string Name => "Shaders / Texture Waves";

        // NOTE: raylib's web build uses GLSL ES 1.00 (WebGL1), so we target glsl100
        private const int GlslVersion = 100;

        private Texture2D _texture;
        private Shader _shader;
        private int _secondsLoc;
        private float _seconds;

        public void Init()
        {
            // Load texture texture to apply shaders
            _texture = LoadTexture("resources/space.png");

            // Load shader and setup location points and values
            _shader = LoadShader(null, $"resources/shaders/glsl{GlslVersion}/wave.fs");

            _secondsLoc = GetShaderLocation(_shader, "secondes");
            int freqXLoc = GetShaderLocation(_shader, "freqX");
            int freqYLoc = GetShaderLocation(_shader, "freqY");
            int ampXLoc = GetShaderLocation(_shader, "ampX");
            int ampYLoc = GetShaderLocation(_shader, "ampY");
            int speedXLoc = GetShaderLocation(_shader, "speedX");
            int speedYLoc = GetShaderLocation(_shader, "speedY");

            // Shader uniform values that can be updated at any time
            float freqX = 25.0f;
            float freqY = 25.0f;
            float ampX = 5.0f;
            float ampY = 5.0f;
            float speedX = 8.0f;
            float speedY = 8.0f;

            float[] screenSize = { (float)GetScreenWidth(), (float)GetScreenHeight() };
            Raylib.SetShaderValue(
                _shader,
                GetShaderLocation(_shader, "size"),
                screenSize,
                ShaderUniformDataType.Vec2
            );
            Raylib.SetShaderValue(_shader, freqXLoc, freqX, ShaderUniformDataType.Float);
            Raylib.SetShaderValue(_shader, freqYLoc, freqY, ShaderUniformDataType.Float);
            Raylib.SetShaderValue(_shader, ampXLoc, ampX, ShaderUniformDataType.Float);
            Raylib.SetShaderValue(_shader, ampYLoc, ampY, ShaderUniformDataType.Float);
            Raylib.SetShaderValue(_shader, speedXLoc, speedX, ShaderUniformDataType.Float);
            Raylib.SetShaderValue(_shader, speedYLoc, speedY, ShaderUniformDataType.Float);

            _seconds = 0.0f;
        }

        public void Update()
        {
            _seconds += GetFrameTime();

            Raylib.SetShaderValue(_shader, _secondsLoc, _seconds, ShaderUniformDataType.Float);

            BeginDrawing();
            ClearBackground(Color.RayWhite);

            BeginShaderMode(_shader);

            DrawTexture(_texture, 0, 0, Color.White);
            DrawTexture(_texture, _texture.Width, 0, Color.White);

            EndShaderMode();

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
