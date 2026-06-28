#if BROWSER
using Examples;
namespace Examples.Shaders;

public partial class Eratosthenes : IExample
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
        public string Name => "Shaders / Eratosthenes";

        // NOTE: raylib's web build uses GLSL ES 1.00 (WebGL1), so we target glsl100
        private const int GlslVersion = 100;

        private const int screenWidth = 800;
        private const int screenHeight = 450;

        private RenderTexture2D _target;
        private Shader _shader;

        public void Init()
        {
            _target = LoadRenderTexture(screenWidth, screenHeight);

            // Load Eratosthenes shader
            // NOTE: Defining 0 (NULL) for vertex shader forces usage of internal default vertex shader
            _shader = LoadShader(null, $"resources/shaders/glsl{GlslVersion}/eratosthenes.fs");
        }

        public void Update()
        {
            // Nothing to do here, everything is happening in the shader

            BeginTextureMode(_target);
            ClearBackground(Color.Black);

            // Draw a rectangle in shader mode to be used as shader canvas
            // NOTE: Rectangle uses font white character texture coordinates,
            // so shader can not be applied here directly because input vertexTexCoord
            // do not represent full screen coordinates (space where want to apply shader)
            DrawRectangle(0, 0, GetScreenWidth(), GetScreenHeight(), Color.Black);
            EndTextureMode();

            BeginDrawing();
            ClearBackground(Color.RayWhite);

            BeginShaderMode(_shader);
            // NOTE: Render texture must be y-flipped due to default OpenGL coordinates (left-bottom)
            DrawTextureRec(
                _target.Texture,
                new Rectangle(0, 0, _target.Texture.Width, -_target.Texture.Height),
                new Vector2(0.0f, 0.0f),
                Color.White
            );
            EndShaderMode();

            EndDrawing();
        }

        public void Unload()
        {
            UnloadShader(_shader);
            UnloadRenderTexture(_target);
        }
    }
}
#endif
