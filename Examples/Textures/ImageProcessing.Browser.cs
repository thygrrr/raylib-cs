#if BROWSER
using Examples;
namespace Examples.Textures;

public partial class ImageProcessing : IExample
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
        public string Name => "Textures / Image Processing";

        private const int NumProcesses = 9;

        private const int screenWidth = 800;
        private const int screenHeight = 450;

        private enum ImageProcess
        {
            None = 0,
            ColorGrayScale,
            ColorTint,
            ColorInvert,
            ColorContrast,
            ColorBrightness,
            GaussianBlur,
            FlipVertical,
            FlipHorizontal
        }

        private static readonly string[] processText = {
            "NO PROCESSING",
            "COLOR GRAYSCALE",
            "COLOR TINT",
            "COLOR INVERT",
            "COLOR CONTRAST",
            "COLOR BRIGHTNESS",
            "GAUSSIAN BLUR",
            "FLIP VERTICAL",
            "FLIP HORIZONTAL"
        };

        private Image _imageOrigin;
        private Image _imageCopy;
        private Texture2D _texture;

        private ImageProcess _currentProcess;
        private bool _textureReload;

        private Rectangle[] _toggleRecs;
        private int _mouseHoverRec;

        public void Init()
        {
            // NOTE: Textures MUST be loaded after Window initialization (OpenGL context is required)
            _imageOrigin = LoadImage("resources/parrots.png");
            ImageFormat(ref _imageOrigin, PixelFormat.UncompressedR8G8B8A8);
            _texture = LoadTextureFromImage(_imageOrigin);

            _imageCopy = ImageCopy(_imageOrigin);

            _currentProcess = ImageProcess.None;
            _textureReload = false;

            _toggleRecs = new Rectangle[NumProcesses];
            _mouseHoverRec = -1;

            for (int i = 0; i < NumProcesses; i++)
            {
                _toggleRecs[i] = new Rectangle(40, 50 + 32 * i, 150, 30);
            }
        }

        public void Update()
        {
            // Mouse toggle group logic
            for (int i = 0; i < NumProcesses; i++)
            {
                if (CheckCollisionPointRec(GetMousePosition(), _toggleRecs[i]))
                {
                    _mouseHoverRec = i;

                    if (IsMouseButtonReleased(MouseButton.Left))
                    {
                        _currentProcess = (ImageProcess)i;
                        _textureReload = true;
                    }
                    break;
                }
                else
                {
                    _mouseHoverRec = -1;
                }
            }

            // Keyboard toggle group logic
            if (IsKeyPressed(KeyboardKey.Down))
            {
                _currentProcess++;
                if ((int)_currentProcess > (NumProcesses - 1))
                {
                    _currentProcess = 0;
                }

                _textureReload = true;
            }
            else if (IsKeyPressed(KeyboardKey.Up))
            {
                _currentProcess--;
                if (_currentProcess < 0)
                {
                    _currentProcess = ImageProcess.FlipHorizontal;
                }

                _textureReload = true;
            }

            if (_textureReload)
            {
                UnloadImage(_imageCopy);
                _imageCopy = ImageCopy(_imageOrigin);

                // NOTE: Image processing is a costly CPU process to be done every frame,
                // If image processing is required in a frame-basis, it should be done
                // with a texture and by shaders
                switch (_currentProcess)
                {
                    case ImageProcess.ColorGrayScale:
                        ImageColorGrayscale(ref _imageCopy);
                        break;
                    case ImageProcess.ColorTint:
                        ImageColorTint(ref _imageCopy, Color.Green);
                        break;
                    case ImageProcess.ColorInvert:
                        ImageColorInvert(ref _imageCopy);
                        break;
                    case ImageProcess.ColorContrast:
                        ImageColorContrast(ref _imageCopy, -40);
                        break;
                    case ImageProcess.ColorBrightness:
                        ImageColorBrightness(ref _imageCopy, -80);
                        break;
                    case ImageProcess.GaussianBlur:
                        ImageBlurGaussian(ref _imageCopy, 10);
                        break;
                    case ImageProcess.FlipVertical:
                        ImageFlipVertical(ref _imageCopy);
                        break;
                    case ImageProcess.FlipHorizontal:
                        ImageFlipHorizontal(ref _imageCopy);
                        break;
                    default:
                        break;
                }

                // Get pixel data from image (RGBA 32bit)
                Color* pixels = LoadImageColors(_imageCopy);
                UpdateTexture(_texture, pixels);
                UnloadImageColors(pixels);

                _textureReload = false;
            }

            BeginDrawing();
            ClearBackground(Color.RayWhite);

            DrawText("IMAGE PROCESSING:", 40, 30, 10, Color.DarkGray);

            // Draw rectangles
            for (int i = 0; i < NumProcesses; i++)
            {
                DrawRectangleRec(_toggleRecs[i], (i == (int)_currentProcess) ? Color.SkyBlue : Color.LightGray);
                DrawRectangleLines(
                    (int)_toggleRecs[i].X,
                    (int)_toggleRecs[i].Y,
                    (int)_toggleRecs[i].Width,
                    (int)_toggleRecs[i].Height,
                    (i == (int)_currentProcess) ? Color.Blue : Color.Gray
                );

                int labelX = (int)(_toggleRecs[i].X + _toggleRecs[i].Width / 2);
                DrawText(
                    processText[i],
                    (int)(labelX - MeasureText(processText[i], 10) / 2),
                    (int)_toggleRecs[i].Y + 11,
                    10,
                    (i == (int)_currentProcess) ? Color.DarkBlue : Color.DarkGray
                );
            }

            int x = screenWidth - _texture.Width - 60;
            int y = screenHeight / 2 - _texture.Height / 2;
            DrawTexture(_texture, x, y, Color.White);
            DrawRectangleLines(x, y, _texture.Width, _texture.Height, Color.Black);

            EndDrawing();
        }

        public void Unload()
        {
            UnloadTexture(_texture);
            UnloadImage(_imageOrigin);
            UnloadImage(_imageCopy);
        }
    }
}
#endif
