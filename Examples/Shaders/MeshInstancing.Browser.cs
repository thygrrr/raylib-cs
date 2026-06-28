#if BROWSER
using Examples;
using System;

namespace Examples.Shaders;

public partial class MeshInstancing : IExample
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
        public string Name => "Shaders / Mesh Instancing";

        private const int MaxInstances = 10000;

        private Camera3D _camera;
        private Mesh _cube;

        private Matrix4x4[] _transforms;

        private Shader _shader;
        private Material _matInstances;
        private Material _matDefault;

        public void Init()
        {
            // Define the camera to look into our 3d world
            _camera = new();
            _camera.Position = new Vector3(-125.0f, 125.0f, -125.0f);
            _camera.Target = new Vector3(0.0f, 0.0f, 0.0f);
            _camera.Up = new Vector3(0.0f, 1.0f, 0.0f);
            _camera.FovY = 45.0f;
            _camera.Projection = CameraProjection.Perspective;

            // Define mesh to be instanced
            _cube = GenMeshCube(1.0f, 1.0f, 1.0f);

            // Define transforms to be uploaded to GPU for instances
            _transforms = new Matrix4x4[MaxInstances];

            // Translate and rotate cubes randomly
            for (int i = 0; i < MaxInstances; i++)
            {
                Matrix4x4 translation = Matrix4x4.CreateTranslation(
                    GetRandomValue(-50, 50),
                    GetRandomValue(-50, 50),
                    GetRandomValue(-50, 50)
                );
                Vector3 axis = Vector3.Normalize(new Vector3(
                    GetRandomValue(0, 360),
                    GetRandomValue(0, 360),
                    GetRandomValue(0, 360)
                ));
                float angle = GetRandomValue(0, 180) * DEG2RAD;
                Matrix4x4 rotation = Matrix4x4.CreateFromAxisAngle(axis, angle);

                _transforms[i] = Matrix4x4.Transpose(Matrix4x4.Multiply(rotation, translation));
            }

            // Load lighting shader
            _shader = LoadShader(
                "resources/shaders/glsl100/lighting_instancing.vs",
                "resources/shaders/glsl100/lighting.fs"
            );

            // Get shader locations
            int* locs = (int*)_shader.Locs;
            locs[(int)ShaderLocationIndex.MatrixMvp] = GetShaderLocation(_shader, "mvp");
            locs[(int)ShaderLocationIndex.VectorView] = GetShaderLocation(_shader, "viewPos");

            // Set shader value: ambient light level
            int ambientLoc = GetShaderLocation(_shader, "ambient");
            SetShaderValue(
                _shader,
                ambientLoc,
                new float[] { 0.2f, 0.2f, 0.2f, 1.0f },
                ShaderUniformDataType.Vec4
            );

            // Create one light
            CreateLight(
                0,
                LightType.Directorional,
                new Vector3(50.0f, 50.0f, 0.0f),
                Vector3.Zero,
                Color.White,
                _shader
            );

            // NOTE: We are assigning the intancing shader to material.shader
            // to be used on mesh drawing with DrawMeshInstanced()
            _matInstances = LoadMaterialDefault();
            _matInstances.Shader = _shader;
            _matInstances.Maps[(int)MaterialMapIndex.Diffuse].Color = Color.Red;

            // Load default material (using raylib intenral default shader) for non-instanced mesh drawing
            _matDefault = LoadMaterialDefault();
            _matDefault.Maps[(int)MaterialMapIndex.Diffuse].Color = Color.Blue;
        }

        public void Update()
        {
            UpdateCamera(ref _camera, CameraMode.Orbital);

            // Update the light shader with the camera view position
            float[] cameraPos = { _camera.Position.X, _camera.Position.Y, _camera.Position.Z };
            SetShaderValue(
                _shader,
                _shader.Locs[(int)ShaderLocationIndex.VectorView],
                cameraPos,
                ShaderUniformDataType.Vec3
            );

            BeginDrawing();

            ClearBackground(Color.RayWhite);

            BeginMode3D(_camera);

            // Draw cube mesh with default material (BLUE)
            DrawMesh(_cube, _matDefault, Matrix4x4.Transpose(Matrix4x4.CreateTranslation(-10.0f, 0.0f, 0.0f)));

            // Draw meshes instanced using material containing instancing shader (RED + lighting),
            // transforms[] for the instances should be provided, they are dynamically
            // updated in GPU every frame, so we can animate the different mesh instances
            DrawMeshInstanced(_cube, _matInstances, _transforms, MaxInstances);

            // Draw cube mesh with default material (BLUE)
            DrawMesh(_cube, _matDefault, Matrix4x4.Transpose(Matrix4x4.CreateTranslation(10.0f, 0.0f, 0.0f)));

            EndMode3D();

            DrawFPS(10, 10);

            EndDrawing();
        }

        public void Unload()
        {
            // Detach shader so UnloadMaterial does not also unload it, then free everything.
            _matInstances.Shader = new();
            UnloadMaterial(_matInstances);
            UnloadMaterial(_matDefault);
            UnloadMesh(_cube);
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
