#if BROWSER
using Examples;
namespace Examples.Textures;

public partial class MousePainting : IExample
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
        public string Name => "Textures / Mouse Painting";

        private const int screenWidth = 800;
        private const int screenHeight = 450;

        // Colours to choose from
        private Color[] _colors;

        // Define colorsRecs data (for every rectangle)
        private Rectangle[] _colorsRecs;

        private int _colorSelected;
        private int _colorSelectedPrev;
        private int _colorMouseHover;
        private int _brushSize;

        private Rectangle _btnSaveRec;
        private bool _btnSaveMouseHover;
        private bool _showSaveMessage;
        private int _saveMessageCounter;

        // RenderTexture2D used as a canvas
        private RenderTexture2D _target;

        public void Init()
        {
            // Colours to choose from
            _colors = new Color[] {
                Color.RayWhite,
                Color.Yellow,
                Color.Gold,
                Color.Orange,
                Color.Pink,
                Color.Red,
                Color.Maroon,
                Color.Green,
                Color.Lime,
                Color.DarkGreen,
                Color.SkyBlue,
                Color.Blue,
                Color.DarkBlue,
                Color.Purple,
                Color.Violet,
                Color.DarkPurple,
                Color.Beige,
                Color.Brown,
                Color.DarkBrown,
                Color.LightGray,
                Color.Gray,
                Color.DarkGray,
                Color.Black
            };

            // Define colorsRecs data (for every rectangle)
            _colorsRecs = new Rectangle[_colors.Length];

            for (int i = 0; i < _colorsRecs.Length; i++)
            {
                _colorsRecs[i].X = 10 + 30 * i + 2 * i;
                _colorsRecs[i].Y = 10;
                _colorsRecs[i].Width = 30;
                _colorsRecs[i].Height = 30;
            }

            _colorSelected = 0;
            _colorSelectedPrev = _colorSelected;
            _colorMouseHover = 0;
            _brushSize = 20;

            _btnSaveRec = new(750, 10, 40, 30);
            _btnSaveMouseHover = false;
            _showSaveMessage = false;
            _saveMessageCounter = 0;

            // Create a RenderTexture2D to use as a canvas
            _target = LoadRenderTexture(screenWidth, screenHeight);

            // Clear render texture before entering the game loop
            BeginTextureMode(_target);
            ClearBackground(_colors[0]);
            EndTextureMode();
        }

        public void Update()
        {
            Vector2 mousePos = GetMousePosition();

            // Move between colors with keys
            if (IsKeyPressed(KeyboardKey.Right))
            {
                _colorSelected++;
            }
            else if (IsKeyPressed(KeyboardKey.Left))
            {
                _colorSelected--;
            }

            if (_colorSelected >= _colors.Length)
            {
                _colorSelected = _colors.Length - 1;
            }
            else if (_colorSelected < 0)
            {
                _colorSelected = 0;
            }

            // Choose color with mouse
            for (int i = 0; i < _colors.Length; i++)
            {
                if (CheckCollisionPointRec(mousePos, _colorsRecs[i]))
                {
                    _colorMouseHover = i;
                    break;
                }
                else
                {
                    _colorMouseHover = -1;
                }
            }

            if ((_colorMouseHover >= 0) && IsMouseButtonPressed(MouseButton.Left))
            {
                _colorSelected = _colorMouseHover;
                _colorSelectedPrev = _colorSelected;
            }

            // Change brush size
            _brushSize += (int)(GetMouseWheelMove() * 5);
            if (_brushSize < 2)
            {
                _brushSize = 2;
            }

            if (_brushSize > 50)
            {
                _brushSize = 50;
            }

            if (IsKeyPressed(KeyboardKey.C))
            {
                // Clear render texture to clear color
                BeginTextureMode(_target);
                ClearBackground(_colors[0]);
                EndTextureMode();
            }

            if (IsMouseButtonDown(MouseButton.Left))
            {
                // Paint circle into render texture
                // NOTE: To avoid discontinuous circles, we could store
                // previous-next mouse points and just draw a line using brush size
                BeginTextureMode(_target);
                if (mousePos.Y > 50)
                {
                    DrawCircle((int)mousePos.X, (int)mousePos.Y, _brushSize, _colors[_colorSelected]);
                }

                EndTextureMode();
            }
            else if (IsMouseButtonDown(MouseButton.Right))
            {
                _colorSelected = 0;

                // Erase circle from render texture
                BeginTextureMode(_target);
                if (mousePos.Y > 50)
                {
                    DrawCircle((int)mousePos.X, (int)mousePos.Y, _brushSize, _colors[0]);
                }

                EndTextureMode();
            }
            else
            {
                _colorSelected = _colorSelectedPrev;
            }

            // Check mouse hover save button
            if (CheckCollisionPointRec(mousePos, _btnSaveRec))
            {
                _btnSaveMouseHover = true;
            }
            else
            {
                _btnSaveMouseHover = false;
            }

            // Image saving logic
            // NOTE: Saving painted texture to a default named image
            if ((_btnSaveMouseHover && IsMouseButtonReleased(MouseButton.Left)) ||
                IsKeyPressed(KeyboardKey.S))
            {
                Image image = LoadImageFromTexture(_target.Texture);
                ImageFlipVertical(ref image);
                ExportImage(image, "my_amazing_texture_painting.png");
                UnloadImage(image);
                _showSaveMessage = true;
            }

            if (_showSaveMessage)
            {
                // On saving, show a full screen message for 2 seconds
                _saveMessageCounter++;
                if (_saveMessageCounter > 240)
                {
                    _showSaveMessage = false;
                    _saveMessageCounter = 0;
                }
            }

            BeginDrawing();
            ClearBackground(Color.RayWhite);

            // NOTE: Render texture must be y-flipped due to default OpenGL coordinates (left-bottom)
            Rectangle source = new(0, 0, _target.Texture.Width, -_target.Texture.Height);
            DrawTextureRec(_target.Texture, source, new Vector2(0, 0), Color.White);

            // Draw drawing circle for reference
            if (mousePos.Y > 50)
            {
                if (IsMouseButtonDown(MouseButton.Right))
                {
                    DrawCircleLines((int)mousePos.X, (int)mousePos.Y, _brushSize, _colors[_colorSelected]);
                }
                else
                {
                    DrawCircle(GetMouseX(), GetMouseY(), _brushSize, _colors[_colorSelected]);
                }
            }

            // Draw top panel
            DrawRectangle(0, 0, GetScreenWidth(), 50, Color.RayWhite);
            DrawLine(0, 50, GetScreenWidth(), 50, Color.LightGray);

            // Draw color selection rectangles
            for (int i = 0; i < _colors.Length; i++)
            {
                DrawRectangleRec(_colorsRecs[i], _colors[i]);
            }

            DrawRectangleLines(10, 10, 30, 30, Color.LightGray);

            if (_colorMouseHover >= 0)
            {
                DrawRectangleRec(_colorsRecs[_colorMouseHover], ColorAlpha(Color.White, 0.6f));
            }

            Rectangle rec = new(
                _colorsRecs[_colorSelected].X - 2,
                _colorsRecs[_colorSelected].Y - 2,
                _colorsRecs[_colorSelected].Width + 4,
                _colorsRecs[_colorSelected].Height + 4
            );
            DrawRectangleLinesEx(rec, 2, Color.Black);

            // Draw save image button
            DrawRectangleLinesEx(_btnSaveRec, 2, _btnSaveMouseHover ? Color.Red : Color.Black);
            DrawText("SAVE!", 755, 20, 10, _btnSaveMouseHover ? Color.Red : Color.Black);

            // Draw save image message
            if (_showSaveMessage)
            {
                DrawRectangle(0, 0, GetScreenWidth(), GetScreenHeight(), ColorAlpha(Color.RayWhite, 0.8f));
                DrawRectangle(0, 150, GetScreenWidth(), 80, Color.Black);
                DrawText("IMAGE SAVED:  my_amazing_texture_painting.png", 150, 180, 20, Color.RayWhite);
            }

            EndDrawing();
        }

        public void Unload()
        {
            UnloadRenderTexture(_target);
        }
    }
}
#endif
