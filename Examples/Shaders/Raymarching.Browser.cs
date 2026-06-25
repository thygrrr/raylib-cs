#if BROWSER
using Examples;
namespace Examples.Shaders;

public partial class Raymarching : IExample
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
        public string Name => "Shaders / Raymarching";

        // NOTE: raylib's web build uses GLSL ES 1.00 (WebGL1), so we target glsl100
        private const int GlslVersion = 100;

        // NOTE: The original example sets ResizableWindow via SetConfigFlags before InitWindow.
        // The host owns the window, so we cannot set config flags here; we read the size instead.

        private int _screenWidth;
        private int _screenHeight;

        private Camera3D _camera;
        private Shader _shader;
        private int _viewEyeLoc;
        private int _viewCenterLoc;
        private int _runTimeLoc;
        private int _resolutionLoc;
        private float _runTime;

        public void Init()
        {
            _screenWidth = GetScreenWidth();
            _screenHeight = GetScreenHeight();

            _camera = new();
            _camera.Position = new Vector3(2.5f, 2.5f, 3.0f);
            _camera.Target = new Vector3(0.0f, 0.0f, 0.7f);
            _camera.Up = new Vector3(0.0f, 1.0f, 0.0f);
            _camera.FovY = 65.0f;

            // Load raymarching shader
            // NOTE: Defining 0 (NULL) for vertex shader forces usage of internal default vertex shader
            _shader = LoadShader(null, $"resources/shaders/glsl{GlslVersion}/raymarching.fs");

            // Get shader locations for required uniforms
            _viewEyeLoc = GetShaderLocation(_shader, "viewEye");
            _viewCenterLoc = GetShaderLocation(_shader, "viewCenter");
            _runTimeLoc = GetShaderLocation(_shader, "runTime");
            _resolutionLoc = GetShaderLocation(_shader, "resolution");

            float[] resolution = { (float)_screenWidth, (float)_screenHeight };
            Raylib.SetShaderValue(_shader, _resolutionLoc, resolution, ShaderUniformDataType.Vec2);

            _runTime = 0.0f;
        }

        public void Update()
        {
            // Check if screen is resized
            if (IsWindowResized())
            {
                _screenWidth = GetScreenWidth();
                _screenHeight = GetScreenHeight();
                float[] resolution = new float[] { (float)_screenWidth, (float)_screenHeight };
                Raylib.SetShaderValue(_shader, _resolutionLoc, resolution, ShaderUniformDataType.Vec2);
            }

            UpdateCamera(ref _camera, CameraMode.Free);

            float deltaTime = GetFrameTime();
            _runTime += deltaTime;

            // Set shader required uniform values
            Raylib.SetShaderValue(_shader, _viewEyeLoc, _camera.Position, ShaderUniformDataType.Vec3);
            Raylib.SetShaderValue(_shader, _viewCenterLoc, _camera.Target, ShaderUniformDataType.Vec3);
            Raylib.SetShaderValue(_shader, _runTimeLoc, _runTime, ShaderUniformDataType.Float);

            BeginDrawing();
            ClearBackground(Color.RayWhite);

            // We only draw a white full-screen rectangle,
            // frame is generated in shader using raymarching
            BeginShaderMode(_shader);
            DrawRectangle(0, 0, _screenWidth, _screenHeight, Color.White);
            EndShaderMode();

            DrawText(
                "(c) Raymarching shader by Iñigo Quilez. MIT License.",
                _screenWidth - 280,
                _screenHeight - 20,
                10,
                Color.Black
            );

            EndDrawing();
        }

        public void Unload()
        {
            UnloadShader(_shader);
        }
    }
}
#endif
