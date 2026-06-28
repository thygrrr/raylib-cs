#if BROWSER
using Examples;
using System;

namespace Examples.Shaders;

public partial class HybridRender : IExample
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
        public string Name => "Shaders / Hybrid Render";

        private const int GLSL_VERSION = 100;

        private const int screenWidth = 800;
        private const int screenHeight = 450;

        private struct RayLocs
        {
            public int CamPos;
            public int CamDir;
            public int ScreenCenter;
        }

        private Shader _shdrRaymarch;
        private Shader _shdrRaster;
        private RayLocs _marchLocs;
        private RenderTexture2D _target;
        private Camera3D _camera;
        private float _camDist;

        public void Init()
        {
            // This Shader calculates pixel depth and color using raymarch
            _shdrRaymarch = LoadShader(null, $"resources/shaders/glsl{GLSL_VERSION}/hybrid_raymarch.fs");

            // This Shader is a standard rasterization fragment shader with the addition of depth writing
            // You are required to write depth for all shaders if one shader does it
            _shdrRaster = LoadShader(null, $"resources/shaders/glsl{GLSL_VERSION}/hybrid_raster.fs");

            // Declare Struct used to store camera locs
            _marchLocs = new();

            // Fill the struct with shader locs
            _marchLocs.CamPos = GetShaderLocation(_shdrRaymarch, "camPos");
            _marchLocs.CamDir = GetShaderLocation(_shdrRaymarch, "camDir");
            _marchLocs.ScreenCenter = GetShaderLocation(_shdrRaymarch, "screenCenter");

            // Transfer screenCenter position to shader. Which is used to calculate ray direction
            Vector2 screenCenter = new(screenWidth / 2.0f, screenHeight / 2.0f);
            SetShaderValue(
                _shdrRaymarch,
                _marchLocs.ScreenCenter,
                screenCenter,
                ShaderUniformDataType.Vec2
            );

            // Use Customized function to create writable depth texture buffer
            _target = LoadRenderTextureDepthTex(screenWidth, screenHeight);

            // Define the camera to look into our 3d world
            _camera = new();
            _camera.Position = new Vector3(0.5f, 1.0f, 1.5f);
            _camera.Target = new Vector3(0.0f, 0.5f, 0.0f);
            _camera.Up = new Vector3(0.0f, 1.0f, 0.0f);
            _camera.FovY = 45.0f;
            _camera.Projection = CameraProjection.Perspective;

            // Camera FOV is pre-calculated in the camera distance
            _camDist = 1.0f / (MathF.Tan(_camera.FovY * 0.5f * DEG2RAD));
        }

        public void Update()
        {
            UpdateCamera(ref _camera, CameraMode.Orbital);

            // Update Camera Postion in the ray march shader
            SetShaderValue(
                _shdrRaymarch,
                _marchLocs.CamPos,
                _camera.Position,
                ShaderUniformDataType.Vec3
            );

            // Update Camera Looking Vector. Vector length determines FOV
            Vector3 camDir = Vector3.Normalize(_camera.Target - _camera.Position) * _camDist;
            SetShaderValue(_shdrRaymarch, _marchLocs.CamDir, camDir, ShaderUniformDataType.Vec3);

            // Draw into our custom render texture (framebuffer)
            BeginTextureMode(_target);
            ClearBackground(Color.White);

            // Raymarch Scene
            // Manually enable Depth Test to handle multiple rendering methods.
            Rlgl.EnableDepthTest();
            BeginShaderMode(_shdrRaymarch);
            DrawRectangleRec(new Rectangle(0, 0, screenWidth, screenHeight), Color.White);
            EndShaderMode();

            // Rasterize Scene
            BeginMode3D(_camera);
            BeginShaderMode(_shdrRaster);
            DrawCubeWiresV(new Vector3(0.0f, 0.5f, 1.0f), new Vector3(1.0f, 1.0f, 1.0f), Color.Red);
            DrawCubeV(new Vector3(0.0f, 0.5f, 1.0f), new Vector3(1.0f, 1.0f, 1.0f), Color.Purple);
            DrawCubeWiresV(new Vector3(0.0f, 0.5f, -1.0f), new Vector3(1.0f, 1.0f, 1.0f), Color.DarkGreen);
            DrawCubeV(new Vector3(0.0f, 0.5f, -1.0f), new Vector3(1.0f, 1.0f, 1.0f), Color.Yellow);
            DrawGrid(10, 1.0f);
            EndShaderMode();
            EndMode3D();

            EndTextureMode();

            // Draw custom render texture
            BeginDrawing();
            ClearBackground(Color.RayWhite);

            DrawTextureRec(
                _target.Texture,
                new Rectangle(0, 0, screenWidth, -screenHeight),
                Vector2.Zero,
                Color.White
            );
            DrawFPS(10, 10);

            EndDrawing();
        }

        public void Unload()
        {
            UnloadRenderTextureDepthTex(_target);
            UnloadShader(_shdrRaymarch);
            UnloadShader(_shdrRaster);
        }

        // Load custom render texture, create a writable depth texture buffer
        private static RenderTexture2D LoadRenderTextureDepthTex(int width, int height)
        {
            RenderTexture2D target = new();

            // Load an empty framebuffer
            target.Id = Rlgl.LoadFramebuffer();

            if (target.Id > 0)
            {
                Rlgl.EnableFramebuffer(target.Id);

                // Create color texture (default to RGBA)
                target.Texture.Id = Rlgl.LoadTexture(
                    null,
                    width,
                    height,
                    PixelFormat.UncompressedR8G8B8A8,
                    1
                );
                target.Texture.Width = width;
                target.Texture.Height = height;
                target.Texture.Format = PixelFormat.UncompressedR8G8B8A8;
                target.Texture.Mipmaps = 1;

                // Create depth texture buffer (instead of raylib default renderbuffer)
                target.Depth.Id = Rlgl.LoadTextureDepth(width, height, false);
                target.Depth.Width = width;
                target.Depth.Height = height;
                target.Depth.Format = PixelFormat.CompressedPvrtRgba;
                target.Depth.Mipmaps = 1;

                // Attach color texture and depth texture to FBO
                Rlgl.FramebufferAttach(
                    target.Id,
                    target.Texture.Id,
                    FramebufferAttachType.ColorChannel0,
                    FramebufferAttachTextureType.Texture2D,
                    0
                );
                Rlgl.FramebufferAttach(
                    target.Id,
                    target.Depth.Id,
                    FramebufferAttachType.Depth,
                    FramebufferAttachTextureType.Texture2D,
                    0
                );

                // Check if fbo is complete with attachments (valid)
                if (Rlgl.FramebufferComplete(target.Id))
                {
                    TraceLog(TraceLogLevel.Info, $"FBO: [ID {target.Id}] Framebuffer object created successfully");
                }

                Rlgl.DisableFramebuffer();
            }
            else
            {
                TraceLog(TraceLogLevel.Warning, "FBO: Framebuffer object can not be created");
            }

            return target;
        }

        // Unload render texture from GPU memory (VRAM)
        private static void UnloadRenderTextureDepthTex(RenderTexture2D target)
        {
            if (target.Id > 0)
            {
                // Color texture attached to FBO is deleted
                Rlgl.UnloadTexture(target.Texture.Id);
                Rlgl.UnloadTexture(target.Depth.Id);

                // NOTE: Depth texture is automatically
                // queried and deleted before deleting framebuffer
                Rlgl.UnloadFramebuffer(target.Id);
            }
        }
    }
}
#endif
