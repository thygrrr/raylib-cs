// Adapted for the browser from Examples/Textures/DrawTiled.cs
namespace Examples.Web;

public class TexturesDrawTiled : IWebExample
{
    public string Name => "Textures / Draw Tiled";

    private const int OptWidth = 220;
    private const int MarginSize = 8;
    private const int ColorSize = 16;

    private int _screenWidth = 800;
    private int _screenHeight = 450;

    private Texture2D _texPattern;

    // Coordinates for all patterns inside the texture
    private Rectangle[] _recPattern;

    // Setup colors
    private Color[] _colors;
    private Rectangle[] _colorRec;

    private int _activePattern;
    private int _activeCol;
    private float _scale;
    private float _rotation;

    public void Init()
    {
        // NOTE: Textures MUST be loaded after Window initialization (OpenGL context is required)
        _texPattern = LoadTexture("resources/patterns.png");

        // Makes the texture smoother when upscaled
        SetTextureFilter(_texPattern, TextureFilter.Trilinear);

        // Coordinates for all patterns inside the texture
        _recPattern = new[] {
            new Rectangle(3, 3, 66, 66),
            new Rectangle(75, 3, 100, 100),
            new Rectangle(3, 75, 66, 66),
            new Rectangle(7, 156, 50, 50),
            new Rectangle(85, 106, 90, 45),
            new Rectangle(75, 154, 100, 60)
        };

        // Setup colors
        _colors = new[]
        {
            Color.Black,
            Color.Maroon,
            Color.Orange,
            Color.Blue,
            Color.Purple,
            Color.Beige,
            Color.Lime,
            Color.Red,
            Color.DarkGray,
            Color.SkyBlue
        };
        _colorRec = new Rectangle[_colors.Length];

        // Calculate rectangle for each color
        for (int i = 0, x = 0, y = 0; i < _colors.Length; i++)
        {
            _colorRec[i].X = 2 + MarginSize + x;
            _colorRec[i].Y = 22 + 256 + MarginSize + y;
            _colorRec[i].Width = ColorSize * 2;
            _colorRec[i].Height = ColorSize;

            if (i == (_colors.Length / 2 - 1))
            {
                x = 0;
                y += ColorSize + MarginSize;
            }
            else
            {
                x += (ColorSize * 2 + MarginSize);
            }
        }

        _activePattern = 0;
        _activeCol = 0;
        _scale = 1.0f;
        _rotation = 0.0f;
    }

