// Adapted for the browser from Examples/Shaders/MeshInstancing.cs
// NOTE: retargeted to GLSL ES 1.00 (WebGL1): glsl100 shader paths.
// WARNING: This example uses hardware mesh instancing (DrawMeshInstanced / instanceTransform
//          attribute); instancing is not part of core WebGL1 and is unlikely to render correctly.
using System;

namespace Examples.Web;

public unsafe class ShadersMeshInstancing : IWebExample
{
    public string Name => "Shaders / Mesh Instancing";

    private const int fps = 60;
    private const int instances = 10000;

    private int _speed;
    private int _groups;
    private float _amp;
    private float _variance;
    private float _loop;

    private float _x;
    private float _y;
    private float _z;

    private Camera3D _camera;
    private Mesh _cube;

    private Matrix4x4[] _rotations;
    private Matrix4x4[] _rotationsInc;
    private Matrix4x4[] _translations;
    private Matrix4x4[] _transforms;

    private Shader _shader;
    private Material _material;

    private int _textPositionY;
    private int _framesCounter;

    public void Init()
    {
        // Speed of jump animation
        _speed = 30;
        // Count of separate groups jumping around
        _groups = 2;
        // Maximum amplitude of jump
        _amp = 10;
        // Global variance in jump height
        _variance = 0.8f;
        // Individual cube's computed loop timer
        _loop = 0.0f;

        // Used for various 3D coordinate & vector ops
        _x = 0.0f;
        _y = 0.0f;
        _z = 0.0f;

        // Define the camera to look into our 3d world
        _camera = new();
        _camera.Position = new Vector3(-125.0f, 125.0f, -125.0f);
        _camera.Target = new Vector3(0.0f, 0.0f, 0.0f);
        _camera.Up = new Vector3(0.0f, 1.0f, 0.0f);
        _camera.FovY = 45.0f;
        _camera.Projection = CameraProjection.Perspective;

        _cube = GenMeshCube(1.0f, 1.0f, 1.0f);

        // Rotation state of instances
        _rotations = new Matrix4x4[instances];
        // Per-frame rotation animation of instances
        _rotationsInc = new Matrix4x4[instances];
        // Locations of instances
        _translations = new Matrix4x4[instances];

        // Scatter random cubes around
        for (int i = 0; i < instances; i++)
        {
            _x = GetRandomValue(-50, 50);
            _y = GetRandomValue(-50, 50);
            _z = GetRandomValue(-50, 50);
            _translations[i] = Matrix4x4.CreateTranslation(_x, _y, _z);

            _x = GetRandomValue(0, 360);
            _y = GetRandomValue(0, 360);
            _z = GetRandomValue(0, 360);
            Vector3 axis = Vector3.Normalize(new Vector3(_x, _y, _z));
            float angle = (float)GetRandomValue(0, 10) * DEG2RAD;

            _rotationsInc[i] = Matrix4x4.CreateFromAxisAngle(axis, angle);
            _rotations[i] = Matrix4x4.Identity;
        }

        // Pre-multiplied transformations passed to rlgl
        _transforms = new Matrix4x4[instances];
        _shader = LoadShader(
            "resources/shaders/glsl100/lighting_instancing.vs",
            "resources/shaders/glsl100/lighting.fs"
        );

        // Get some shader loactions
        int* locs = (int*)_shader.Locs;
        locs[(int)ShaderLocationIndex.MatrixMvp] = GetShaderLocation(_shader, "mvp");
        locs[(int)ShaderLocationIndex.VectorView] = GetShaderLocation(_shader, "viewPos");
        locs[(int)ShaderLocationIndex.MatrixModel] = GetShaderLocationAttrib(
            _shader,
            "instanceTransform"
        );

        // Ambient light level
        int ambientLoc = GetShaderLocation(_shader, "ambient");
        SetShaderValue(
            _shader,
            ambientLoc,
            new float[] { 0.2f, 0.2f, 0.2f, 1.0f },
            ShaderUniformDataType.Vec4
        );

        CreateLight(
            0,
            LightType.Directorional,
            new Vector3(50, 50, 0),
            Vector3.Zero,
            Color.White,
            _shader
        );

        _material = LoadMaterialDefault();
        _material.Shader = _shader;
        _material.Maps[(int)MaterialMapIndex.Diffuse].Color = Color.Red;

        _textPositionY = 300;

        // Simple frames counter to manage animation
        _framesCounter = 0;
    }

