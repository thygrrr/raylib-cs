// Adapted for the browser from Examples/Core/InputGesturesTestBed.cs
namespace Examples.Web;

public class CoreInputGesturesTestBed : IWebExample
{
    public string Name => "Core / Input Gestures Test Bed";

    private const int GESTURE_LOG_SIZE = 20;
    private const int MAX_TOUCH_COUNT = 32;

    private Vector2 _messagePosition;

    // Last gesture variables
    private Gesture _lastGesture;
    private Vector2 _lastGesturePosition;

    // Gesture log variables
    // NOTE: The gesture log uses an array (as an inverted circular queue) to store the performed gestures
    private string[] _gestureLog;
    // NOTE: The index for the inverted circular queue (moving from last to first direction, then looping around)
    private int _gestureLogIndex;
    private Gesture _previousGesture;

    // Log mode values:
    // - 0 shows repeated events
    // - 1 hides repeated events
    // - 2 shows repeated events but hide hold events
    // - 3 hides repeated events and hide hold events
    private int _logMode;

    private Color _gestureColor;
    private Rectangle _logButton1;
    private Rectangle _logButton2;
    private Vector2 _gestureLogPosition;

    // Protractor variables
    private float _angleLength;
    private float _currentAngleDegrees;
    private Vector2 _finalVector;
    private Vector2 _protractorPosition;

    public void Init()
    {
        _messagePosition = new Vector2(160, 7);

        _lastGesture = 0;
        _lastGesturePosition = new Vector2(165, 130);

        _gestureLog = new string[GESTURE_LOG_SIZE + 1];
        for (int i = 0; i < GESTURE_LOG_SIZE; i++)
        {
            _gestureLog[i] = new string(new char[12]);
        }

        _gestureLogIndex = GESTURE_LOG_SIZE;
        _previousGesture = 0;

        _logMode = 1;

        _gestureColor = new Color(0, 0, 0, 255);
        _logButton1 = new Rectangle(53, 7, 48, 26);
        _logButton2 = new Rectangle(108, 7, 36, 26);
        _gestureLogPosition = new Vector2(10, 10);

        _angleLength = 90.0f;
        _currentAngleDegrees = 0.0f;
        _finalVector = new Vector2(0.0f, 0.0f);
        _protractorPosition = new Vector2(266.0f, 315.0f);
    }