    public void Update()
    {
        _screenWidth = GetScreenWidth();
        _screenHeight = GetScreenHeight();

        // Handle mouse
        if (IsMouseButtonPressed(MouseButton.Left))
        {
            Vector2 mouse = GetMousePosition();

            // Check which pattern was clicked and set it as the active pattern
            for (int i = 0; i < _recPattern.Length; i++)
            {
                Rectangle rec = new(
                    2 + MarginSize + _recPattern[i].X,
                    40 + MarginSize + _recPattern[i].Y,
                    _recPattern[i].Width,
                    _recPattern[i].Height
                );
                if (CheckCollisionPointRec(mouse, rec))
                {
                    _activePattern = i;
                    break;
                }
            }

            // Check to see which color was clicked and set it as the active color
            for (int i = 0; i < _colors.Length; ++i)
            {
                if (CheckCollisionPointRec(mouse, _colorRec[i]))
                {
                    _activeCol = i;
                    break;
                }
            }
        }

        // Handle keys

        // Change scale
        if (IsKeyPressed(KeyboardKey.Up))
        {
            _scale += 0.25f;
        }
        if (IsKeyPressed(KeyboardKey.Down))
        {
            _scale -= 0.25f;
        }
        if (_scale > 10.0f)
        {
            _scale = 10.0f;
        }
        else if (_scale <= 0.0f)
        {
            _scale = 0.25f;
        }

        // Change rotation
        if (IsKeyPressed(KeyboardKey.Left))
        {
            _rotation -= 25.0f;
        }
        if (IsKeyPressed(KeyboardKey.Right))
        {
            _rotation += 25.0f;
        }

        // Reset
        if (IsKeyPressed(KeyboardKey.Space))
        {
            _rotation = 0.0f;
            _scale = 1.0f;
        }

        BeginDrawing();
        ClearBackground(Color.RayWhite);

        // Draw the tiled area
        Rectangle source = _recPattern[_activePattern];
        Rectangle dest = new(
            OptWidth + MarginSize,
            MarginSize,
            _screenWidth - OptWidth - 2 * MarginSize,
            _screenHeight - 2 * MarginSize
        );
        DrawTextureTiled(_texPattern, source, dest, Vector2.Zero, _rotation, _scale, _colors[_activeCol]);

        // Draw options
        Color color = ColorAlpha(Color.LightGray, 0.5f);
        DrawRectangle(MarginSize, MarginSize, OptWidth - MarginSize, _screenHeight - 2 * MarginSize, color);

        DrawText("Select Pattern", 2 + MarginSize, 30 + MarginSize, 10, Color.Black);
        DrawTexture(_texPattern, 2 + MarginSize, 40 + MarginSize, Color.Black);
        DrawRectangle(
            2 + MarginSize + (int)_recPattern[_activePattern].X,
            40 + MarginSize + (int)_recPattern[_activePattern].Y,
            (int)_recPattern[_activePattern].Width,
            (int)_recPattern[_activePattern].Height,
            ColorAlpha(Color.DarkBlue, 0.3f)
        );

        DrawText("Select Color", 2 + MarginSize, 10 + 256 + MarginSize, 10, Color.Black);
        for (int i = 0; i < _colors.Length; i++)
        {
            DrawRectangleRec(_colorRec[i], _colors[i]);
            if (_activeCol == i)
            {
                DrawRectangleLinesEx(_colorRec[i], 3, ColorAlpha(Color.White, 0.5f));
            }
        }

        DrawText("Scale (UP/DOWN to change)", 2 + MarginSize, 80 + 256 + MarginSize, 10, Color.Black);
        DrawText($"{_scale}x", 2 + MarginSize, 92 + 256 + MarginSize, 20, Color.Black);

        DrawText("Rotation (LEFT/RIGHT to change)", 2 + MarginSize, 122 + 256 + MarginSize, 10, Color.Black);
        DrawText($"{_rotation} degrees", 2 + MarginSize, 134 + 256 + MarginSize, 20, Color.Black);

        DrawText("Press [SPACE] to reset", 2 + MarginSize, 164 + 256 + MarginSize, 10, Color.DarkBlue);

        // Draw FPS
        DrawText($"{GetFPS()}", 2 + MarginSize, 2 + MarginSize, 20, Color.Black);
        EndDrawing();
    }

    public void Unload()
    {
        UnloadTexture(_texPattern);
    }

