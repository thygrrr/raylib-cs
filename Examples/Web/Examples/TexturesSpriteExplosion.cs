// Adapted for the browser from Examples/Textures/SpriteExplosion.cs
namespace Examples.Web;

public class TexturesSpriteExplosion : IWebExample
{
    public string Name => "Textures / Sprite Explosion";

    private const int NumFramesPerLine = 5;
    private const int NumLines = 5;

    private Sound _fxBoom;
    private Texture2D _explosion;

    // Sprite one frame rectangle width / height
    private int _frameWidth;
    private int _frameHeight;
    private int _currentFrame;
    private int _currentLine;
    private Rectangle _frameRec;
    private Vector2 _position;
    private bool _active;
    private int _framesCounter;

    public void Init()
    {
        InitAudioDevice();

        // Load explosion sound
        _fxBoom = LoadSound("resources/audio/boom.wav");

        // Load explosion texture
        _explosion = LoadTexture("resources/explosion.png");

        // Init variables for animation
        _frameWidth = _explosion.Width / NumFramesPerLine;
        _frameHeight = _explosion.Height / NumLines;

        _currentFrame = 0;
        _currentLine = 0;

        _frameRec = new(0, 0, _frameWidth, _frameHeight);
        _position = new(0.0f, 0.0f);

        _active = false;
        _framesCounter = 0;
    }

    public void Update()
    {
        // Check for mouse button pressed and activate explosion (if not active)
        if (IsMouseButtonPressed(MouseButton.Left) && !_active)
        {
            _position = GetMousePosition();
            _active = true;

            _position.X -= _frameWidth / 2;
            _position.Y -= _frameHeight / 2;

            PlaySound(_fxBoom);
        }

        // Compute explosion animation frames
        if (_active)
        {
            _framesCounter++;

            if (_framesCounter > 2)
            {
                _currentFrame++;

                if (_currentFrame >= NumFramesPerLine)
                {
                    _currentFrame = 0;
                    _currentLine++;

                    if (_currentLine >= NumLines)
                    {
                        _currentLine = 0;
                        _active = false;
                    }
                }

                _framesCounter = 0;
            }
        }

        _frameRec.X = _frameWidth * _currentFrame;
        _frameRec.Y = _frameHeight * _currentLine;

        BeginDrawing();
        ClearBackground(Color.RayWhite);

        // Draw explosion required frame rectangle
        if (_active)
        {
            DrawTextureRec(_explosion, _frameRec, _position, Color.White);
        }

        EndDrawing();
    }

    public void Unload()
    {
        UnloadTexture(_explosion);
        UnloadSound(_fxBoom);

        CloseAudioDevice();
    }
}