    public void Update()
    {
        // Handle common gestures data
        int i, ii; // Iterators that will be reused by all for loops
        Gesture currentGesture = GetGestureDetected();
        float currentDragDegrees = GetGestureDragAngle();
        float currentPitchDegrees = GetGesturePinchAngle();
        int touchCount = GetTouchPointCount();

        // Handle last gesture
        if ((currentGesture != 0) && ((int)currentGesture != 4) && (currentGesture != _previousGesture))
        {
            _lastGesture = currentGesture; // Filter the meaningful gestures (1, 2, 8 to 512) for the display
        }

        // Handle gesture log
        if (IsMouseButtonReleased(MouseButton.Left))
        {
            if (CheckCollisionPointRec(GetMousePosition(), _logButton1))
            {
                switch (_logMode)
                {
                    case 3:
                        _logMode = 2;
                        break;
                    case 2:
                        _logMode = 3;
                        break;
                    case 1:
                        _logMode = 0;
                        break;
                    default:
                        _logMode = 1;
                        break;
                }
            }
            else if (CheckCollisionPointRec(GetMousePosition(), _logButton2))
            {
                switch (_logMode)
                {
                    case 3:
                        _logMode = 1;
                        break;
                    case 2:
                        _logMode = 0;
                        break;
                    case 1:
                        _logMode = 3;
                        break;
                    default:
                        _logMode = 2;
                        break;
                }
            }
        }

        int fillLog = 0; // Gate variable to be used to allow or not the gesture log to be filled
        if (currentGesture != 0)
        {
            if (_logMode == 3) // 3 hides repeated events and hide hold events
            {
                if ((((int)currentGesture != 4) && (currentGesture != _previousGesture)) || ((int)currentGesture < 3))
                {
                    fillLog = 1;
                }
            }
            else if (_logMode == 2) // 2 shows repeated events but hide hold events
            {
                if ((int)currentGesture != 4)
                {
                    fillLog = 1;
                }
            }
            else if (_logMode == 1) // 1 hides repeated events
            {
                if (currentGesture != _previousGesture)
                {
                    fillLog = 1;
                }
            }
            else // 0 shows repeated events
            {
                fillLog = 1;
            }
        }

        if (fillLog > 0) // If one of the conditions from logMode was met, fill the gesture log
        {
            _previousGesture = currentGesture;
            _gestureColor = GetGestureColor((int)currentGesture);
            if (_gestureLogIndex <= 0)
            {
                _gestureLogIndex = GESTURE_LOG_SIZE;
            }
            _gestureLogIndex--;

            // Copy the gesture respective name to the gesture log array
            _gestureLog[_gestureLogIndex] = GetGestureName((int)currentGesture);
        }

        // Handle protractor
        if ((int)currentGesture > 255)
        {
            _currentAngleDegrees = currentPitchDegrees; // Pinch In and Pinch Out
        }
        else if ((int)currentGesture > 15)
        {
            _currentAngleDegrees = currentDragDegrees; // Swipe Right, Swipe Left, Swipe Up and Swipe Down
        }
        else if (currentGesture > 0)
        {
            _currentAngleDegrees = 0.0f; // Tap, Doubletap, Hold and Grab
        }

        float currentAngleRadians =
            ((_currentAngleDegrees + 90.0f) * MathF.PI / 180); // Convert the current angle to Radians
        // Calculate the final vector for display
        _finalVector = new Vector2(
            (_angleLength * MathF.Sin(currentAngleRadians)) + _protractorPosition.X,
            (_angleLength * MathF.Cos(currentAngleRadians)) + _protractorPosition.Y
        );

        // Handle touch and mouse pointer points
        Vector2[] touchPosition = new Vector2[MAX_TOUCH_COUNT];

        Vector2 mousePosition = Vector2.Zero;
        if (currentGesture != Gesture.None)
        {
            if (touchCount != 0)
            {
                for (i = 0; i < touchCount; i++)
                {
                    touchPosition[i] = GetTouchPosition(i); // Fill the touch positions
                }
            }
            else
            {
                mousePosition = GetMousePosition();
            }
        }

        BeginDrawing();
        ClearBackground(Color.RayWhite);

        // Draw common elements
        DrawText("*", (int)_messagePosition.X + 5, (int)_messagePosition.Y + 5, 10, Color.Black);
        DrawText("Example optimized for Web/HTML5\non Smartphones with Touch Screen.", (int)_messagePosition.X + 15,
            (int)_messagePosition.Y + 5, 10, Color.Black);
        DrawText("*", (int)_messagePosition.X + 5, (int)_messagePosition.Y + 35, 10, Color.Black);
        DrawText("While running on Desktop Web Browsers,\ninspect and turn on Touch Emulation.",
            (int)_messagePosition.X + 15, (int)_messagePosition.Y + 35, 10, Color.Black);

        // Draw last gesture
        DrawText("Last gesture", (int)_lastGesturePosition.X + 33, (int)_lastGesturePosition.Y - 47, 20, Color.Black);
        DrawText("Swipe         Tap       Pinch  Touch", (int)_lastGesturePosition.X + 17,
            (int)_lastGesturePosition.Y - 18, 10, Color.Black);
        DrawRectangle((int)_lastGesturePosition.X + 20, (int)_lastGesturePosition.Y, 20, 20,
            _lastGesture == Gesture.SwipeUp ? Color.Red : Color.LightGray);
        DrawRectangle((int)_lastGesturePosition.X, (int)_lastGesturePosition.Y + 20, 20, 20,
            _lastGesture == Gesture.SwipeLeft ? Color.Red : Color.LightGray);
        DrawRectangle((int)_lastGesturePosition.X + 40, (int)_lastGesturePosition.Y + 20, 20, 20,
            _lastGesture == Gesture.SwipeRight ? Color.Red : Color.LightGray);
        DrawRectangle((int)_lastGesturePosition.X + 20, (int)_lastGesturePosition.Y + 40, 20, 20,
            _lastGesture == Gesture.SwipeDown ? Color.Red : Color.LightGray);
        DrawCircle((int)_lastGesturePosition.X + 80, (int)_lastGesturePosition.Y + 16, 10,
            _lastGesture == Gesture.Tap ? Color.Blue : Color.LightGray);
        DrawRing(new Vector2(
            _lastGesturePosition.X + 103, _lastGesturePosition.Y + 16
        ), 6.0f, 11.0f, 0.0f, 360.0f, 0, _lastGesture == Gesture.Drag ? Color.Lime : Color.LightGray);
        DrawCircle((int)_lastGesturePosition.X + 80, (int)_lastGesturePosition.Y + 43, 10,
            _lastGesture == Gesture.DoubleTap ? Color.SkyBlue : Color.LightGray);
        DrawCircle((int)_lastGesturePosition.X + 103, (int)_lastGesturePosition.Y + 43, 10,
            _lastGesture == Gesture.DoubleTap ? Color.SkyBlue : Color.LightGray);
        DrawTriangle(new Vector2(
            _lastGesturePosition.X + 122, _lastGesturePosition.Y + 16
        ), new Vector2(
            _lastGesturePosition.X + 137, _lastGesturePosition.Y + 26
        ), new Vector2(
            _lastGesturePosition.X + 137, _lastGesturePosition.Y + 6
        ), _lastGesture == Gesture.PinchOut ? Color.Orange : Color.LightGray);
        DrawTriangle(new Vector2(
            _lastGesturePosition.X + 147, _lastGesturePosition.Y + 6
        ), new Vector2(
            _lastGesturePosition.X + 147, _lastGesturePosition.Y + 26
        ), new Vector2(
            _lastGesturePosition.X + 162, _lastGesturePosition.Y + 16
        ), _lastGesture == Gesture.PinchOut ? Color.Orange : Color.Gray);
        DrawTriangle(new Vector2(
            _lastGesturePosition.X + 125, _lastGesturePosition.Y + 33
        ), new Vector2(
            _lastGesturePosition.X + 125, _lastGesturePosition.Y + 53
        ), new Vector2(
            _lastGesturePosition.X + 140, _lastGesturePosition.Y + 43
        ), _lastGesture == Gesture.PinchIn ? Color.Violet : Color.LightGray);
        DrawTriangle(new Vector2(
            _lastGesturePosition.X + 144, _lastGesturePosition.Y + 43
        ), new Vector2(
            _lastGesturePosition.X + 159, _lastGesturePosition.Y + 53
        ), new Vector2(
            _lastGesturePosition.X + 159, _lastGesturePosition.Y + 33
        ), _lastGesture == Gesture.PinchIn ? Color.Violet : Color.LightGray);
        for (i = 0; i < 4; i++)
        {
            DrawCircle((int)_lastGesturePosition.X + 180, (int)_lastGesturePosition.Y + 7 + i * 15, 5,
                touchCount <= i ? Color.LightGray : _gestureColor);
        }

        // Draw gesture log
        DrawText("Log", (int)_gestureLogPosition.X, (int)_gestureLogPosition.Y, 20, Color.Black);

        // Loop in both directions to print the gesture log array in the inverted order (and looping around if the index started somewhere in the middle)
        for (i = 0, ii = _gestureLogIndex; i < GESTURE_LOG_SIZE; i++, ii = (ii + 1) % GESTURE_LOG_SIZE)
        {
            DrawText(_gestureLog[ii], (int)_gestureLogPosition.X, (int)_gestureLogPosition.Y + 410 - i * 20, 20,
                (i == 0 ? _gestureColor : Color.LightGray));
        }

        Color logButton1Color, logButton2Color;
        switch (_logMode)
        {
            case 3:
                logButton1Color = Color.Maroon;
                logButton2Color = Color.Maroon;
                break;
            case 2:
                logButton1Color = Color.Gray;
                logButton2Color = Color.Maroon;
                break;
            case 1:
                logButton1Color = Color.Maroon;
                logButton2Color = Color.Gray;
                break;
            default:
                logButton1Color = Color.Gray;
                logButton2Color = Color.Gray;
                break;
        }

        DrawRectangleRec(_logButton1, logButton1Color);
        DrawText("Hide", (int)_logButton1.X + 7, (int)_logButton1.Y + 3, 10, Color.White);
        DrawText("Repeat", (int)_logButton1.X + 7, (int)_logButton1.Y + 13, 10, Color.White);
        DrawRectangleRec(_logButton2, logButton2Color);
        DrawText("Hide", (int)_logButton1.X + 62, (int)_logButton1.Y + 3, 10, Color.White);
        DrawText("Hold", (int)_logButton1.X + 62, (int)_logButton1.Y + 13, 10, Color.White);

        // Draw protractor
        DrawText("Angle", (int)_protractorPosition.X + 55, (int)_protractorPosition.Y + 76, 10, Color.Black);

        // Note: Official it's using raylibs functions for string manipulation. But in C# it will end up in an unsafe handling.
        string angleString = _currentAngleDegrees.ToString("F3");
        int angleStringDot = angleString.IndexOf('.');
        string angleStringTrim = angleString.Substring(0, angleStringDot + 3);

        DrawText(angleStringTrim, (int)_protractorPosition.X + 55, (int)_protractorPosition.Y + 92, 20, _gestureColor);
        DrawCircleV(_protractorPosition, 80.0f, Color.White);
        DrawLineEx(new Vector2(
            _protractorPosition.X - 90, _protractorPosition.Y
        ), new Vector2(
            _protractorPosition.X + 90, _protractorPosition.Y
        ), 3.0f, Color.LightGray);
        DrawLineEx(new Vector2(
            _protractorPosition.X, _protractorPosition.Y - 90
        ), new Vector2(
            _protractorPosition.X, _protractorPosition.Y + 90
        ), 3.0f, Color.LightGray);
        DrawLineEx(new Vector2(
            _protractorPosition.X - 80, _protractorPosition.Y - 45
        ), new Vector2(
            _protractorPosition.X + 80, _protractorPosition.Y + 45
        ), 3.0f, Color.Green);
        DrawLineEx(new Vector2(
            _protractorPosition.X - 80, _protractorPosition.Y + 45
        ), new Vector2(
            _protractorPosition.X + 80, _protractorPosition.Y - 45
        ), 3.0f, Color.Green);
        DrawText("0", (int)_protractorPosition.X + 96, (int)_protractorPosition.Y - 9, 20, Color.Black);
        DrawText("30", (int)_protractorPosition.X + 74, (int)_protractorPosition.Y - 68, 20, Color.Black);
        DrawText("90", (int)_protractorPosition.X - 11, (int)_protractorPosition.Y - 110, 20, Color.Black);
        DrawText("150", (int)_protractorPosition.X - 100, (int)_protractorPosition.Y - 68, 20, Color.Black);
        DrawText("180", (int)_protractorPosition.X - 124, (int)_protractorPosition.Y - 9, 20, Color.Black);
        DrawText("210", (int)_protractorPosition.X - 100, (int)_protractorPosition.Y + 50, 20, Color.Black);
        DrawText("270", (int)_protractorPosition.X - 18, (int)_protractorPosition.Y + 92, 20, Color.Black);
        DrawText("330", (int)_protractorPosition.X + 72, (int)_protractorPosition.Y + 50, 20, Color.Black);
        if (_currentAngleDegrees != 0.0f)
        {
            DrawLineEx(_protractorPosition, _finalVector, 3.0f, _gestureColor);
        }

        // Draw touch and mouse pointer points
        if (currentGesture != Gesture.None)
        {
            if (touchCount != 0)
            {
                for (i = 0; i < touchCount; i++)
                {
                    DrawCircleV(touchPosition[i], 50.0f, Fade(_gestureColor, 0.5f));
                    DrawCircleV(touchPosition[i], 5.0f, _gestureColor);
                }

                if (touchCount == 2)
                {
                    DrawLineEx(touchPosition[0], touchPosition[1], (((int)currentGesture == 512) ? 8.0f : 12.0f),
                        _gestureColor);
                }
            }
            else
            {
                DrawCircleV(mousePosition, 35.0f, Fade(_gestureColor, 0.5f));
                DrawCircleV(mousePosition, 5.0f, _gestureColor);
            }
        }

        EndDrawing();
    }

    public void Unload()
    {
    }

    private static string GetGestureName(int gesture)
    {
        switch (gesture)
        {
            case 0:
                return "None";
            case 1:
                return "Tap";
            case 2:
                return "Double Tap";
            case 4:
                return "Hold";
            case 8:
                return "Drag";
            case 16:
                return "Swipe Right";
            case 32:
                return "Swipe Left";
            case 64:
                return "Swipe Up";
            case 128:
                return "Swipe Down";
            case 256:
                return "Pinch In";
            case 512:
                return "Pinch Out";
            default:
                return "Unknown";
        }
    }

    // Get color for gesture value
    private static Color GetGestureColor(int gesture)
    {
        switch (gesture)
        {
            case 0:
                return Color.Black;
            case 1:
                return Color.Blue;
            case 2:
                return Color.SkyBlue;
            case 4:
                return Color.Black;
            case 8:
                return Color.Lime;
            case 16:
                return Color.Red;
            case 32:
                return Color.Red;
            case 64:
                return Color.Red;
            case 128:
                return Color.Red;
            case 256:
                return Color.Violet;
            case 512:
                return Color.Orange;
            default:
                return Color.Black;
        }
    }
}