    public void Update()
    {
        UpdateCamera(ref _camera, CameraMode.Free);

        _textPositionY = 300;
        _framesCounter += 1;

        if (IsKeyDown(KeyboardKey.Up))
        {
            _amp += 0.5f;
        }

        if (IsKeyDown(KeyboardKey.Down))
        {
            _amp = (_amp <= 1) ? 1.0f : (_amp - 1.0f);
        }

        if (IsKeyDown(KeyboardKey.Left))
        {
            _variance = (_variance <= 0.0f) ? 0.0f : (_variance - 0.01f);
        }

        if (IsKeyDown(KeyboardKey.Right))
        {
            _variance = (_variance >= 1.0f) ? 1.0f : (_variance + 0.01f);
        }

        if (IsKeyDown(KeyboardKey.One))
        {
            _groups = 1;
        }

        if (IsKeyDown(KeyboardKey.Two))
        {
            _groups = 2;
        }

        if (IsKeyDown(KeyboardKey.Three))
        {
            _groups = 3;
        }

        if (IsKeyDown(KeyboardKey.Four))
        {
            _groups = 4;
        }

        if (IsKeyDown(KeyboardKey.Five))
        {
            _groups = 5;
        }

        if (IsKeyDown(KeyboardKey.Six))
        {
            _groups = 6;
        }

        if (IsKeyDown(KeyboardKey.Seven))
        {
            _groups = 7;
        }

        if (IsKeyDown(KeyboardKey.Eight))
        {
            _groups = 8;
        }

        if (IsKeyDown(KeyboardKey.Nine))
        {
            _groups = 9;
        }

        if (IsKeyDown(KeyboardKey.W))
        {
            _groups = 7;
            _amp = 25;
            _speed = 18;
            _variance = 0.70f;
        }

        if (IsKeyDown(KeyboardKey.Equal))
        {
            _speed = (_speed <= (int)(fps * 0.25f)) ? (int)(fps * 0.25f) : (int)(_speed * 0.95f);
        }

        if (IsKeyDown(KeyboardKey.KpAdd))
        {
            _speed = (_speed <= (int)(fps * 0.25f)) ? (int)(fps * 0.25f) : (int)(_speed * 0.95f);
        }

        if (IsKeyDown(KeyboardKey.Minus))
        {
            _speed = (int)MathF.Max(_speed * 1.02f, _speed + 1);
        }

        if (IsKeyDown(KeyboardKey.KpSubtract))
        {
            _speed = (int)MathF.Max(_speed * 1.02f, _speed + 1);
        }

        // Update the light shader with the camera view position
        float[] cameraPos = { _camera.Position.X, _camera.Position.Y, _camera.Position.Z };
        SetShaderValue(
            _shader,
            (int)ShaderLocationIndex.VectorView,
            cameraPos,
            ShaderUniformDataType.Vec3
        );

        // Apply per-instance transformations
        for (int i = 0; i < instances; i++)
        {
            _rotations[i] = Matrix4x4.Multiply(_rotations[i], _rotationsInc[i]);
            _transforms[i] = Matrix4x4.Multiply(_rotations[i], _translations[i]);

            // Get the animation cycle's framesCounter for this instance
            _loop = (float)((_framesCounter + (int)(((float)(i % _groups) / _groups) * _speed)) % _speed) / _speed;

            // Calculate the y according to loop cycle
            _y = (MathF.Sin(_loop * MathF.PI * 2)) * _amp * ((1 - _variance) + (_variance * (float)(i % (_groups * 10)) / (_groups * 10)));

            // Clamp to floor
            _y = (_y < 0) ? 0.0f : _y;

            _transforms[i] = Matrix4x4.Multiply(_transforms[i], Matrix4x4.CreateTranslation(0.0f, _y, 0.0f));
            _transforms[i] = Matrix4x4.Transpose(_transforms[i]);
        }

        BeginDrawing();
        ClearBackground(Color.RayWhite);

        BeginMode3D(_camera);
        DrawMeshInstanced(_cube, _material, _transforms, instances);
        EndMode3D();

        DrawText("A CUBE OF DANCING CUBES!", 490, 10, 20, Color.Maroon);
        DrawText("PRESS KEYS:", 10, _textPositionY, 20, Color.Black);

        DrawText("1 - 9", 10, _textPositionY += 25, 10, Color.Black);
        DrawText(": Number of groups", 50, _textPositionY, 10, Color.Black);
        DrawText($": {_groups}", 160, _textPositionY, 10, Color.Black);

        DrawText("UP", 10, _textPositionY += 15, 10, Color.Black);
        DrawText(": increase amplitude", 50, _textPositionY, 10, Color.Black);
        DrawText($": {_amp}%.2f", 160, _textPositionY, 10, Color.Black);

        DrawText("DOWN", 10, _textPositionY += 15, 10, Color.Black);
        DrawText(": decrease amplitude", 50, _textPositionY, 10, Color.Black);

        DrawText("LEFT", 10, _textPositionY += 15, 10, Color.Black);
        DrawText(": decrease variance", 50, _textPositionY, 10, Color.Black);
        DrawText($": {_variance}.2f", 160, _textPositionY, 10, Color.Black);

        DrawText("RIGHT", 10, _textPositionY += 15, 10, Color.Black);
        DrawText(": increase variance", 50, _textPositionY, 10, Color.Black);

        DrawText("+/=", 10, _textPositionY += 15, 10, Color.Black);
        DrawText(": increase speed", 50, _textPositionY, 10, Color.Black);
        DrawText($": {_speed} = {((float)fps / _speed)} loops/sec", 160, _textPositionY, 10, Color.Black);

        DrawText("-", 10, _textPositionY += 15, 10, Color.Black);
        DrawText(": decrease speed", 50, _textPositionY, 10, Color.Black);

        DrawText("W", 10, _textPositionY += 15, 10, Color.Black);
        DrawText(": Wild setup!", 50, _textPositionY, 10, Color.Black);

        DrawFPS(10, 10);

        EndDrawing();
    }

    public void Unload()
    {
        // Detach shader so UnloadMaterial does not also unload it, then free everything.
        _material.Shader = new();
        UnloadMaterial(_material);
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
