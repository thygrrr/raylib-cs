#if BROWSER
using Examples;
namespace Examples.Shaders;

public partial class WriteDepth : IExample
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
        public string Name => "Shaders / Write Depth";

        private const int GLSL_VERSION = 100;

        private const int screenWidth = 800;
        private const int screenHeight = 450;

        private Shader _shader;
        private RenderTexture2D _target;
        private Camera3D _camera;

        public void Init()
        {
            // The shader inverts the depth buffer by writing into it by `gl_FragDepth = 1 - gl_FragCoord.z;`
            _shader = LoadShader(null, $"resources/shaders/glsl{GLSL_VERSION}/write_depth.fs");

            // Use customized function to create writable depth texture buffer
            _target = LoadRenderTextureDepthTex(screenWidth, screenHeight);

            // Define the camera to look into our 3d world
            _camera = new();
            _camera.Position = new Vector3(2.0f, 2.0f, 3.0f);
            _camera.Target = new Vector3(0.0f, 0.5f, 0.0f);
            _camera.Up = new Vector3(0.0f, 1.0f, 0.0f);
            _camera.FovY = 45.0f;
            _camera.Projection = CameraProjection.Perspective;
        }

        public void Update()
        {
            UpdateCamera(ref _camera, CameraMode.Orbital);

            // Draw into our custom render texture (framebuffer)
            BeginTextureMode(_target);
            ClearBackground(Color.White);

            BeginMode3D(_camera);
            BeginShaderMode(_shader);

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
            UnloadShader(_shader);
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