    // Draw part of a texture (defined by a rectangle) with rotation and scale tiled into dest.
    private static void DrawTextureTiled(
        Texture2D texture,
        Rectangle source,
        Rectangle dest,
        Vector2 origin,
        float rotation,
        float scale,
        Color tint
    )
    {
        if ((texture.Id <= 0) || (scale <= 0.0f))
        {
            // Wanna see a infinite loop?!...just delete this line!
            return;
        }

        if ((source.Width == 0) || (source.Height == 0))
        {
            return;
        }

        int tileWidth = (int)(source.Width * scale), tileHeight = (int)(source.Height * scale);
        if ((dest.Width < tileWidth) && (dest.Height < tileHeight))
        {
            // Can fit only one tile
            DrawTexturePro(
                texture,
                new Rectangle(
                    source.X,
                    source.Y,
                    ((float)dest.Width / tileWidth) * source.Width,
                    ((float)dest.Height / tileHeight) * source.Height
                ),
                new Rectangle(dest.X, dest.Y, dest.Width, dest.Height), origin, rotation, tint
            );
        }
        else if (dest.Width <= tileWidth)
        {
            // Tiled vertically (one column)
            int dy = 0;
            for (; dy + tileHeight < dest.Height; dy += tileHeight)
            {
                DrawTexturePro(
                    texture,
                    new Rectangle(
                        source.X,
                        source.Y,
                        ((float)dest.Width / tileWidth) * source.Width,
                        source.Height
                    ),
                    new Rectangle(dest.X, dest.Y + dy, dest.Width, (float)tileHeight),
                    origin,
                    rotation,
                    tint
                );
            }

            // Fit last tile
            if (dy < dest.Height)
            {
                DrawTexturePro(
                    texture,
                    new Rectangle(
                        source.X,
                        source.Y,
                        ((float)dest.Width / tileWidth) * source.Width,
                        ((float)(dest.Height - dy) / tileHeight) * source.Height
                    ),
                    new Rectangle(dest.X, dest.Y + dy, dest.Width, dest.Height - dy),
                    origin,
                    rotation,
                    tint
                );
            }
        }
        else if (dest.Height <= tileHeight)
        {
            // Tiled horizontally (one row)
            int dx = 0;
            for (; dx + tileWidth < dest.Width; dx += tileWidth)
            {
                DrawTexturePro(
                    texture,
                    new Rectangle(
                        source.X,
                        source.Y,
                        source.Width,
                        ((float)dest.Height / tileHeight) * source.Height
                    ),
                    new Rectangle(dest.X + dx, dest.Y, (float)tileWidth, dest.Height),
                    origin,
                    rotation,
                    tint
                );
            }

            // Fit last tile
            if (dx < dest.Width)
            {
                DrawTexturePro(
                    texture,
                    new Rectangle(
                        source.X,
                        source.Y,
                        ((float)(dest.Width - dx) / tileWidth) * source.Width,
                        ((float)dest.Height / tileHeight) * source.Height
                    ),
                    new Rectangle(
                        dest.X + dx,
                        dest.Y,
                        dest.Width - dx,
                        dest.Height
                    ),
                    origin,
                    rotation,
                    tint
                );
            }
        }
        else
        {
            // Tiled both horizontally and vertically (rows and columns)
            int dx = 0;
            for (; dx + tileWidth < dest.Width; dx += tileWidth)
            {
                int dy = 0;
                for (; dy + tileHeight < dest.Height; dy += tileHeight)
                {
                    DrawTexturePro(
                        texture,
                        source,
                        new Rectangle(
                            dest.X + dx,
                            dest.Y + dy,
                            (float)tileWidth,
                            (float)tileHeight
                        ),
                        origin,
                        rotation,
                        tint
                    );
                }

                if (dy < dest.Height)
                {
                    DrawTexturePro(
                        texture,
                        new Rectangle(
                            source.X,
                            source.Y,
                            source.Width,
                            ((float)(dest.Height - dy) / tileHeight) * source.Height
                        ),
                        new Rectangle(
                            dest.X + dx,
                            dest.Y + dy,
                            (float)tileWidth, dest.Height - dy
                        ),
                        origin,
                        rotation,
                        tint
                    );
                }
            }

            // Fit last column of tiles
            if (dx < dest.Width)
            {
                int dy = 0;
                for (; dy + tileHeight < dest.Height; dy += tileHeight)
                {
                    DrawTexturePro(
                        texture,
                        new Rectangle(
                            source.X,
                            source.Y,
                            ((float)(dest.Width - dx) / tileWidth) * source.Width,
                            source.Height
                        ),
                        new Rectangle(
                            dest.X + dx,
                            dest.Y + dy,
                            dest.Width - dx,
                            (float)tileHeight
                        ),
                        origin,
                        rotation,
                        tint
                    );
                }

                // Draw final tile in the bottom right corner
                if (dy < dest.Height)
                {
                    DrawTexturePro(
                        texture,
                        new Rectangle(
                            source.X,
                            source.Y,
                            ((float)(dest.Width - dx) / tileWidth) * source.Width,
                            ((float)(dest.Height - dy) / tileHeight) * source.Height
                        ),
                        new Rectangle(
                            dest.X + dx,
                            dest.Y + dy,
                            dest.Width - dx,
                            dest.Height - dy
                        ),
                        origin,
                        rotation,
                        tint
                    );
                }
            }
        }
    }
}
