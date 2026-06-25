// Adapted for the browser from Examples/Shaders/HotReloading.cs
// NOTE: retargeted to GLSL ES 1.00 (WebGL1): glsl100 shader path.
// NOTE: Hot-reloading (file mtime watching + re-load) is meaningless in the browser sandbox,
//       so the watch/reload logic is dropped; the shader is loaded once and rendered statically.
namespace Examples.Web;

public class ShadersHotReloading : IWebExample
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
