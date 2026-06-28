#if BROWSER
using Examples;
namespace Examples.Textures;

public partial class SrcRecDstRec : IExample
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

    private sealed class BrowserAdapter : IExample
    {
        public string Name => "Textures / Src and Dst Rectangles";

        private const int screenWidth = 800;
        private const int screenHeight = 450;

        private Texture2D _scarfy;
        private Rectangle _sourceRec;
        private Rectangle _destRec;
        private Vector2 _origin;
        private int _rotation;

        public void Init()
        {
            // NOTE: Textures MUST be loaded after Window initialization (OpenGL context is required)
            _scarfy = LoadTexture("resources/scarfy.png");

            int frameWidth = _scarfy.Width / 6;
            int frameHeight = _scarfy.Height;

            // NOTE: Source rectangle (part of the texture to use for drawing)
            _sourceRec = new(0, 0, frameWidth, frameHeight);

            // NOTE: Destination rectangle (screen rectangle where drawing part of texture)
            _destRec = new(screenWidth / 2, screenHeight / 2, frameWidth * 2, frameHeight * 2);

            // NOTE: Origin of the texture (rotation/scale point), it's relative to destination rectangle size
            _origin = new(frameWidth, frameHeight);

            _rotation = 0;
        }

        public void Update()
        {
            _rotation++;

            BeginDrawing();
            ClearBackground(Color.RayWhite);

            // NOTE: Using DrawTexturePro() we can easily rotate and scale the part of the texture we draw
            // sourceRec defines the part of the texture we use for drawing
            // destRec defines the rectangle where our texture part will fit (scaling it to fit)
            // origin defines the point of the texture used as reference for rotation and scaling
            // rotation defines the texture rotation (using origin as rotation point)
            DrawTexturePro(_scarfy, _sourceRec, _destRec, _origin, _rotation, Color.White);

            DrawLine((int)_destRec.X, 0, (int)_destRec.X, screenHeight, Color.Gray);
            DrawLine(0, (int)_destRec.Y, screenWidth, (int)_destRec.Y, Color.Gray);

            DrawText("(c) Scarfy sprite by Eiden Marsal", screenWidth - 200, screenHeight - 20, 10, Color.Gray);

            EndDrawing();
        }

        public void Unload()
        {
            UnloadTexture(_scarfy);
        }
    }
}
#endif
