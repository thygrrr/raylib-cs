// Adapted for the browser from Examples/Shaders/BasicPbr.cs
// NOTE: retargeted to GLSL ES 1.00 (WebGL1): GLSL_VERSION 100 and glsl100 shader paths.
// WARNING: This is a full PBR pipeline (MRA / normal / emissive maps, many uniforms). PBR shaders
//          generally require GLSL ES 3.00+/desktop GL features and are unlikely to work on WebGL1.
using System.Numerics;

namespace Examples.Web;

public unsafe class ShadersBasicPbr : IWebExample
{
    public string Name => "Shaders / Basic PBR";

    private const int GLSL_VERSION = 100;

    private const int screenWidth = 800;
    private const int screenHeight = 450;

    private Camera3D _camera;
    private Shader _shader;
    private Model _car;
    private Model _floor;
    private PbrLight[] _lights;

    private int _emissiveIntensityLoc;
    private int _emissiveColorLoc;
    private int _textureTilingLoc;

    private Vector2 _carTextureTiling;
    private Vector2 _floorTextureTiling;

    public void Init()
    {
        // Define the camera to look into our 3d world
        _camera = new();
        _camera.Position = new Vector3(2.0f, 4.0f, 6.0f);
        _camera.Target = new Vector3(0.0f, 0.5f, 0.0f);
        _camera.Up = new Vector3(0.0f, 1.0f, 0.0f);
        _camera.FovY = 45.0f;
        _camera.Projection = CameraProjection.Perspective;

        // Load PBR shader and setup all required locations
        _shader = LoadShader("resources/shaders/glsl100/pbr.vs", "resources/shaders/glsl100/pbr.fs");

        _shader.Locs[(int)ShaderLocationIndex.MapAlbedo] = GetShaderLocation(_shader, "albedoMap");
        // WARNING: Metalness, roughness, and ambient occlusion are all packed into a MRA texture
        // They are passed as to the SHADER_LOC_MAP_METALNESS location for convenience,
        // shader already takes care of it accordingly
        _shader.Locs[(int)ShaderLocationIndex.MapMetalness] = GetShaderLocation(_shader, "mraMap");
        _shader.Locs[(int)ShaderLocationIndex.MapNormal] = GetShaderLocation(_shader, "normalMap");
        // WARNING: Similar to the MRA map, the emissive map packs different information
        // into a single texture: it stores height and emission data
        // It is binded to SHADER_LOC_MAP_EMISSION location an properly processed on shader
        _shader.Locs[(int)ShaderLocationIndex.MapEmission] = GetShaderLocation(_shader, "emissiveMap");
        _shader.Locs[(int)ShaderLocationIndex.ColorDiffuse] = GetShaderLocation(_shader, "albedoColor");

        // Setup additional required shader locations, including lights data
        _shader.Locs[(int)ShaderLocationIndex.VectorView] = GetShaderLocation(_shader, "viewPos");
        var lightCountLoc = GetShaderLocation(_shader, "numOfLights");
        var maxLightCount = 4;
        SetShaderValue(_shader, lightCountLoc, &maxLightCount, ShaderUniformDataType.Int);

        // Setup ambient color and intensity parameters
        var ambientIntensity = 0.02f;
        var ambientColor = new Color(26, 32, 135, 255);
        var ambientColorNormalized = new Vector3(ambientColor.R / 255.0F, ambientColor.G / 255.0F, ambientColor.B / 255.0F);
        SetShaderValue(_shader, GetShaderLocation(_shader, "ambientColor"), &ambientColorNormalized, ShaderUniformDataType.Vec3);
        SetShaderValue(_shader, GetShaderLocation(_shader, "ambient"), &ambientIntensity, ShaderUniformDataType.Float);

        // Get location for shader parameters that can be modified in real time
        _emissiveIntensityLoc = GetShaderLocation(_shader, "emissivePower");
        _emissiveColorLoc = GetShaderLocation(_shader, "emissiveColor");
        _textureTilingLoc = GetShaderLocation(_shader, "tiling");

        // Load old car model using PBR maps and shader
        // WARNING: We know this model consists of a single model.meshes[0] and
        // that model.materials[0] is by default assigned to that mesh
        // There could be more complex models consisting of multiple meshes and
        // multiple materials defined for those meshes... but always 1 mesh = 1 material
        _car = LoadModel("resources/models/gltf/old_car_new.glb");

        // Assign already setup PBR shader to model.materials[0], used by models.meshes[0]
        _car.Materials[0].Shader = _shader;

        // Setup materials[0].maps default parameters
        _car.Materials[0].Maps[(int)MaterialMapIndex.Albedo].Color = Color.White;
        _car.Materials[0].Maps[(int)MaterialMapIndex.Metalness].Value = 0.0f;
        _car.Materials[0].Maps[(int)MaterialMapIndex.Roughness].Value = 0.0f;
        _car.Materials[0].Maps[(int)MaterialMapIndex.Occlusion].Value = 1.0f;
        _car.Materials[0].Maps[(int)MaterialMapIndex.Emission].Color = new Color(255, 162, 0, 255);

        // Setup materials[0].maps default textures
        _car.Materials[0].Maps[(int)MaterialMapIndex.Albedo].Texture = LoadTexture("resources/old_car_d.png");
        _car.Materials[0].Maps[(int)MaterialMapIndex.Metalness].Texture = LoadTexture("resources/old_car_mra.png");
        _car.Materials[0].Maps[(int)MaterialMapIndex.Normal].Texture = LoadTexture("resources/old_car_n.png");
        _car.Materials[0].Maps[(int)MaterialMapIndex.Emission].Texture = LoadTexture("resources/old_car_e.png");

        // Load floor model mesh and assign material parameters
        _floor = LoadModel("resources/models/gltf/plane.glb");

        // Assign material shader for our floor model, same PBR shader
        _floor.Materials[0].Shader = _shader;

        _floor.Materials[0].Maps[(int)MaterialMapIndex.Albedo].Color = Color.White;
        _floor.Materials[0].Maps[(int)MaterialMapIndex.Metalness].Value = 0.0f;
        _floor.Materials[0].Maps[(int)MaterialMapIndex.Roughness].Value = 0.0f;
        _floor.Materials[0].Maps[(int)MaterialMapIndex.Occlusion].Value = 1.0f;
        _floor.Materials[0].Maps[(int)MaterialMapIndex.Emission].Color = Color.Black;

        _floor.Materials[0].Maps[(int)MaterialMapIndex.Albedo].Texture = LoadTexture("resources/road_a.png");
        _floor.Materials[0].Maps[(int)MaterialMapIndex.Metalness].Texture = LoadTexture("resources/road_mra.png");
        _floor.Materials[0].Maps[(int)MaterialMapIndex.Normal].Texture = LoadTexture("resources/road_n.png");

        // Models texture tiling parameter can be stored in the Material struct if required (CURRENTLY NOT USED)
        _carTextureTiling = new Vector2(0.5f, 0.5f);
        _floorTextureTiling = new Vector2(0.5f, 0.5f);

        // Create some lights
        _lights = new PbrLight[4];
        _lights[0] = CreateLight(
            0,
            PbrLightType.Point,
            new Vector3(-1.0f, 1.0f, -2.0f),
            new Vector3(0.0f, 0.0f, 0.0f),
            Color.Yellow,
            4.0f,
            _shader);
        _lights[1] = CreateLight(1,
            PbrLightType.Point,
            new Vector3(2.0f, 1.0f, 1.0f),
            new Vector3(0.0f, 0.0f, 0.0f),
            Color.Green,
            3.3f,
            _shader);
        _lights[2] = CreateLight(
            2, PbrLightType.Point,
            new Vector3(-2.0f, 1.0f, 1.0f),
            new Vector3(0.0f, 0.0f, 0.0f),
            Color.Red,
            8.3f,
            _shader);
        _lights[3] = CreateLight(
            3,
            PbrLightType.Point,
            new Vector3(1.0f, 1.0f, -2.0f),
            new Vector3(0.0f, 0.0f, 0.0f),
            Color.Blue,
            2.0f,
            _shader);

        // Setup material texture maps usage in shader
        // NOTE: By default, the texture maps are always used
        var usage = 1;
        SetShaderValue(_shader, GetShaderLocation(_shader, "useTexAlbedo"), &usage, ShaderUniformDataType.Int);
        SetShaderValue(_shader, GetShaderLocation(_shader, "useTexNormal"), &usage, ShaderUniformDataType.Int);
        SetShaderValue(_shader, GetShaderLocation(_shader, "useTexMRA"), &usage, ShaderUniformDataType.Int);
        SetShaderValue(_shader, GetShaderLocation(_shader, "useTexEmissive"), &usage, ShaderUniformDataType.Int);
    }

