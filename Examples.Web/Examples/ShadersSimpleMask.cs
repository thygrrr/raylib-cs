// Adapted for the browser from Examples/Shaders/SimpleMask.cs
using static Raylib_cs.Raymath;

namespace Examples.Web;

public unsafe class ShadersSimpleMask : IWebExample
{
    public string Name => "Shaders / Simple Mask";

    private Camera3D _camera;

    private Model _model1;
    private Model _model2;
    private Model _model3;

    private Shader _shader;
    private Texture2D _texDiffuse;
    private Texture2D _texMask;

    private int _shaderFrame;
    private int _framesCounter;
    private Vector3 _rotation;

    public void Init()
    {
        // Define the camera to look into our 3d world
        _camera = new();
        _camera.Position = new Vector3(0.0f, 1.0f, 2.0f);
        _camera.Target = new Vector3(0.0f, 0.0f, 0.0f);
        _camera.Up = new Vector3(0.0f, 1.0f, 0.0f);
        _camera.FovY = 45.0f;
        _camera.Projection = CameraProjection.Perspective;

        // Define our three models to show the shader on
        Mesh torus = GenMeshTorus(.3f, 1, 16, 32);
        _model1 = LoadModelFromMesh(torus);

        Mesh cube = GenMeshCube(.8f, .8f, .8f);
        _model2 = LoadModelFromMesh(cube);

        // Generate model to be shaded just to see the gaps in the other two
        Mesh sphere = GenMeshSphere(1, 16, 16);
        _model3 = LoadModelFromMesh(sphere);

        // Load the shader
        // NOTE: Using GLSL 100 shader version for WebGL1 (OpenGL ES 2.0)
        _shader = LoadShader("resources/shaders/glsl100/mask.vs", "resources/shaders/glsl100/mask.fs");

        // Load and apply the diffuse texture (colour map)
        _texDiffuse = LoadTexture("resources/plasma.png");

        Material* materials = _model1.Materials;
        MaterialMap* maps = materials[0].Maps;
        _model1.Materials[0].Maps[(int)MaterialMapIndex.Albedo].Texture = _texDiffuse;

        materials = _model2.Materials;
        maps = materials[0].Maps;
        maps[(int)MaterialMapIndex.Albedo].Texture = _texDiffuse;

        // Using MAP_EMISSION as a spare slot to use for 2nd texture
        // NOTE: Don't use MAP_IRRADIANCE, MAP_PREFILTER or  MAP_CUBEMAP
        // as they are bound as cube maps
        _texMask = LoadTexture("resources/mask.png");

        materials = _model1.Materials;
        maps = (MaterialMap*)materials[0].Maps;
        maps[(int)MaterialMapIndex.Emission].Texture = _texMask;

        materials = _model2.Materials;
        maps = (MaterialMap*)materials[0].Maps;
        maps[(int)MaterialMapIndex.Emission].Texture = _texMask;

        int* locs = _shader.Locs;
        locs[(int)ShaderLocationIndex.MapEmission] = GetShaderLocation(_shader, "mask");

        // Frame is incremented each frame to animate the shader
        _shaderFrame = GetShaderLocation(_shader, "framesCounter");

        // Apply the shader to the two models
        materials = _model1.Materials;
        materials[0].Shader = _shader;

        materials = (Material*)_model2.Materials;
        materials[0].Shader = _shader;

        _framesCounter = 0;

        // Model rotation angles
        _rotation = new(0, 0, 0);
    }

    public void Update()
    {
        _framesCounter++;
        _rotation.X += 0.01f;
        _rotation.Y += 0.005f;
        _rotation.Z -= 0.0025f;

        // Send frames counter to shader for animation
        Raylib.SetShaderValue(_shader, _shaderFrame, _framesCounter, ShaderUniformDataType.Int);

        // Rotate one of the models
        _model1.Transform = MatrixRotateXYZ(_rotation);

        UpdateCamera(ref _camera, CameraMode.Custom);

        BeginDrawing();
        ClearBackground(Color.DarkBlue);

        BeginMode3D(_camera);

        DrawModel(_model1, new Vector3(0.5f, 0, 0), 1, Color.White);
        DrawModelEx(_model2, new Vector3(-.5f, 0, 0), new Vector3(1, 1, 0), 50, new Vector3(1, 1, 1), Color.White);
        DrawModel(_model3, new Vector3(0, 0, -1.5f), 1, Color.White);
        DrawGrid(10, 1.0f);

        EndMode3D();

        string frameText = $"Frame: {_framesCounter}";
        DrawRectangle(16, 698, MeasureText(frameText, 20) + 8, 42, Color.Blue);
        DrawText(frameText, 20, 700, 20, Color.White);

        DrawFPS(10, 10);

        EndDrawing();
    }

    public void Unload()
    {
        UnloadModel(_model1);
        UnloadModel(_model2);
        UnloadModel(_model3);

        UnloadTexture(_texDiffuse);
        UnloadTexture(_texMask);

        UnloadShader(_shader);
    }
}
