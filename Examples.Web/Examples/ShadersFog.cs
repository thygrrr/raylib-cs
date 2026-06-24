// Adapted for the browser from Examples/Shaders/Fog.cs
// NOTE: retargeted to GLSL ES 1.00 (WebGL1): glsl100 shader paths.
using System;
using static Raylib_cs.Raymath;

namespace Examples.Web;

public class ShadersFog : IWebExample
{
    public string Name => "Shaders / Fog";

    private Camera3D _camera;
    private Model _modelA;
    private Model _modelB;
    private Model _modelC;
    private Texture2D _texture;
    private Shader _shader;
    private int _fogDensityLoc;
    private float _fogDensity;

    public void Init()
    {
        // Define the camera to look into our 3d world
        _camera = new();
        _camera.Position = new Vector3(2.0f, 2.0f, 6.0f);
        _camera.Target = new Vector3(0.0f, 0.5f, 0.0f);
        _camera.Up = new Vector3(0.0f, 1.0f, 0.0f);
        _camera.FovY = 45.0f;
        _camera.Projection = CameraProjection.Perspective;

        // Load models and texture
        _modelA = LoadModelFromMesh(GenMeshTorus(0.4f, 1.0f, 16, 32));
        _modelB = LoadModelFromMesh(GenMeshCube(1.0f, 1.0f, 1.0f));
        _modelC = LoadModelFromMesh(GenMeshSphere(0.5f, 32, 32));
        _texture = LoadTexture("resources/texel_checker.png");

        // Assign texture to default model material
        SetMaterialTexture(ref _modelA, 0, MaterialMapIndex.Albedo, ref _texture);
        SetMaterialTexture(ref _modelB, 0, MaterialMapIndex.Albedo, ref _texture);
        SetMaterialTexture(ref _modelC, 0, MaterialMapIndex.Albedo, ref _texture);

        // Load shader and set up some uniforms
        _shader = LoadShader("resources/shaders/glsl100/lighting.vs", "resources/shaders/glsl100/fog.fs");
        _shader.Locs[(int)ShaderLocationIndex.MatrixModel] = GetShaderLocation(_shader, "matModel");
        _shader.Locs[(int)ShaderLocationIndex.VectorView] = GetShaderLocation(_shader, "viewPos");

        // Ambient light level
        int ambientLoc = GetShaderLocation(_shader, "ambient");
        SetShaderValue(
            _shader,
            ambientLoc,
            new float[] { 0.2f, 0.2f, 0.2f, 1.0f },
            ShaderUniformDataType.Vec4
        );

        _fogDensity = 0.15f;
        _fogDensityLoc = GetShaderLocation(_shader, "fogDensity");
        SetShaderValue(_shader, _fogDensityLoc, _fogDensity, ShaderUniformDataType.Float);

        // NOTE: All models share the same shader
        SetMaterialShader(ref _modelA, 0, ref _shader);
        SetMaterialShader(ref _modelB, 0, ref _shader);
        SetMaterialShader(ref _modelC, 0, ref _shader);

        // Using just 1 point lights
        CreateLight(0, LightType.Point, new Vector3(0, 2, 6), Vector3.Zero, Color.White, _shader);
    }

    public void Update()
    {
        UpdateCamera(ref _camera, CameraMode.Orbital);

        if (IsKeyDown(KeyboardKey.Up))
        {
            _fogDensity += 0.001f;
            if (_fogDensity > 1.0f)
            {
                _fogDensity = 1.0f;
            }
        }

        if (IsKeyDown(KeyboardKey.Down))
        {
            _fogDensity -= 0.001f;
            if (_fogDensity < 0.0f)
            {
                _fogDensity = 0.0f;
            }
        }

        SetShaderValue(_shader, _fogDensityLoc, _fogDensity, ShaderUniformDataType.Float);

        // Rotate the torus
        _modelA.Transform = MatrixMultiply(_modelA.Transform, MatrixRotateX(-0.025f));
        _modelA.Transform = MatrixMultiply(_modelA.Transform, MatrixRotateZ(0.012f));

        // Update the light shader with the camera view position
        SetShaderValue(
            _shader,
            _shader.Locs[(int)ShaderLocationIndex.VectorView],
            _camera.Position,
            ShaderUniformDataType.Vec3
        );

        BeginDrawing();
        ClearBackground(Color.Gray);

        BeginMode3D(_camera);

        // Draw the three models
        DrawModel(_modelA, Vector3.Zero, 1.0f, Color.White);
        DrawModel(_modelB, new Vector3(-2.6f, 0, 0), 1.0f, Color.White);
        DrawModel(_modelC, new Vector3(2.6f, 0, 0), 1.0f, Color.White);

        for (int i = -20; i < 20; i += 2)
        {
            DrawModel(_modelA, new Vector3(i, 0, 2), 1.0f, Color.White);
        }

        EndMode3D();

        DrawText(
            $"Use up/down to change fog density [{_fogDensity:F2}]",
            10,
            10,
            20,
            Color.RayWhite
        );

        EndDrawing();
    }

    public void Unload()
    {
        UnloadModel(_modelA);
        UnloadModel(_modelB);
        UnloadModel(_modelC);

        UnloadTexture(_texture);
        UnloadShader(_shader);
    }

    // Inlined minimal equivalent of Examples.Shared.Rlights (not referenced by Examples.Web)
    private enum LightType
    {
        Directorional,
        Point
    }

    private struct Light
    {
        public bool Enabled;
        public LightType Type;
        public Vector3 Position;
        public Vector3 Target;
        public Color Color;

        public int EnabledLoc;
        public int TypeLoc;
        public int PosLoc;
        public int TargetLoc;
        public int ColorLoc;
    }

    private static Light CreateLight(
        int lightsCount,
        LightType type,
        Vector3 pos,
        Vector3 target,
        Color color,
        Shader shader
    )
    {
        Light light = new();

        light.Enabled = true;
        light.Type = type;
        light.Position = pos;
        light.Target = target;
        light.Color = color;

        light.EnabledLoc = GetShaderLocation(shader, "lights[" + lightsCount + "].enabled");
        light.TypeLoc = GetShaderLocation(shader, "lights[" + lightsCount + "].type");
        light.PosLoc = GetShaderLocation(shader, "lights[" + lightsCount + "].position");
        light.TargetLoc = GetShaderLocation(shader, "lights[" + lightsCount + "].target");
        light.ColorLoc = GetShaderLocation(shader, "lights[" + lightsCount + "].color");

        UpdateLightValues(shader, light);

        return light;
    }

    private static void UpdateLightValues(Shader shader, Light light)
    {
        SetShaderValue(shader, light.EnabledLoc, light.Enabled ? 1 : 0, ShaderUniformDataType.Int);
        SetShaderValue(shader, light.TypeLoc, (int)light.Type, ShaderUniformDataType.Int);

        SetShaderValue(shader, light.PosLoc, light.Position, ShaderUniformDataType.Vec3);
        SetShaderValue(shader, light.TargetLoc, light.Target, ShaderUniformDataType.Vec3);

        float[] color = new[]
        {
            (float)light.Color.R / 255f,
            (float)light.Color.G / 255f,
            (float)light.Color.B / 255f,
            (float)light.Color.A / 255f
        };
        SetShaderValue(shader, light.ColorLoc, color, ShaderUniformDataType.Vec4);
    }
}
