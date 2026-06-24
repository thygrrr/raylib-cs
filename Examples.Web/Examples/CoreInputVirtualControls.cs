// Adapted for the browser from Examples/Core/InputVirtualControls.cs
namespace Examples.Web;

public class CoreInputVirtualControls : IWebExample
{
    public string Name => "Core / Input Virtual Controls";

    private const int ScreenWidth = 800;
    private const int ScreenHeight = 450;

    private enum PadButton
    {
        BUTTON_NONE = -1,
        BUTTON_UP,
        BUTTON_LEFT,
        BUTTON_RIGHT,
        BUTTON_DOWN,
        BUTTON_MAX
    }

    private Vector2 _padPosition;
    private float _buttonRadius;
    private Vector2[] _buttonPositions;
    private Vector2[][] _arrowTris;
    private Color[] _buttonLabelColors;
    private int _pressedButton;
    private Vector2 _inputPosition;
    private Vector2 _playerPosition;
    private float _playerSpeed;

    public void Init()
    {
        _padPosition = new Vector2(100, 350);
        _buttonRadius = 30;

        _buttonPositions =
        [
            new Vector2(
                _padPosition.X, _padPosition.Y - _buttonRadius * 1.5f
            ), // Up
            new Vector2(
                _padPosition.X - _buttonRadius * 1.5f, _padPosition.Y
            ), // Left
            new Vector2(
                _padPosition.X + _buttonRadius * 1.5f, _padPosition.Y
            ), // Right
            new Vector2(
                _padPosition.X, _padPosition.Y + _buttonRadius * 1.5f
            ) // Down
        ];

        _arrowTris =
        [
            // Up
            [
                new Vector2(
                    _buttonPositions[0].X, _buttonPositions[0].Y - 12
                ),
                new Vector2(
                    _buttonPositions[0].X - 9, _buttonPositions[0].Y + 9
                ),
                new Vector2(
                    _buttonPositions[0].X + 9, _buttonPositions[0].Y + 9
                )
            ],
            // Left
            [
                new Vector2(
                    _buttonPositions[1].X + 9, _buttonPositions[1].Y - 9
                ),
                new Vector2(
                    _buttonPositions[1].X - 12, _buttonPositions[1].Y
                ),
                new Vector2(
                    _buttonPositions[1].X + 9, _buttonPositions[1].Y + 9
                )
            ],
            // Right
            [
                new Vector2(
                    _buttonPositions[2].X + 12, _buttonPositions[2].Y
                ),
                new Vector2(
                    _buttonPositions[2].X - 9, _buttonPositions[2].Y - 9
                ),
                new Vector2(
                    _buttonPositions[2].X - 9, _buttonPositions[2].Y + 9
                )
            ],
            // Down
            [
                new Vector2(
                    _buttonPositions[3].X - 9, _buttonPositions[3].Y - 9
                ),
                new Vector2(
                    _buttonPositions[3].X, _buttonPositions[3].Y + 12
                ),
                new Vector2(
                    _buttonPositions[3].X + 9, _buttonPositions[3].Y - 9
                )
            ]
        ];

        _buttonLabelColors =
        [
            Color.Yellow, // Up
            Color.Blue, // Left
            Color.Red, // Right
            Color.Green // Down
        ];

        _pressedButton = (int)PadButton.BUTTON_NONE;
        _inputPosition = new Vector2(0, 0);

        _playerPosition = new Vector2((float)ScreenWidth / 2, (float)ScreenHeight / 2);
        _playerSpeed = 75f;
    }

    public void Update()
    {
        if (GetTouchPointCount() > 0)
        {
            _inputPosition = GetTouchPosition(0); // Use touch position
        }
        else
        {
            _inputPosition = GetMousePosition(); // Use mouse position
        }

        // Reset pressed button to none
        _pressedButton = (int)PadButton.BUTTON_NONE;

        // Make sure user is pressing left mouse button if they're from desktop
        if ((GetTouchPointCount() > 0) ||
            ((GetTouchPointCount() == 0) && IsMouseButtonDown(MouseButton.Left)))
        {
            // Find nearest D-Pad button to the input position
            for (int i = 0; i < (int)PadButton.BUTTON_MAX; i++)
            {
                float distX = MathF.Abs(_buttonPositions[i].X - _inputPosition.X);
                float distY = MathF.Abs(_buttonPositions[i].Y - _inputPosition.Y);

                if (distX + distY < _buttonRadius)
                {
                    _pressedButton = i;
                    break;
                }
            }
        }

        // Move player according to pressed button
        switch ((PadButton)_pressedButton)
        {
            case PadButton.BUTTON_UP:
                _playerPosition.Y -= _playerSpeed * GetFrameTime();
                break;
            case PadButton.BUTTON_LEFT:
                _playerPosition.X -= _playerSpeed * GetFrameTime();
                break;
            case PadButton.BUTTON_RIGHT:
                _playerPosition.X += _playerSpeed * GetFrameTime();
                break;
            case PadButton.BUTTON_DOWN:
                _playerPosition.Y += _playerSpeed * GetFrameTime();
                break;
            default:
                break;
        }

        BeginDrawing();

        ClearBackground(Color.RayWhite);

        // Draw world
        DrawCircleV(_playerPosition, 50, Color.Maroon);

        // Draw GUI
        for (int i = 0; i < (int)PadButton.BUTTON_MAX; i++)
        {
            DrawCircleV(_buttonPositions[i], _buttonRadius, (i == _pressedButton) ? Color.DarkGray : Color.Black);

            DrawTriangle(
                _arrowTris[i][0],
                _arrowTris[i][1],
                _arrowTris[i][2],
                _buttonLabelColors[i]
            );
        }

        DrawText("move the player with D-Pad buttons", 10, 10, 20, Color.DarkGray);

        EndDrawing();
    }

    public void Unload()
    {
    }
}
