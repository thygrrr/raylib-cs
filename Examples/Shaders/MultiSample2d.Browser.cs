#if BROWSER
using Examples;
namespace Examples.Shaders;

public partial class MultiSample2d : IExample
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
        public string Name => "Shaders / Multi Sample 2D";

        private Texture2D _texRed;
        private Texture2D _texBlue;
        private Shader _shader;
        private int _texBlueLoc;
        private int _dividerLoc;
        private float _dividerValue;

        public void Init()
        {
            Image imRed = GenImageColor(800, 450, new Color(255, 0, 0, 255));
            _texRed = LoadTextureFromImage(imRed);
            UnloadImage(imRed);

            Image imBlue = GenImageColor(800, 450, new Color(0, 0, 255, 255));
            _texBlue = LoadTextureFromImage(imBlue);
            UnloadImage(imBlue);

            // NOTE: Using GLSL 100 shader version for WebGL1 (OpenGL ES 2.0)
            _shader = LoadShader(null, "resources/shaders/glsl100/color_mix.fs");

            // Get an additional sampler2D location to be enabled on drawing
            _texBlueLoc = GetShaderLocation(_shader, "texture1");

            // Get shader uniform for divider
            _dividerLoc = GetShaderLocation(_shader, "divider");
            _dividerValue = 0.5f;
        }

        public void Update()
        {
            if (IsKeyDown(KeyboardKey.Right))
            {
                _dividerValue += 0.01f;
            }
            else if (IsKeyDown(KeyboardKey.Left))
            {
                _dividerValue -= 0.01f;
            }

            if (_dividerValue < 0.0f)
            {
                _dividerValue = 0.0f;
            }
            else if (_dividerValue > 1.0f)
            {
                _dividerValue = 1.0f;
            }

            Raylib.SetShaderValue(_shader, _dividerLoc, _dividerValue, ShaderUniformDataType.Float);

            BeginDrawing();
            ClearBackground(Color.RayWhite);

            BeginShaderMode(_shader);

            // WARNING: Additional textures (sampler2D) are enabled for ALL draw calls in the batch,
            // but EndShaderMode() forces batch drawing and resets active textures, this way
            // other textures (sampler2D) can be activated on consequent drawings (if required)
            // The downside of this approach is that SetShaderValue() must be called inside the loop,
            // to be set again after every EndShaderMode() reset
            SetShaderValueTexture(_shader, _texBlueLoc, _texBlue);

            // We are drawing texRed using default [sampler2D texture0] but
            // an additional texture units is enabled for texBlue [sampler2D texture1]
            DrawTexture(_texRed, 0, 0, Color.White);

            EndShaderMode();

            int y = GetScreenHeight() - 40;
            DrawText("Use KEY_LEFT/KEY_RIGHT to move texture mixing in shader!", 80, y, 20, Color.RayWhite);

            EndDrawing();
        }

        public void Unload()
        {
            UnloadShader(_shader);
            UnloadTexture(_texRed);
            UnloadTexture(_texBlue);
        }
    }
}
#endif
