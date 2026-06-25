// Adapted for the browser from Examples/Core/SmoothPixelperfect.cs
namespace Examples.Web;

public class CoreSmoothPixelperfect : IWebExample
{
    public string Name => "Core / Smooth Pixelperfect";

    private const int screenWidth = 800;
    private const int screenHeight = 450;

    private const int virtualscreenWidth = 160;
    private const int virtualscreenHeight = 90;

    private const float virtualRatio = (float)screenWidth / (float)virtualscreenWidth;

    private Camera2D _worldSpaceCamera;
    private Camera2D _screenSpaceCamera;
    private RenderTexture2D _target;

    private Rectangle _rec01;
    private Rectangle _rec02;
    private Rectangle _rec03;

    private Rectangle _sourceRec;
    private Rectangle _destRec;

    private Vector2 _origin;
    private float _rotation;
    private float _cameraX;
    private float _cameraY;

    public void Init()
    {
        // Game world camera
        _worldSpaceCamera = new();
        _worldSpaceCamera.Zoom = 1.0f;

        // Smoothing camera
        _screenSpaceCamera = new();
        _screenSpaceCamera.Zoom = 1.0f;

        // This is where we'll draw all our objects.
        _target = LoadRenderTexture(virtualscreenWidth, virtualscreenHeight);

        _rec01 = new(70.0f, 35.0f, 20.0f, 20.0f);
        _rec02 = new(90.0f, 55.0f, 30.0f, 10.0f);
        _rec03 = new(80.0f, 65.0f, 15.0f, 25.0f);

        // The target's height is flipped (in the source Rectangle), due to OpenGL reasons
        _sourceRec = new(
            0.0f,
            0.0f,
            (float)_target.Texture.Width,
            -(float)_target.Texture.Height
        );
        _destRec = new(
            -virtualRatio,
            -virtualRatio,
            screenWidth + (virtualRatio * 2),
            screenHeight + (virtualRatio * 2)
        );

        _origin = new(0.0f, 0.0f);

        _rotation = 0.0f;

        _cameraX = 0.0f;
        _cameraY = 0.0f;
    }

    public void Update()
    {
        _rotation += 60.0f * GetFrameTime();   // Rotate the rectangles, 60 degrees per second

        // Make the camera move to demonstrate the effect
        _cameraX = (MathF.Sin((float)GetTime()) * 50.0f) - 10.0f;
        _cameraY = MathF.Cos((float)GetTime()) * 30.0f;

        // Set the camera's target to the values computed above
        _screenSpaceCamera.Target = new Vector2(_cameraX, _cameraY);

        // Round worldSpace coordinates, keep decimals into screenSpace coordinates
        _worldSpaceCamera.Target.X = (int)_screenSpaceCamera.Target.X;
        _screenSpaceCamera.Target.X -= _worldSpaceCamera.Target.X;
        _screenSpaceCamera.Target.X *= virtualRatio;

        _worldSpaceCamera.Target.Y = (int)_screenSpaceCamera.Target.Y;
        _screenSpaceCamera.Target.Y -= _worldSpaceCamera.Target.Y;
        _screenSpaceCamera.Target.Y *= virtualRatio;

        BeginTextureMode(_target);
        ClearBackground(Color.RayWhite);

        BeginMode2D(_worldSpaceCamera);
        DrawRectanglePro(_rec01, _origin, _rotation, Color.Black);
        DrawRectanglePro(_rec02, _origin, -_rotation, Color.Red);
        DrawRectanglePro(_rec03, _origin, _rotation + 45.0f, Color.Blue);
        EndMode2D();

        EndTextureMode();

        BeginDrawing();
        ClearBackground(Color.Red);

        BeginMode2D(_screenSpaceCamera);
        DrawTexturePro(_target.Texture, _sourceRec, _destRec, _origin, 0.0f, Color.White);
        EndMode2D();

        DrawText($"Screen resolution: {screenWidth}x{screenHeight}", 10, 10, 20, Color.DarkBlue);
        DrawText($"World resolution: {virtualscreenWidth}x{virtualscreenHeight}", 10, 40, 20, Color.DarkGreen);
        DrawFPS(GetScreenWidth() - 95, 10);
        EndDrawing();
    }

    public void Unload()
    {
        UnloadRenderTexture(_target);
    }
}
