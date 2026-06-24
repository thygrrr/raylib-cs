// Adapted for the browser from Examples/Shaders/CustomUniform.cs
namespace Examples.Web;

public class ShadersCustomUniform : IWebExample
{
    public string Name => "Shaders / Custom Uniform";

    private const int screenWidth = 960;
    private const int screenHeight = 540;

    // NOTE: The original example enables MSAA 4x via SetConfigFlags before InitWindow.
    // The host owns the window, so we cannot set config flags here.

    private Camera3D _camera;
    private Model _model;
    private Texture2D _texture;
    private Vector3 _position;
    private Shader _shader;
    private int _swirlCenterLoc;
    private float[] _swirlCenter;
    private RenderTexture2D _target;

    public void Init()
    {
        // Define the camera to look into our 3d world
        _camera = new();
        _camera.Position = new Vector3(8.0f, 8.0f, 8.0f);
        _camera.Target = new Vector3(0.0f, 1.5f, 0.0f);
        _camera.Up = new Vector3(0.0f, 1.0f, 0.0f);
        _camera.FovY = 45.0f;
        _camera.Projection = CameraProjection.Perspective;

        _model = LoadModel("resources/models/obj/barracks.obj");
        _texture = LoadTexture("resources/models/obj/barracks_diffuse.png");

        // Set model diffuse texture
        Raylib.SetMaterialTexture(ref _model, 0, MaterialMapIndex.Albedo, ref _texture);

        _position = new(0.0f, 0.0f, 0.0f);

        // Load postpro shader
        // NOTE: Using GLSL 100 shader version for WebGL1 (OpenGL ES 2.0)
        _shader = LoadShader("resources/shaders/glsl100/base.vs",
                             "resources/shaders/glsl100/swirl.fs");

        // Get variable (uniform) location on the shader to connect with the program
        // NOTE: If uniform variable could not be found in the shader, function returns -1
        _swirlCenterLoc = GetShaderLocation(_shader, "center");

        _swirlCenter = new float[2] { (float)screenWidth / 2, (float)screenHeight / 2 };

        // Create a RenderTexture2D to be used for render to texture
        _target = LoadRenderTexture(screenWidth, screenHeight);
    }

    public void Update()
    {
        Vector2 mousePosition = GetMousePosition();

        _swirlCenter[0] = mousePosition.X;
        _swirlCenter[1] = screenHeight - mousePosition.Y;

        // Send new value to the shader to be used on drawing
        Raylib.SetShaderValue(_shader, _swirlCenterLoc, _swirlCenter, ShaderUniformDataType.Vec2);

        UpdateCamera(ref _camera, CameraMode.Orbital);

        BeginDrawing();
        ClearBackground(Color.RayWhite);

        // Enable drawing to texture
        BeginTextureMode(_target);
        ClearBackground(Color.RayWhite);

        BeginMode3D(_camera);

        DrawModel(_model, _position, 0.5f, Color.White);
        DrawGrid(10, 1.0f);

        EndMode3D();

        DrawText("TEXT DRAWN IN RENDER TEXTURE", 200, 10, 30, Color.Red);

        // End drawing to texture (now we have a texture available for next passes)
        EndTextureMode();

        BeginShaderMode(_shader);

        // NOTE: Render texture must be y-flipped due to default OpenGL coordinates (left-bottom)
        DrawTextureRec(
            _target.Texture,
            new Rectangle(0, 0, _target.Texture.Width, -_target.Texture.Height),
            new Vector2(0, 0),
            Color.White
        );

        EndShaderMode();

        DrawText(
            "(c) Barracks 3D model by Alberto Cano",
            screenWidth - 220,
            screenHeight - 20,
            10,
            Color.Gray
        );

        DrawFPS(10, 10);

        EndDrawing();
    }

    public void Unload()
    {
        UnloadShader(_shader);
        UnloadTexture(_texture);
        UnloadModel(_model);
        UnloadRenderTexture(_target);
    }
}
