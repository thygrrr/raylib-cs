#if BROWSER
using Examples;
using System;

namespace Examples.Shaders;

public partial class BasicLighting : IExample
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

    private unsafe sealed class BrowserAdapter : IExample
    {
        public string Name => "Shaders / Basic Lighting";

        private const int GLSL_VERSION = 100;

        private Camera3D _camera;
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

            // Load basic lighting shader
            _shader = LoadShader(
                "resources/shaders/glsl100/lighting.vs",
                "resources/shaders/glsl100/lighting.fs"
            );

            // Get some required shader locations
            _shader.Locs[(int)ShaderLocationIndex.VectorView] = GetShaderLocation(_shader, "viewPos");

            // Ambient light level (some basic lighting)
            int ambientLoc = GetShaderLocation(_shader, "ambient");
            float[] ambient = new[] { 0.1f, 0.1f, 0.1f, 1.0f };
            SetShaderValue(_shader, ambientLoc, ambient, ShaderUniformDataType.Vec4);

            // Create lights
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

            // Update the shader with the camera view vector (points towards { 0.0f, 0.0f, 0.0f })
            SetShaderValue(
                _shader,
                _shader.Locs[(int)ShaderLocationIndex.VectorView],
                _camera.Position,
                ShaderUniformDataType.Vec3
            );

            // Check key inputs to enable/disable lights
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
            for (int i = 0; i < 4; i++)
            {
                UpdateLightValues(_shader, _lights[i]);
            }

            BeginDrawing();

            ClearBackground(Color.RayWhite);

            BeginMode3D(_camera);

            BeginShaderMode(_shader);

            DrawPlane(Vector3.Zero, new Vector2(10.0f, 10.0f), Color.White);
            DrawCube(Vector3.Zero, 2.0f, 4.0f, 2.0f, Color.White);

            EndShaderMode();

            // Draw spheres to show where the lights are
            for (int i = 0; i < 4; i++)
            {
                if (_lights[i].Enabled)
                {
                    DrawSphereEx(_lights[i].Position, 0.2f, 8, 8, _lights[i].Color);
                }
                else
                {
                    DrawSphereWires(_lights[i].Position, 0.2f, 8, 8, ColorAlpha(_lights[i].Color, 0.3f));
                }
            }

            DrawGrid(10, 1.0f);

            EndMode3D();

            DrawFPS(10, 10);

            DrawText("Use keys [Y][R][G][B] to toggle lights", 10, 40, 20, Color.DarkGray);

            EndDrawing();
        }

        public void Unload()
        {
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
}
#endif
