#if BROWSER
using Examples;
using System;

namespace Examples.Core;

public partial class VrSimulator : IExample
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
        public string Name => "Core / VR Simulator";

        private VrStereoConfig _config;
        private Shader _distortion;
        private RenderTexture2D _target;
        private Rectangle _sourceRec;
        private Rectangle _destRec;
        private Camera3D _camera;
        private Vector3 _cubePosition;

        public void Init()
        {
            // VR device parameters definition
            VrDeviceInfo device = new VrDeviceInfo
            {
                // Oculus Rift CV1 parameters for simulator
                HResolution = 2160,
                VResolution = 1200,
                HScreenSize = 0.133793f,
                VScreenSize = 0.0669f,
                EyeToScreenDistance = 0.041f,
                LensSeparationDistance = 0.07f,
                InterpupillaryDistance = 0.07f,
            };

            // NOTE: CV1 uses fresnel-hybrid-asymmetric lenses with specific compute shaders
            // Following parameters are just an approximation to CV1 distortion stereo rendering
            device.LensDistortionValues[0] = 1.0f;
            device.LensDistortionValues[1] = 0.22f;
            device.LensDistortionValues[2] = 0.24f;
            device.LensDistortionValues[3] = 0.0f;
            device.ChromaAbCorrection[0] = 0.996f;
            device.ChromaAbCorrection[1] = -0.004f;
            device.ChromaAbCorrection[2] = 1.014f;
            device.ChromaAbCorrection[3] = 0.0f;

            // Load VR stereo config for VR device parameteres (Oculus Rift CV1 parameters)
            _config = LoadVrStereoConfig(device);

            // Distortion shader (uses device lens distortion and chroma)
            _distortion = LoadShader(null, "resources/shaders/glsl100/distortion100.fs");

            // Update distortion shader with lens and distortion-scale parameters
            SetShaderValue(
                _distortion,
                GetShaderLocation(_distortion, "leftLensCenter"),
                _config.LeftLensCenter,
                ShaderUniformDataType.Vec2
            );
            SetShaderValue(
                _distortion,
                GetShaderLocation(_distortion, "rightLensCenter"),
                _config.RightLensCenter,
                ShaderUniformDataType.Vec2
            );
            SetShaderValue(
                _distortion,
                GetShaderLocation(_distortion, "leftScreenCenter"),
                _config.LeftScreenCenter,
                ShaderUniformDataType.Vec2
            );
            SetShaderValue(
                _distortion,
                GetShaderLocation(_distortion, "rightScreenCenter"),
                _config.RightScreenCenter,
                ShaderUniformDataType.Vec2
            );

            SetShaderValue(
                _distortion,
                GetShaderLocation(_distortion, "scale"),
                _config.Scale,
                ShaderUniformDataType.Vec2
            );
            SetShaderValue(
                _distortion,
                GetShaderLocation(_distortion, "scaleIn"),
                _config.ScaleIn,
                ShaderUniformDataType.Vec2
            );

            SetShaderValue(
                _distortion,
                GetShaderLocation(_distortion, "deviceWarpParam"),
                device.LensDistortionValues,
                ShaderUniformDataType.Vec4
            );
            SetShaderValue(
                _distortion,
                GetShaderLocation(_distortion, "chromaAbParam"),
                device.ChromaAbCorrection,
                ShaderUniformDataType.Vec4
            );

            // Initialize framebuffer for stereo rendering
            // NOTE: Screen size should match HMD aspect ratio
            _target = LoadRenderTexture(device.HResolution, device.VResolution);

            // The target's height is flipped (in the source Rectangle), due to OpenGL reasons
            _sourceRec = new Rectangle(0.0f, 0.0f, (float)_target.Texture.Width, -(float)_target.Texture.Height);
            _destRec = new Rectangle(0.0f, 0.0f, (float)GetScreenWidth(), (float)GetScreenHeight());

            // Define the camera to look into our 3d world
            _camera = new();
            _camera.Position = new Vector3(5.0f, 2.0f, 5.0f);
            _camera.Target = new Vector3(0.0f, 2.0f, 0.0f);
            _camera.Up = new Vector3(0.0f, 1.0f, 0.0f);
            _camera.FovY = 60.0f;
            _camera.Projection = CameraProjection.Perspective;

            _cubePosition = new(0.0f, 0.0f, 0.0f);
        }

        public void Update()
        {
            UpdateCamera(ref _camera, CameraMode.FirstPerson);

            BeginTextureMode(_target);
            ClearBackground(Color.RayWhite);
            BeginVrStereoMode(_config);
            BeginMode3D(_camera);

            DrawCube(_cubePosition, 2.0f, 2.0f, 2.0f, Color.Red);
            DrawCubeWires(_cubePosition, 2.0f, 2.0f, 2.0f, Color.Maroon);
            DrawGrid(40, 1.0f);

            EndMode3D();
            EndVrStereoMode();
            EndTextureMode();

            BeginDrawing();
            ClearBackground(Color.RayWhite);
            BeginShaderMode(_distortion);
            DrawTexturePro(_target.Texture, _sourceRec, _destRec, new Vector2(0.0f, 0.0f), 0.0f, Color.White);
            EndShaderMode();
            DrawFPS(10, 10);
            EndDrawing();
        }

        public void Unload()
        {
            UnloadVrStereoConfig(_config);
            UnloadRenderTexture(_target);
            UnloadShader(_distortion);
        }
    }
}
#endif
