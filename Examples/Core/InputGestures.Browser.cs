#if BROWSER
using Examples;
namespace Examples.Core;

public partial class InputGestures : IExample
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
        public string Name => "Core / Input Gestures";

        private const int MaxGestureStrings = 20;
        private const int screenWidth = 800;
        private const int screenHeight = 450;

        private Vector2 _touchPosition;
        private Rectangle _touchArea;
        private int _gesturesCount;
        private string[] _gestureStrings;
        private Gesture _currentGesture;
        private Gesture _lastGesture;

        public void Init()
        {
            _touchPosition = new Vector2(0, 0);
            _touchArea = new Rectangle(220, 10, screenWidth - 230, screenHeight - 20);

            _gesturesCount = 0;
            _gestureStrings = new string[MaxGestureStrings];

            _currentGesture = Gesture.None;
            _lastGesture = Gesture.None;

            // SetGesturesEnabled(0b0000000000001001);   // Enable only some gestures to be detected
        }

        public void Update()
        {
            _lastGesture = _currentGesture;
            _currentGesture = GetGestureDetected();
            _touchPosition = GetTouchPosition(0);

            if (CheckCollisionPointRec(_touchPosition, _touchArea) && (_currentGesture != Gesture.None))
            {
                if (_currentGesture != _lastGesture)
                {
                    // Store gesture string
                    switch (_currentGesture)
                    {
                        case Gesture.Tap:
                            _gestureStrings[_gesturesCount] = "GESTURE TAP";
                            break;
                        case Gesture.DoubleTap:
                            _gestureStrings[_gesturesCount] = "GESTURE DOUBLETAP";
                            break;
                        case Gesture.Hold:
                            _gestureStrings[_gesturesCount] = "GESTURE HOLD";
                            break;
                        case Gesture.Drag:
                            _gestureStrings[_gesturesCount] = "GESTURE DRAG";
                            break;
                        case Gesture.SwipeRight:
                            _gestureStrings[_gesturesCount] = "GESTURE SWIPE RIGHT";
                            break;
                        case Gesture.SwipeLeft:
                            _gestureStrings[_gesturesCount] = "GESTURE SWIPE LEFT";
                            break;
                        case Gesture.SwipeUp:
                            _gestureStrings[_gesturesCount] = "GESTURE SWIPE UP";
                            break;
                        case Gesture.SwipeDown:
                            _gestureStrings[_gesturesCount] = "GESTURE SWIPE DOWN";
                            break;
                        case Gesture.PinchIn:
                            _gestureStrings[_gesturesCount] = "GESTURE PINCH IN";
                            break;
                        case Gesture.PinchOut:
                            _gestureStrings[_gesturesCount] = "GESTURE PINCH OUT";
                            break;
                        default:
                            break;
                    }

                    _gesturesCount++;

                    // Reset gestures strings
                    if (_gesturesCount >= MaxGestureStrings)
                    {
                        for (int i = 0; i < MaxGestureStrings; i++)
                        {
                            _gestureStrings[i] = " ";
                        }
                        _gesturesCount = 0;
                    }
                }
            }

            BeginDrawing();
            ClearBackground(Color.RayWhite);

            DrawRectangleRec(_touchArea, Color.Gray);
            DrawRectangle(225, 15, screenWidth - 240, screenHeight - 30, Color.RayWhite);

            DrawText("GESTURES TEST AREA", screenWidth - 270, screenHeight - 40, 20, ColorAlpha(Color.Gray, 0.5f));

            for (int i = 0; i < _gesturesCount; i++)
            {
                if (i % 2 == 0)
                {
                    DrawRectangle(10, 30 + 20 * i, 200, 20, ColorAlpha(Color.LightGray, 0.5f));
                }
                else
                {
                    DrawRectangle(10, 30 + 20 * i, 200, 20, ColorAlpha(Color.LightGray, 0.3f));
                }

                if (i < _gesturesCount - 1)
                {
                    DrawText(_gestureStrings[i], 35, 36 + 20 * i, 10, Color.DarkGray);
                }
                else
                {
                    DrawText(_gestureStrings[i], 35, 36 + 20 * i, 10, Color.Maroon);
                }
            }

            DrawRectangleLines(10, 29, 200, screenHeight - 50, Color.Gray);
            DrawText("DETECTED GESTURES", 50, 15, 10, Color.Gray);

            if (_currentGesture != Gesture.None)
            {
                DrawCircleV(_touchPosition, 30, Color.Maroon);
            }

            EndDrawing();
        }

        public void Unload()
        {
        }
    }
}
#endif