    public void Update()
    {
        UpdateCamera(ref _camera, CameraMode.Orbital);

        // Update the shader with the camera view vector (points towards { 0.0f, 0.0f, 0.0f })
        var cameraPos = _camera.Position;
        SetShaderValue(_shader, _shader.Locs[(int)ShaderLocationIndex.VectorView], cameraPos, ShaderUniformDataType.Vec3);

        // Check key inputs to enable/disable lights
        if (IsKeyPressed(KeyboardKey.One))
        {
            _lights[2].Enabled = !_lights[2].Enabled;
        }

        if (IsKeyPressed(KeyboardKey.Two))
        {
            _lights[1].Enabled = !_lights[1].Enabled;
        }

        if (IsKeyPressed(KeyboardKey.Three))
        {
            _lights[3].Enabled = !_lights[3].Enabled;
        }

        if (IsKeyPressed(KeyboardKey.Four))
        {
            _lights[0].Enabled = !_lights[0].Enabled;
        }

        // Update light values on shader (actually, only enable/disable them)
        for (var i = 0; i < 4; i++)
        {
            UpdateLight(_shader, _lights[i]);
        }

        BeginDrawing();

        ClearBackground(Color.Black);

        BeginMode3D(_camera);

        // Set floor model texture tiling and emissive color parameters on shader
        SetShaderValue(_shader, _textureTilingLoc, _floorTextureTiling, ShaderUniformDataType.Vec2);
        var floorEmissiveColor = ColorNormalize(_floor.Materials[0].Maps[(int)MaterialMapIndex.Emission].Color);
        SetShaderValue(_shader, _emissiveColorLoc, floorEmissiveColor, ShaderUniformDataType.Vec4);

        DrawModel(_floor, Vector3.Zero, 5.0f, Color.White); // Draw floor model

        // Set old car model texture tiling, emissive color and emissive intensity parameters on shader
        SetShaderValue(_shader, _textureTilingLoc, _carTextureTiling, ShaderUniformDataType.Vec2);
        var carEmissiveColor = ColorNormalize(_car.Materials[0].Maps[(int)MaterialMapIndex.Emission].Color);
        SetShaderValue(_shader, _emissiveColorLoc, carEmissiveColor, ShaderUniformDataType.Vec4);
        var emissiveIntensity = 0.01f;
        SetShaderValue(_shader, _emissiveIntensityLoc, emissiveIntensity, ShaderUniformDataType.Float);

        DrawModel(_car, Vector3.Zero, 0.25f, Color.White); // Draw car model

        // Draw spheres to show the lights positions
        for (var i = 0; i < 4; i++)
        {
            var color = _lights[i].Color;
            var lightColor = new Color((byte)(color.X * 255), (byte)(color.Y * 255), (byte)(color.Z * 255),
                (byte)(color.W * 255));

            if (_lights[i].Enabled)
            {
                DrawSphereEx(_lights[i].Position, 0.2f, 8, 8, lightColor);
            }
            else
            {
                DrawSphereWires(_lights[i].Position, 0.2f, 8, 8, ColorAlpha(lightColor, 0.3f));
            }
        }

        EndMode3D();

        DrawText("Toggle lights: [1][2][3][4]", 10, 40, 20, Color.LightGray);

        DrawText("(c) Old Rusty Car model by Renafox (https://skfb.ly/LxRy)", screenWidth - 320, screenHeight - 20, 10, Color.LightGray);

        DrawFPS(10, 10);

        EndDrawing();
    }

