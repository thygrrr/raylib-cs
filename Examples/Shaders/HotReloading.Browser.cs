#if BROWSER
using Examples;
namespace Examples.Shaders;

public partial class HotReloading : IExample
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
        public string Name => "Shaders / Hot Reloading";

        private const int screenWidth = 800;
        private const int screenHeight = 450;

        private Shader _shader;
        private int _resolutionLoc;
        private int _mouseLoc;
        private int _timeLoc;
        private float[] _resolution;
        private float _totalTime;

        public void Init()
        {
            string fragShaderFileName = "resources/shaders/glsl100/reload.fs";

            // Load raymarching shader
            // NOTE: Defining 0 (NULL) for vertex shader forces usage of internal default vertex shader
            _shader = LoadShader(null, fragShaderFileName);

            // Get shader locations for required uniforms
            _resolutionLoc = GetShaderLocation(_shader, "resolution");
            _mouseLoc = GetShaderLocation(_shader, "mouse");
            _timeLoc = GetShaderLocation(_shader, "time");

            _resolution = new[] { (float)screenWidth, (float)screenHeight };
            SetShaderValue(_shader, _resolutionLoc, _resolution, ShaderUniformDataType.Vec2);

            _totalTime = 0.0f;
        }

        public void Update()
        {
            _totalTime += GetFrameTime();
            Vector2 mouse = GetMousePosition();
            float[] mousePos = new[] { mouse.X, mouse.Y };

            // Set shader required uniform values
            SetShaderValue(_shader, _timeLoc, _totalTime, ShaderUniformDataType.Float);
            SetShaderValue(_shader, _mouseLoc, mousePos, ShaderUniformDataType.Vec2);

            BeginDrawing();
            ClearBackground(Color.RayWhite);

            // We only draw a white full-screen rectangle, frame is generated in shader
            BeginShaderMode(_shader);
            DrawRectangle(0, 0, screenWidth, screenHeight, Color.White);
            EndShaderMode();

            DrawText("Shader generates the frame in real time", 10, 10, 10, Color.Black);

            EndDrawing();
        }

        public void Unload()
        {
            UnloadShader(_shader);
        }
    }
}
#endif
