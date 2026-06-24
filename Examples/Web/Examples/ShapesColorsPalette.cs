// Adapted for the browser from Examples/Shapes/ColorsPalette.cs
namespace Examples.Web;

public class ShapesColorsPalette : IWebExample
{
    public string Name => "Shapes / Colors Palette";

    private Color[] _colors;
    private string[] _colorNames;
    private Rectangle[] _colorsRecs;
    private int[] _colorState;
    private Vector2 _mousePoint;

    public void Init()
    {
        _colors = new[]
        {
            Color.DarkGray,
            Color.Maroon,
            Color.Orange,
            Color.DarkGreen,
            Color.DarkBlue,
            Color.DarkPurple,
            Color.DarkBrown,
            Color.Gray,
            Color.Red,
            Color.Gold,
            Color.Lime,
            Color.Blue,
            Color.Violet,
            Color.Brown,
            Color.LightGray,
            Color.Pink,
            Color.Yellow,
            Color.Green,
            Color.SkyBlue,
            Color.Purple,
            Color.Beige
        };

        _colorNames = new[]
        {
            "DARKGRAY",
            "MAROON",
            "ORANGE",
            "DARKGREEN",
            "DARKBLUE",
            "DARKPURPLE",
            "DARKBROWN",
            "GRAY",
            "RED",
            "GOLD",
            "LIME",
            "BLUE",
            "VIOLET",
            "BROWN",
            "LIGHTGRAY",
            "PINK",
            "YELLOW",
            "GREEN",
            "SKYBLUE",
            "PURPLE",
            "BEIGE"
        };

        // Rectangles array
        _colorsRecs = new Rectangle[_colors.Length];

        // Fills colorsRecs data (for every rectangle)
        for (int i = 0; i < _colorsRecs.Length; i++)
        {
            _colorsRecs[i].X = 20 + 100 * (i % 7) + 10 * (i % 7);
            _colorsRecs[i].Y = 80 + 100 * (i / 7) + 10 * (i / 7);
            _colorsRecs[i].Width = 100;
            _colorsRecs[i].Height = 100;
        }

        // Color state: 0-DEFAULT, 1-MOUSE_HOVER
        _colorState = new int[_colors.Length];

        _mousePoint = new Vector2(0.0f, 0.0f);
    }

    public void Update()
    {
        _mousePoint = GetMousePosition();

        for (int i = 0; i < _colors.Length; i++)
        {
            if (CheckCollisionPointRec(_mousePoint, _colorsRecs[i]))
            {
                _colorState[i] = 1;
            }
            else
            {
                _colorState[i] = 0;
            }
        }

        BeginDrawing();
        ClearBackground(Color.RayWhite);

        DrawText("raylib colors palette", 28, 42, 20, Color.Black);
        DrawText(
            "press SPACE to see all colors",
            GetScreenWidth() - 180,
            GetScreenHeight() - 40,
            10,
            Color.Gray
        );

        // Draw all rectangles
        for (int i = 0; i < _colorsRecs.Length; i++)
        {
            DrawRectangleRec(_colorsRecs[i], ColorAlpha(_colors[i], _colorState[i] != 0 ? 0.6f : 1.0f));

            if (IsKeyDown(KeyboardKey.Space) || _colorState[i] != 0)
            {
                DrawRectangle(
                    (int)_colorsRecs[i].X,
                    (int)(_colorsRecs[i].Y + _colorsRecs[i].Height - 26),
                    (int)_colorsRecs[i].Width,
                    20,
                    Color.Black
                );
                DrawRectangleLinesEx(_colorsRecs[i], 6, ColorAlpha(Color.Black, 0.3f));
                DrawText(
                    _colorNames[i],
                    (int)(_colorsRecs[i].X + _colorsRecs[i].Width - MeasureText(_colorNames[i], 10) - 12),
                    (int)(_colorsRecs[i].Y + _colorsRecs[i].Height - 20),
                    10,
                    _colors[i]
                );
            }
        }

        EndDrawing();
    }

    public void Unload()
    {
    }
}