    public void Unload()
    {
        // Unbind (disconnect) shader from car.material[0]
        // to avoid UnloadMaterial() trying to unload it automatically
        _car.Materials[0].Shader = new();
        UnloadMaterial(_car.Materials[0]);
        _car.Materials[0].Maps = null;
        UnloadModel(_car);

        _floor.Materials[0].Shader = new();
        UnloadMaterial(_floor.Materials[0]);
        _floor.Materials[0].Maps = null;
        UnloadModel(_floor);

        UnloadShader(_shader);
    }

    private static void UpdateLight(Shader shader, PbrLight light)
    {
        SetShaderValue(shader, light.EnabledLoc, light.Enabled ? 1 : 0, ShaderUniformDataType.Int);
        SetShaderValue(shader, light.TypeLoc, (int)light.Type, ShaderUniformDataType.Int);

        // Send to shader light position values
        SetShaderValue(shader, light.PositionLoc, light.Position, ShaderUniformDataType.Vec3);

        // Send to shader light target position values
        SetShaderValue(shader, light.TargetLoc, light.Target, ShaderUniformDataType.Vec3);
        SetShaderValue(shader, light.ColorLoc, light.Color, ShaderUniformDataType.Vec4);
        SetShaderValue(shader, light.IntensityLoc, light.Intensity, ShaderUniformDataType.Float);
    }

    // Inlined minimal equivalent of Examples.Shared.PbrLights (not referenced by Examples.Web)
    private enum PbrLightType
    {
        Directorional,
        Point,
        Spot
    }

    private struct PbrLight
    {
        public PbrLightType Type;
        public bool Enabled;
        public Vector3 Position;
        public Vector3 Target;
        public Vector4 Color;
        public float Intensity;

        public int TypeLoc;
        public int EnabledLoc;
        public int PositionLoc;
        public int TargetLoc;
        public int ColorLoc;
        public int IntensityLoc;
    }

    private static PbrLight CreateLight(
        int lightsCount,
        PbrLightType type,
        Vector3 pos,
        Vector3 target,
        Color color,
        float intensity,
        Shader shader
    )
    {
        PbrLight light = new();

        light.Enabled = true;
        light.Type = type;
        light.Position = pos;
        light.Target = target;
        light.Color = new Vector4(
            color.R / 255.0f,
            color.G / 255.0f,
            color.B / 255.0f,
            color.A / 255.0f
        );
        light.Intensity = intensity;

        light.EnabledLoc = GetShaderLocation(shader, "lights[" + lightsCount + "].enabled");
        light.TypeLoc = GetShaderLocation(shader, "lights[" + lightsCount + "].type");
        light.PositionLoc = GetShaderLocation(shader, "lights[" + lightsCount + "].position");
        light.TargetLoc = GetShaderLocation(shader, "lights[" + lightsCount + "].target");
        light.ColorLoc = GetShaderLocation(shader, "lights[" + lightsCount + "].color");
        light.IntensityLoc = GetShaderLocation(shader, "lights[" + lightsCount + "].intensity");

        UpdateLight(shader, light);

        return light;
    }
}
