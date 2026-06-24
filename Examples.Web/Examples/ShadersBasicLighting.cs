// Adapted for the browser from Examples/Shaders/BasicLighting.cs
// NOTE: retargeted to GLSL ES 1.00 (WebGL1): GLSL_VERSION 100 and glsl100 shader paths.
using System;

namespace Examples.Web;

public class ShadersBasicLighting : IWebExample
{
    public string Name => "Shaders / Basic Lighting";

    private const int GLSL_VERSION = 100;

    private Camera3D _camera;
    private Model _model;
    private Model _cube;
    private Shader _shader;
    private Light[] _lights;

    public unsafe void Init()
    {
        // Define the camera to look into our 3d world
        _camera = new();
        _camera.Position = new Vector3(2.0f, 4.0f, 6.0f);
        _camera.Target = new Vector3(0.0f, 0.5f, 0.0f);
        _camera.Up = new Vector3(0.0f, 1.0f, 0.0f);
        _camera.FovY = 45.0f;
        _camera.Projection = CameraProjection.Perspective;

        // Load plane model from a generated mesh
        _model = LoadModelFromMesh(GenMeshPlane(10.0f, 10.0f, 3, 3));
        _cube = LoadModelFromMesh(GenMeshCube(2.0f, 4.0f, 2.0f));

        _shader = LoadShader(
            "resources/shaders/glsl100/lighting.vs",
            "resources/shaders/glsl100/lighting.fs"
        );

        // Get some required shader loactions
        _shader.Locs[(int)ShaderLocationIndex.VectorView] = GetShaderLocation(_shader, "viewPos");

        // ambient light level
        int ambientLoc = GetShaderLocation(_shader, "ambient");
        float[] ambient = new[] { 0.1f, 0.1f, 0.1f, 1.0f };
        SetShaderValue(_shader, ambientLoc, ambient, ShaderUniformDataType.Vec4);

        // Assign out lighting shader to model
        _model.Materials[0].Shader = _shader;
        _cube.Materials[0].Shader = _shader;

        // Using 4 point lights: Color.gold, Color.red, Color.green and Color.blue
        _lights = new Light[4];
        _lights[0] = CreateLight(
            0,
            LightType.Point,
            new Vector3(-2, 1, -2),
            Vector3.Zero,
            Color.Yellow,
            _shader
        );
        _lights[1] = CreateLight(
            1,
            LightType.Point,
            new Vector3(2, 1, 2),
            Vector3.Zero,
            Color.Red,
            _shader
        );
        _lights[2] = CreateLight(
            2,
            LightType.Point,
            new Vector3(-2, 1, 2),
            Vector3.Zero,
            Color.Green,
            _shader
        );
        _lights[3] = CreateLight(
            3,
            LightType.Point,
            new Vector3(2, 1, -2),
            Vector3.Zero,
            Color.Blue,
            _shader
        );
    }

    public void Update()
    {
        UpdateCamera(ref _camera, CameraMode.Orbital);

        if (IsKeyPressed(KeyboardKey.Y))
        {
            _lights[0].Enabled = !_lights[0].Enabled;
        }
        if (IsKeyPressed(KeyboardKey.R))
        {
            _lights[1].Enabled = !_lights[1].Enabled;
        }
        if (IsKeyPressed(KeyboardKey.G))
        {
            _lights[2].Enabled = !_lights[2].Enabled;
        }
        if (IsKeyPressed(KeyboardKey.B))
        {
            _lights[3].Enabled = !_lights[3].Enabled;
        }

        // Update light values (actually, only enable/disable them)
        UpdateLightValues(_shader, _lights[0]);
        UpdateLightValues(_shader, _lights[1]);
        UpdateLightValues(_shader, _lights[2]);
        UpdateLightValues(_shader, _lights[3]);

        // Update the light shader with the camera view position
        SetShaderValue(
            _shader,
            _shader.Locs[(int)ShaderLocationIndex.VectorView],
            _camera.Position,
            ShaderUniformDataType.Vec3
        );

        BeginDrawing();
        ClearBackground(Color.RayWhite);

        BeginMode3D(_camera);

        DrawModel(_model, Vector3.Zero, 1.0f, Color.White);
        DrawModel(_cube, Vector3.Zero, 1.0f, Color.White);

        // Draw markers to show where the lights are
        if (_lights[0].Enabled)
        {
            DrawSphereEx(_lights[0].Position, 0.2f, 8, 8, Color.Yellow);
        }
        else
        {
            DrawSphereWires(_lights[0].Position, 0.2f, 8, 8, ColorAlpha(Color.Yellow, 0.3f));
        }

        if (_lights[1].Enabled)
        {
            DrawSphereEx(_lights[1].Position, 0.2f, 8, 8, Color.Red);
        }
        else
        {
            DrawSphereWires(_lights[1].Position, 0.2f, 8, 8, ColorAlpha(Color.Red, 0.3f));
        }

        if (_lights[2].Enabled)
        {
            DrawSphereEx(_lights[2].Position, 0.2f, 8, 8, Color.Green);
        }
        else
        {
            DrawSphereWires(_lights[2].Position, 0.2f, 8, 8, ColorAlpha(Color.Green, 0.3f));
        }

        if (_lights[3].Enabled)
        {
            DrawSphereEx(_lights[3].Position, 0.2f, 8, 8, Color.Blue);
        }
        else
        {
            DrawSphereWires(_lights[3].Position, 0.2f, 8, 8, ColorAlpha(Color.Blue, 0.3f));
        }

        DrawGrid(10, 1.0f);

        EndMode3D();

        DrawFPS(10, 10);
        DrawText("Use keys [Y][R][G][B] to toggle lights", 10, 40, 20, Color.DarkGray);

        EndDrawing();
    }

    public void Unload()
    {
        UnloadModel(_model);
        UnloadModel(_cube);
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
