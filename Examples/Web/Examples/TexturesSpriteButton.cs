// Adapted for the browser from Examples/Textures/SpriteButton.cs
namespace Examples.Web;

public class TexturesSpriteButton : IWebExample
{
    public string Name => "Textures / Sprite Button";

    // Number of frames (rectangles) for the button sprite texture
    private const int NumFrames = 3;
    private const int screenWidth = 800;
    private const int screenHeight = 450;

    private Sound _fxButton;
    private Texture2D _button;

    private int _frameHeight;
    private Rectangle _sourceRec;
    private Rectangle _btnBounds;
    // Button state: 0-NORMAL, 1-MOUSE_HOVER, 2-PRESSED
    private int _btnState;

    public void Init()
    {
        InitAudioDevice();

        _fxButton = LoadSound("resources/audio/buttonfx.wav");
        _button = LoadTexture("resources/button.png");

        // Define frame rectangle for drawing
        _frameHeight = _button.Height / NumFrames;
        _sourceRec = new(0, 0, _button.Width, _frameHeight);

        // Define button bounds on screen
        _btnBounds = new(
            screenWidth / 2 - _button.Width / 2,
            screenHeight / 2 - _button.Height / NumFrames / 2,
            _button.Width,
            _frameHeight
        );

        _btnState = 0;
    }

    public void Update()
    {
        Vector2 mousePoint = GetMousePosition();
        bool btnAction = false;

        // Check button state
        if (CheckCollisionPointRec(mousePoint, _btnBounds))
        {
            if (IsMouseButtonDown(MouseButton.Left))
            {
                _btnState = 2;
            }
            else
            {
                _btnState = 1;
            }

            if (IsMouseButtonReleased(MouseButton.Left))
            {
                btnAction = true;
            }
        }
        else
        {
            _btnState = 0;
        }

        if (btnAction)
        {
            PlaySound(_fxButton);
            // TODO: Any desired action
        }

        // Calculate button frame rectangle to draw depending on button state
        _sourceRec.Y = _btnState * _frameHeight;

        BeginDrawing();
        ClearBackground(Color.RayWhite);

        // Draw button frame
        DrawTextureRec(_button, _sourceRec, new Vector2(_btnBounds.X, _btnBounds.Y), Color.White);

        EndDrawing();
    }

    public void Unload()
    {
        UnloadTexture(_button);
        UnloadSound(_fxButton);

        CloseAudioDevice();
    }
}
